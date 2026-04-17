EverDrive64 X7 SD Tools



A Windows utility for preparing, repairing, and managing SD cards used with the EverDrive64 X7.



Overview



EverDrive64 X7 SD Tools is designed to simplify SD card setup and maintenance. It provides scanning, formatting, backup, restore, and OS installation features in a single interface.



The tool ensures your SD card is correctly structured and ready for reliable use with the EverDrive64 X7.



Features

SD card scan with detailed logging (files, directories, pass/fail results)

FAT32 formatting using bundled fat32format

Automatic cluster size selection based on card size

Backup and restore:

/ED64 (system, saves, cheats, settings)

/THEMES

/N64 (game ROMs)

/TOOLS (utility ROMs)

OS version selection and installation

First-time SD card setup (creates required directory structure)

Activity log with real-time status output

Folder Structure



After setup, the SD card will contain:



/ED64

/ED64/save

/ED64/cheats

/ED64/sys

/ED64/emu

/ED64/patcher

/N64

/TOOLS

/THEMES

Requirements

Windows

.NET 6 Runtime (if using non-self-contained build)

Administrator privileges (required for formatting)

Build Instructions



From the project directory:



dotnet clean

dotnet restore

dotnet build

dotnet run

Build Executable



To publish:



dotnet publish -c Release -r win-x64 --self-contained false



Output:



bin\\Release\\net6.0-windows\\win-x64\\publish\\



For a standalone build:



dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true

Usage

Select the SD card drive

Run a scan to check integrity

Back up existing data (recommended)

Format the card if needed

Install the desired OS version

Restore backups or perform first-time setup

Notes

Always verify the selected drive before formatting

Do not remove the SD card during operations

Ensure fat32format.exe is present in the Tools directory

Run the application as Administrator for full functionality

License



Specify your license here (MIT, GPL, etc.)



Disclaimer



Use at your own risk. Formatting will permanently erase all data on the selected drive.

