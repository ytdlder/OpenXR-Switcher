# OpenXR Switcher

Easily switch between OpenXR runtimes and toggle layers
<p align="center">
  <a href="https://github.com/ytdlder/OpenXR-Switcher/releases/">Download<br><br><img src="icon3.png"/></a><br>
</p>


## Description

Reads the Windows registry which OpenXR runtimes/layers are installed, checks if these .json descriptor files are really present (or stale) and lets you enable/disable them quickly.

*(-> A PC restart is not required, as those keys are read on VR launch)*

## Getting Started

### Dependencies

* Windows 7, 10, 11
* for whatever reasons, Visual Studio no longer creates a stand-alone, all-in-one EXE, so the .Net Runtime **has to be installed** 
* .NET 6.0 Runtime
  * [Microsoft Runtime Download](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
  * under **.NET Desktop Runtime 6.x.x**, next to **Windows**, click on your arch type (x86, x64) to download the newest runtime
  * __Direct Link__: [Latest x64 Runtime](https://aka.ms/dotnet/6.0/windowsdesktop-runtime-win-x64.exe)
* !! REQUIRES ADMIN RIGHTS !! *(-> the reg keys are located in _HKLM_!)*

### Installing

* no installation needed
* just unzip and start the EXE

### HowTo - GUI

* if runtimes or layers are not found, they are displayed with an italic font on a yellowish background 
* the active runtime and any activated layer will have a green background
* otherwise, the background will be red
* the runtime can be switched to any inactive one
* layers can be en-/disabled as needed *(I've added clearer names for known layers like OpenKneeboard)*
* all names can be renamed to ones liking

### HowTo - CLI

* the program can also be started via CMD and will let you switch to another runtime without starting the GUI:
* `"OpenXR Switcher.exe" /h`

* you don't need to specify the exact JSON file name *(eg. "steamxr_win64.json")*, but some text that can be found
* like: `"OpenXR Switcher.exe" steam`
* or:   `"OpenXR Switcher.exe" steamvr`

## Errors

* if the registry key (HKLM\SOFTWARE\Khronos) cannot be found or written to, you'll get an error message and the program will quit

## Version History

* 1.2
	* added Rename function, that allows for custom names
	* these are added to the local users registry (HKCU\Software\Hotshot\OpenXRSwitcher)
* 1.1
	* added Refresh button
	* added recognized layers 
* 1.0
	* initial release

## License

This project is licensed under the GNU GENERAL PUBLIC LICENSE v3 - see the LICENSE.txt file for details

## Acknowledgments

Inspiration (as well as some icons^^) from this awesome dude:
* [OpenXR Runtime Switcher](https://github.com/WaGi-Coding/OpenXR-Runtime-Switcher/)
