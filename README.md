# RPDManager

Opens RDP session based on filename, host name or IP address for existing RDP files in the specified directory or a new one if an existing one can't be found.

## Settings
The settings file is located at `%appdata\RDPManager\settings.xml`

`RDPFileSearchPath`    
- Type: `string`
- Default Value: `.`
- Description: The directory path to search for the existing RDP files. Searches in all child directories. Supports environment variables.

`ExitOnSuccess`    
- Type: `bool`
- Default Value: `true`
- Description: Close RDPManager if an existing RDP file is found or if a new one is started.

`RDPApplicationPath`    
- Type: `string`
- Default Value: `%windir%\system32\mstsc.exe`
- Description: The path to the windows RDP application. Supports environment variables.