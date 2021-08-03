using Communication.Match;
using InstantMultiplayer.Communication;
using UnityEngine;

namespace InstantMultiplayer.UnityIntegration.Controllers
{
    public class TextMessageController : BaseMessageController<MessageText>
    {
        public static TextMessageController Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new TextMessageController();
                return _instance;
            }
        }
        private static TextMessageController _instance;

        public override void HandleMessage(MessageText message)
        {
            Debug.Log("RECIEVED MESSAGE: " + message.Message);
        }

        public override bool TryGetMessage(out IMessage message)
        {
            message = null;
            return false;
        }
    }
}
