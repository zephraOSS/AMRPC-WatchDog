using Newtonsoft.Json;
using WebSocketSharp;
using WebSocketSharp.Server;


namespace AMRPC_WatchDog_Desktop
{
    internal class Messenger
    {
        private readonly Payload _payload;
        private WebSocketServer _server;
        private Sender _sender;
        public Messenger(Payload payload)
        {
            _payload = payload;
            Reconfigure();
        }

        public void Reconfigure()
        {
            if (_server != null)
            {
                _server.RemoveWebSocketService("/watchdog");
                _server.Stop();
                _server = null;
                _sender = null;
            }
            
            _sender = new Sender { Payload = _payload };
            _payload.PropertyChanged += _sender.OnPayloadChanged;
            _sender.Messenger = this;

            _server = new WebSocketServer(9632);
            _server.AddWebSocketService<Sender>("/watchdog", () => _sender);
            _server.Start();
            
        }
    }
    
    internal class Sender : WebSocketBehavior
    {
        public Payload Payload;
        public Messenger Messenger;

        protected override void OnMessage(MessageEventArgs e)
        {
            AnswerAsType(Payload.ResponseTypes.Response);
        }
        
        public void OnPayloadChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            AnswerAsType(Payload.ResponseTypes.Event);
        }

        protected override void OnClose(CloseEventArgs e)
        {
            Messenger.Reconfigure();
        }

        private void AnswerAsType(string answerType)
        {
            var original = Payload.type;
            Payload.type = answerType;
            Send(JsonConvert.SerializeObject(Payload));
            Payload.type = original;
        }
    }
}