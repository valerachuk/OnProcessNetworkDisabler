# About

OnProcessNetworkDisabler is a simple utility written on C# that disables all enabled network adapters when any of specified processes are running. Disabled network adapters will be enabled when processes are not running.

# Config

Configuration file should be in the same folder with executable file, named "pnd.cfg"

## Example **pnd.config**:

```
msgBoxes
delay: 123
proc: SnippingTool notepad
```

- **msgBoxes** [optional] - show info and error messages in MessageBoxes.
  - Such as `You have to write processes in pnd.cfg` if list of processes is empty.
  - Or `Disabled 2 interfaces` when network adapters was disabled.
- **delay: {integer}** [optional] - delay between checks of running processes in milliseconds. Default value is 2000 ms.
- **proc: {processName1} {processName2}** [required] - list of processes. When any of them is running network adapters will be disabled.

# Use cases

- If some app doesn’t require internet access but uses it to, for example, showing ads.
- You can use someone's steam library, but when he starts using his library you will be kicked out.

To avoid this this app disables network adapters when, for example, `witcher3.exe` or `offlineGameWithAds.exe` is running.

# Notes

- Interacting with network adapters implemented via cli utility `netsh`, simply parsing its output with regex.
- You can't launch two or more instances of this app; second instance will exit immediately with `Already running!` message.
- 'Inventing' configuration file by myself were not good idea. I had to use json.
- This app is not a service it is just windows application that runs without GUI.
- If app was launched via .exe file, to close it you have to kill the process in a task manager.
