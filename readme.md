# Apple Music RPC's WatchDog
WatchDog is an service implementation that enables support for Apple Music Preview Windows app on [Apple-Music-RPC](https://github.com/zephraOSS/Apple-Music-RPC).

# Install
Go to the [releases page](https://github.com/zephraOSS/AMRPC-WatchDog/releases/tag/latest) for install instructions.

# TO-DO 
- [x] Build the websocket service to export Windows playing info on a websocket
- [x] Build the desktop version
- [ ] Fix the fail to reconnect websocket when watchdog keeps running but connection is reset
- [ ] Fix duplicated websocket messages on song info changed 
- [ ] Add "Autostart with system" option
- [ ] Add "Start minimized" option 