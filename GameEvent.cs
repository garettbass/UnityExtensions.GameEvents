using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityExtensions
{

    public abstract class GameEvent : ScriptableObject
    {

        private GameEventReceivers<object> m_receivers;

        internal GameEvent() { }

        public abstract Type messageType { get; }

        public void Add(IGameEventReceiver<object> receiver)
        {
            m_receivers.Add(receiver);
        }

        public void Remove(IGameEventReceiver<object> receiver)
        {
            m_receivers.Remove(receiver);
        }

        public void Send(object message)
        {
            SendBase(message);
            SendDerived(message);
        }

        internal void SendBase(object message)
        {
            GameEvents.Send(this, message);
            m_receivers.Send(this, message);
        }

        internal abstract void SendDerived(object message);

    }

    //==========================================================================

    public class GameEvent<TMessage> : GameEvent
    {

        private GameEventReceivers<TMessage> m_receivers;

        public sealed override Type messageType => typeof(TMessage);

        public void Add(IGameEventReceiver<TMessage> receiver)
        {
            m_receivers.Add(receiver);
        }

        public void Remove(IGameEventReceiver<TMessage> receiver)
        {
            m_receivers.Remove(receiver);
        }

        public void Send(TMessage message)
        {
            SendBase(message);
            SendDerived(message);
        }

        private void SendDerived(TMessage message)
        {
            m_receivers.Send(this, message);
        }

        internal sealed override void SendDerived(object message)
        {
            m_receivers.Send(this, (TMessage)message);
        }

    }

}