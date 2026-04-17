# EverDrive64 SD Repair

Build quickly:

```powershell
dotnet clean
dotnet restore
dotnet build
dotnet run
```

# EverDrive64 SD Repair

## Build instructions

Open a terminal in the project folder and run:

```powershell
dotnet clean
dotnet restore
dotnet build
dotnet run
```

## Publish an EXE

```powershell
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
```

Published EXE location:

```text
bin\Release\net6.0-windows\win-x64\publish\EverDrive64SdRepair.exe
```

## Important

- Run the EXE as **Administrator** for formatting to work.
- Confirm `Tools\\fat32format.exe` is present in the publish output.
- This project targets **.NET 6**.

## What this build includes

- Designer-based WinForms UI
- Back up and restore for `ED64`, `THEMES`, `N64`, and `TOOLS`
- First-time SD card setup
- Krikzz OS version loading
- FAT32 formatting via bundled `fat32format.exe`
- Formatter confirmation handling (`y` sent automatically)
- Activity log output for formatter path, arguments, stdout, and stderr


## Scan behavior
- The Scan button now logs each directory and file as it is scanned.
- The activity log shows PASS/FAIL for required items, directories, and files.
- The scan also computes a SHA-256 hash for readable files and logs it.
- Fragmentation analysis is not implemented in this build.
