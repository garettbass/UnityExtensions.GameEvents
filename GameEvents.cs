namespace UnityExtensions
{

    public static class GameEvents
    {

        private static GameEventReceivers<object> s_receivers;

        public static void Add(IGameEventReceiver<object> receiver)
        {
            s_receivers.Add(receiver);
        }

        public static void Send(GameEvent gameEvent, object message)
        {
            s_receivers.Send(gameEvent, message);
        }

        public static void Remove(IGameEventReceiver<object> receiver)
        {
            s_receivers.Remove(receiver);
        }

    }

}