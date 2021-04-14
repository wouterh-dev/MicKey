# MicKey

MicKey is a small standalone Windows utility to toggle the microphone system-wide using a shortcut.

## Usage

Download the application from the GitHub releases and run it.

It will appear in the system tray:

| Muted | Unmuted |
|-------|---------|
| ![](https://i.imgur.com/kvcM6fP.png) | ![](https://i.imgur.com/lPkQttW.png) |

Click the tray icon to show the main window, or right click the tray icon to exit the application.

| Muted | Unmuted |
|-------|---------|
| ![](https://i.imgur.com/FdU24y0.png) | ![](https://i.imgur.com/FyIijBj.png) |

Here you can set the global hotkey, which works even if the application is minimized, or toggle the mute.

The window will always show itself on top of other windows, as it is intended to be used as an indicator to see whether the microphone is currently muted or not. You can close the window to return it to the tray, or minimize it to keep the task bar indicator:

| Muted | Unmuted |
|-------|---------|
| ![](https://i.imgur.com/TRqcNAp.png) | ![](https://i.imgur.com/6StjHTh.png) |

Tip: By default the hotkey is set to F24, which most keyboards tend not to have. It may be useful to bind this key to your mouse buttons!

## Automatically launch on startup

To automatically launch this program on startup:

1. Press Win+R to open the Run dialog and type `shell:startup` then press enter

2. Right click in the resulting folder and choose New -> Shortcut

3. Make a shortcut to your `MicKey.exe`

## How does it work?

This program mutes the currently active microphone for communication on the operating
system level, which works for all communication software that is configured to use
the default microphone.

To see this setting:

1. Find Sound Settings in the start menu
2. Click Sound Control Panel under Related Settings
3. Click the Recording tab
4. Locate the microphone set as default recording device
5. Right click -> Properties
6. Click the Levels tab
7. The ![](https://i.imgur.com/clLKw4L.png) button shows if muted