using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleSmbTester;

namespace SimpleSmbTester.Tests
{
    [TestClass]
    public class SmbTestServiceTests
    {
        [TestMethod]
        public void TryCreateProbeFile_creates_text_file_in_requested_folder()
        {
            var method = GetTryCreateProbeFileMethod();
            var store = new RecordingStore();
            var args = new object[] { store, "scanner\\drop", null, null, null };

            var success = (bool)method.Invoke(null, args);

            Assert.IsTrue(success, "Expected write probe creation to succeed.");
            Assert.AreEqual("scanner\\drop\\smb-test-probe.txt", store.CreatedPath, "Expected the write probe to target the requested folder.");
            CollectionAssert.AreEqual(System.Text.Encoding.UTF8.GetBytes("Simple SMB Tester write probe\r\n"), store.WrittenBytes);
            Assert.AreEqual(1, store.CloseCallCount, "Expected the probe handle to be closed.");
        }

        [TestMethod]
        public void TryCreateProbeFile_returns_not_supported_when_store_lacks_write_methods()
        {
            var method = GetTryCreateProbeFileMethod();
            var args = new object[] { new object(), string.Empty, null, null, null };

            var success = (bool)method.Invoke(null, args);

            Assert.IsFalse(success, "Expected write probe creation to fail when the SMB store lacks file methods.");
            Assert.AreEqual("STATUS_NOT_SUPPORTED", args[2].ToString());
            StringAssert.Contains(args[3].ToString(), "did not expose the expected file-store methods");
        }

        private static MethodInfo GetTryCreateProbeFileMethod()
        {
            var type = typeof(Form1).Assembly.GetType("SimpleSmbTester.SmbTestService", throwOnError: true);
            var method = type.GetMethod("TryCreateProbeFile", BindingFlags.Static | BindingFlags.NonPublic);
            Assert.IsNotNull(method, "Expected SmbTestService.TryCreateProbeFile to exist.");
            return method;
        }

        private sealed class RecordingStore
        {
            public string CreatedPath { get; private set; }
            public byte[] WrittenBytes { get; private set; }
            public int CloseCallCount { get; private set; }

            public object LastHandle { get; private set; }

            public SMBLibrary.NTStatus CreateFile(out object handle, out SMBLibrary.FileStatus fileStatus, string path, SMBLibrary.AccessMask desiredAccess, SMBLibrary.FileAttributes fileAttributes, SMBLibrary.ShareAccess shareAccess, SMBLibrary.CreateDisposition createDisposition, SMBLibrary.CreateOptions createOptions, object securityContext)
            {
                CreatedPath = path;
                handle = new object();
                LastHandle = handle;
                fileStatus = SMBLibrary.FileStatus.FILE_CREATED;
                return SMBLibrary.NTStatus.STATUS_SUCCESS;
            }

            public SMBLibrary.NTStatus WriteFile(out int numberOfBytesWritten, object handle, long offset, byte[] data)
            {
                numberOfBytesWritten = data.Length;
                WrittenBytes = data;
                return SMBLibrary.NTStatus.STATUS_SUCCESS;
            }

            public SMBLibrary.NTStatus CloseFile(object handle)
            {
                if (!ReferenceEquals(handle, LastHandle))
                {
                    throw new InvalidOperationException("Unexpected handle.");
                }

                CloseCallCount++;
                return SMBLibrary.NTStatus.STATUS_SUCCESS;
            }
        }
    }
}
