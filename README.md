# RDPQuickAccess

Opens RDP session based on filename, host name or IP address for existing RDP files in the specified directory or a new one if an existing one can't be found.

## Settings
The settings file is located at `%appdata\RDPQuickAccess\settings.xml`

`RDPFileSearchPath`    
- Type: `string`
- Default Value: `.`
- Description: The directory path to search for the existing RDP files. Searches in all child directories. Supports environment variables.

`ExitOnSuccess`    
- Type: `bool`
- Default Value: `true`
- Description: Close RDPQuickAccess if an existing RDP file is found or if a new one is started.

`RDPApplicationPath`    
- Type: `string`
- Default Value: `%windir%\system32\mstsc.exe`
- Description: The path to the windows RDP application. Supports environment variables.

## Command Line Arguments
The first command line argument is used as the search term and the search is triggered when the application starts.

### URI Scheme
You can register the URI scheme `rdpquickaccess` so that this application can be started by another application e.g. web browser.

To start with a web browser you would type the URL `rdpquickaccess:<search term>`.

The following registry keys need to be created:

- Computer\HKEY_CLASSES_ROOT\RDPQuickAccess
  - `URL Protocol`, `REG_SZ`, `<empty>`
  - shell\open\command
    - `(Default)`, `REG_SZ`, `<path to .exe> "%1"`
