# Apple Music RPC's WatchDog
WatchDog is an service implementation that enables support for Apple Music Preview Windows app on [Apple-Music-RPC](https://github.com/zephraOSS/Apple-Music-RPC).

# Install
Go to the [releases page](https://github.com/zephraOSS/AMRPC-WatchDog/releases/tag/latest) for install instructions.

# TO-DO 
- [x] Build the websocket service to export Windows playing info on a websocket
- [x] Build the desktop version
- [x] Fix the fail to reconnect websocket when watchdog keeps running but connection is closed
- [x] Add "Autostart with system" option
- [x] Add "Start minimized" option 
- [x] Add "Quit" to context menu on notify icon
- [x] Fix thumbnailPath null value
- [ ] Fix triple websocket messages bug on song info changed 
