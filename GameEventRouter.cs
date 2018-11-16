using System;
using UnityEngine;
using UnityEngine.Events;

namespace UnityExtensions
{

    public abstract class GameEventRouter<
        TMessage,
        TGameEvent,
        TGameEventAction>
    : MonoBehaviour, IGameEventReceiver<TMessage>
    where TGameEvent : GameEvent<TMessage>
    where TGameEventAction : GameEventAction<TMessage>
    {

        public TGameEvent gameEvent;

        public TGameEventAction actions;

        private void OnEnable() => gameEvent?.Add(this);

        private void OnDisable() => gameEvent?.Remove(this);

        public void Receive(GameEvent gameEvent, TMessage message)
        {
            actions?.Invoke(message);
        }

    }

}