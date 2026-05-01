# Simple SMB Tester

Simple SMB Tester is a Windows desktop utility for checking whether a set of credentials can authenticate to a UNC share or folder path.

## Features

- Targets **.NET Framework 4.6**
- Tests **SMB 1**, **SMB 2**, and **SMB 3**
- Validates both authentication and the requested **share/folder path**
- Shows a clear **success/failure** result in the UI
- Includes a portable **single EXE** build at the repository root: `SimpleSmbTester.exe`

## Usage

1. Launch `SimpleSmbTester.exe`.
2. Choose the SMB version you want to test.
3. Enter a UNC path such as `\\server\share` or `\\server\share\folder`.
4. Enter the username and password.
5. Click **Test Credentials**.

## Project Files

- `SimpleSmbTester.csproj` - WinForms project
- `Form1.cs` / `Form1.Designer.cs` - main UI
- `SmbTestService.cs` - credential and path validation logic
- `AppLogo.png` / `AppIcon.ico` - branding assets

## Third-Party Licensing

This project uses **SMBLibrary** and also includes modified SMBLibrary-derived source files for exact SMB2/SMB3 dialect testing:

- `ExactSmb2Client.cs`
- `ExactSmb2FileStore.cs`
- `SMB2Cryptography.cs`
- `SP800_1008.cs`

See:

- `SMBLibrary-LICENSE.txt`
- `THIRD-PARTY-NOTICES.txt`
