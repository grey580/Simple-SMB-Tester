using System;
using System.Reflection;
using SMBLibrary;
using SMBLibrary.Client;
using SMBLibrary.SMB2;

namespace SimpleSmbTester
{
    internal enum SmbProtocolSelection
    {
        Smb1 = 0,
        Smb2 = 1,
        Smb3 = 2
    }

    internal sealed class SmbTestResult
    {
        public bool Success { get; set; }
        public string StatusText { get; set; }
        public string DetailsText { get; set; }
    }

    internal static class SmbTestService
    {
        public static SmbTestResult Test(string rawPath, string rawUsername, string password, SmbProtocolSelection protocol)
        {
            try
            {
                var pathInfo = SharePathInfo.Parse(rawPath);
                var credential = CredentialParts.Parse(rawUsername);

                switch (protocol)
                {
                    case SmbProtocolSelection.Smb1:
                        return TestSmb1(pathInfo, credential, password);
                    case SmbProtocolSelection.Smb2:
                        return TestSmb2Or3(pathInfo, credential, password, ExactSmbDialectFamily.Smb2Only, "SMB 2");
                    case SmbProtocolSelection.Smb3:
                        return TestSmb2Or3(pathInfo, credential, password, ExactSmbDialectFamily.Smb3Only, "SMB 3");
                    default:
                        return Fail("Unsupported SMB selection.", "Choose SMB 1, SMB 2, or SMB 3.");
                }
            }
            catch (Exception ex)
            {
                return Fail("Input or connection error.", ex.Message);
            }
        }

        private static SmbTestResult TestSmb1(SharePathInfo pathInfo, CredentialParts credential, string password)
        {
            SMB1Client client = null;
            object store = null;
            string transportLabel = "TCP 445";

            try
            {
                client = new SMB1Client(5000);
                var connected = client.Connect(pathInfo.ServerName, SMBTransportType.DirectTCPTransport);
                if (!connected)
                {
                    transportLabel = "NetBIOS over TCP 139";
                    connected = client.Connect(pathInfo.ServerName, SMBTransportType.NetBiosOverTCP);
                }

                if (!connected)
                {
                    return Fail("SMB 1 connection failed.", "The server did not accept an SMB 1 session on TCP 445 or NetBIOS over TCP 139.");
                }

                var loginStatus = client.Login(credential.Domain, credential.UserName, password);
                if (loginStatus != NTStatus.STATUS_SUCCESS)
                {
                    return Fail("SMB 1 login failed.", "Login status: " + loginStatus);
                }

                NTStatus treeStatus;
                store = client.TreeConnect(pathInfo.ShareName, out treeStatus);
                if (treeStatus != NTStatus.STATUS_SUCCESS || store == null)
                {
                    return Fail("Connected but could not open the share.", "Tree connect status: " + treeStatus);
                }

                NTStatus folderStatus;
                string folderMessage;
                if (!ValidateFolderAccess(store, pathInfo.RelativePath, out folderStatus, out folderMessage))
                {
                    return Fail("Credential worked, but the folder path was not accessible.", "Protocol: SMB 1\r\nTransport: " + transportLabel + "\r\nStatus: " + folderStatus + "\r\n" + folderMessage);
                }

                return Success("SMB 1 test succeeded.", "Protocol: SMB 1\r\nTransport: " + transportLabel + "\r\nShare: \\\\" + pathInfo.ServerName + "\\" + pathInfo.ShareName + "\r\nFolder: " + (string.IsNullOrEmpty(pathInfo.RelativePath) ? "<share root>" : pathInfo.RelativePath));
            }
            finally
            {
                SafeDisconnectStore(store);
                SafeLogoff(client);
                SafeDisconnect(client);
            }
        }

        private static SmbTestResult TestSmb2Or3(SharePathInfo pathInfo, CredentialParts credential, string password, ExactSmbDialectFamily dialectFamily, string requestedLabel)
        {
            ExactSmb2Client client = null;
            object store = null;

            try
            {
                client = new ExactSmb2Client(dialectFamily, 5000, true);
                if (!client.Connect(pathInfo.ServerName, SMBTransportType.DirectTCPTransport))
                {
                    return Fail(requestedLabel + " connection failed.", "The server did not negotiate " + requestedLabel + " on TCP 445.");
                }

                var loginStatus = client.Login(credential.Domain, credential.UserName, password);
                if (loginStatus != NTStatus.STATUS_SUCCESS)
                {
                    return Fail(requestedLabel + " login failed.", "Login status: " + loginStatus + "\r\nNegotiated dialect: " + client.NegotiatedDialect);
                }

                NTStatus treeStatus;
                store = client.TreeConnect(pathInfo.ShareName, out treeStatus);
                if (treeStatus != NTStatus.STATUS_SUCCESS || store == null)
                {
                    return Fail("Connected but could not open the share.", "Requested protocol: " + requestedLabel + "\r\nNegotiated dialect: " + client.NegotiatedDialect + "\r\nTree connect status: " + treeStatus);
                }

                NTStatus folderStatus;
                string folderMessage;
                if (!ValidateFolderAccess(store, pathInfo.RelativePath, out folderStatus, out folderMessage))
                {
                    return Fail("Credential worked, but the folder path was not accessible.", "Requested protocol: " + requestedLabel + "\r\nNegotiated dialect: " + client.NegotiatedDialect + "\r\nStatus: " + folderStatus + "\r\n" + folderMessage);
                }

                return Success(requestedLabel + " test succeeded.", "Requested protocol: " + requestedLabel + "\r\nNegotiated dialect: " + client.NegotiatedDialect + "\r\nShare: \\\\" + pathInfo.ServerName + "\\" + pathInfo.ShareName + "\r\nFolder: " + (string.IsNullOrEmpty(pathInfo.RelativePath) ? "<share root>" : pathInfo.RelativePath));
            }
            finally
            {
                SafeDisconnectStore(store);
                SafeLogoff(client);
                SafeDisconnect(client);
            }
        }

        private static bool ValidateFolderAccess(object store, string relativePath, out NTStatus status, out string message)
        {
            if (string.IsNullOrEmpty(relativePath))
            {
                status = NTStatus.STATUS_SUCCESS;
                message = "Share root opened successfully.";
                return true;
            }

            var createFile = store.GetType().GetMethod("CreateFile");
            var closeFile = store.GetType().GetMethod("CloseFile");
            if (createFile == null || closeFile == null)
            {
                status = NTStatus.STATUS_NOT_SUPPORTED;
                message = "The SMB library did not expose the expected file-store methods.";
                return false;
            }

            object handle = null;
            var arguments = new object[]
            {
                handle,
                default(FileStatus),
                relativePath,
                AccessMask.GENERIC_READ | AccessMask.SYNCHRONIZE,
                FileAttributes.Directory,
                ShareAccess.Read | ShareAccess.Write | ShareAccess.Delete,
                CreateDisposition.FILE_OPEN,
                CreateOptions.FILE_DIRECTORY_FILE | CreateOptions.FILE_SYNCHRONOUS_IO_NONALERT,
                null
            };

            status = (NTStatus)createFile.Invoke(store, arguments);
            handle = arguments[0];
            if (status == NTStatus.STATUS_SUCCESS && handle != null)
            {
                closeFile.Invoke(store, new[] { handle });
                message = "Folder opened successfully.";
                return true;
            }

            message = "The credential authenticated, but the folder path could not be opened. Verify the folder exists and that the account has access.";
            return false;
        }

        private static void SafeDisconnectStore(object store)
        {
            if (store == null)
            {
                return;
            }

            var disconnect = store.GetType().GetMethod("Disconnect");
            if (disconnect != null)
            {
                disconnect.Invoke(store, new object[0]);
            }
        }

        private static void SafeLogoff(object client)
        {
            if (client == null)
            {
                return;
            }

            var logoff = client.GetType().GetMethod("Logoff");
            if (logoff != null)
            {
                try
                {
                    logoff.Invoke(client, new object[0]);
                }
                catch
                {
                }
            }
        }

        private static void SafeDisconnect(object client)
        {
            if (client == null)
            {
                return;
            }

            var disconnect = client.GetType().GetMethod("Disconnect");
            if (disconnect != null)
            {
                disconnect.Invoke(client, new object[0]);
            }
        }

        private static SmbTestResult Success(string statusText, string detailsText)
        {
            return new SmbTestResult
            {
                Success = true,
                StatusText = statusText,
                DetailsText = detailsText
            };
        }

        private static SmbTestResult Fail(string statusText, string detailsText)
        {
            return new SmbTestResult
            {
                Success = false,
                StatusText = statusText,
                DetailsText = detailsText
            };
        }

        private sealed class CredentialParts
        {
            public string Domain { get; private set; }
            public string UserName { get; private set; }

            public static CredentialParts Parse(string rawUsername)
            {
                var value = (rawUsername ?? string.Empty).Trim();
                if (value.Length == 0)
                {
                    throw new InvalidOperationException("Username cannot be empty.");
                }

                var slashIndex = value.IndexOf('\\');
                if (slashIndex > 0)
                {
                    return new CredentialParts
                    {
                        Domain = value.Substring(0, slashIndex),
                        UserName = value.Substring(slashIndex + 1)
                    };
                }

                var atIndex = value.IndexOf('@');
                if (atIndex > 0)
                {
                    return new CredentialParts
                    {
                        Domain = value.Substring(atIndex + 1),
                        UserName = value.Substring(0, atIndex)
                    };
                }

                return new CredentialParts
                {
                    Domain = string.Empty,
                    UserName = value
                };
            }
        }

        private sealed class SharePathInfo
        {
            public string ServerName { get; private set; }
            public string ShareName { get; private set; }
            public string RelativePath { get; private set; }

            public static SharePathInfo Parse(string rawPath)
            {
                var value = (rawPath ?? string.Empty).Trim();
                if (!value.StartsWith(@"\\"))
                {
                    throw new InvalidOperationException("Use a UNC path such as \\\\server\\share or \\\\server\\share\\folder.");
                }

                value = value.TrimEnd('\\');
                var parts = value.Substring(2).Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 2)
                {
                    throw new InvalidOperationException("The path must include both a server name and a share name.");
                }

                return new SharePathInfo
                {
                    ServerName = parts[0],
                    ShareName = parts[1],
                    RelativePath = parts.Length > 2 ? string.Join("\\", parts, 2, parts.Length - 2) : string.Empty
                };
            }
        }
    }
}
