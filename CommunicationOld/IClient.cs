namespace InstantMultiplayer.Communication
{
    public interface IClient
    {
        bool TryRecieveMessage(out IMessage message);
        void SendMessage(IMessage message);
    }
}
