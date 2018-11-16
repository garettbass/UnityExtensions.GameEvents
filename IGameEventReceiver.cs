namespace UnityExtensions
{

    public interface IGameEventReceiver<TMessage>
    {
        void Receive(GameEvent gameEvent, TMessage message);
    }

}