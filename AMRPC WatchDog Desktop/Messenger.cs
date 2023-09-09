using Newtonsoft.Json;
using WebSocketSharp;
using WebSocketSharp.Server;


namespace AMRPC_WatchDog_Desktop
{
    internal class Messenger
    {
        private readonly Payload _payload;
        public Messenger(Payload payload)
        {
            var sender = new Sender { Payload = payload };
            sender.Payload.PropertyChanged += sender.OnPayloadChanged;
            _payload = payload;
            _payload.PropertyChanged += sender.OnPayloadChanged;
            
            var server = new WebSocketServer(9632);
            server.AddWebSocketService<Sender>("/watchdog", () => sender);
            server.Start();
        }
    }
    
    internal class Sender : WebSocketBehavior
    {
        public Payload Payload;

        protected override void OnMessage (MessageEventArgs e)
        {
            Send(JsonConvert.SerializeObject(Payload));
        }
        
        public void OnPayloadChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Send(JsonConvert.SerializeObject(Payload));
        }
    }
}