using System;

namespace UnityExtensions
{

    internal struct GameEventReceivers<TMessage>
    {

        private static readonly IGameEventReceiver<TMessage>[]
        EmptyArray = new IGameEventReceiver<TMessage>[0];

        private bool m_invocationInProgress;

        private int m_length;

        private IGameEventReceiver<TMessage>[] m_array;

        //----------------------------------------------------------------------

        public void Add(IGameEventReceiver<TMessage> r)
        {
            var oldIndex = IndexOf(r);
            if (oldIndex < 0)
            {
                var newIndex = m_length;
                Array.Resize(ref m_array, ++m_length);
                m_array[newIndex] = r;
            }
        }

        public void Remove(IGameEventReceiver<TMessage> r)
        {
            if (m_array == null)
                return;
            if (m_invocationInProgress)
                ReplaceWithNull(r);
            else
                RemoveFromArray(r);
        }

        public void Send(GameEvent gameEvent, TMessage message)
        {
            if (m_array == null)
                return;
            try
            {
                m_invocationInProgress = true;
                for (int i = 0, n = m_length; i < n; ++i)
                {
                    m_array[i]?.Receive(gameEvent, message);
                }
            }
            finally
            {
                RemoveNullsFromArray();
                m_invocationInProgress = false;
            }
        }

        //----------------------------------------------------------------------

        private void ReplaceWithNull(IGameEventReceiver<TMessage> r)
        {
            var oldIndex = IndexOf(r);
            if (oldIndex >= 0) m_array[oldIndex] = null;
        }

        private void RemoveFromArray(IGameEventReceiver<TMessage> r)
        {
            for (int o = 0, n = m_length; o < n; ++o)
            {
                if (m_array[o] == r)
                {
                    for (int i = o + 1; i < n; ++o, ++i)
                    {
                        m_array[o] = m_array[i];
                    }
                    m_length -= 1;
                    return;
                }
            }
        }

        private void RemoveNullsFromArray()
        {
            for (int o = 0, i = 1, n = m_length; o < n; ++o, ++i)
            {
                if (m_array[o] == null)
                {
                    // find next non-null receiver, and move it here
                    for (; i < n; ++i)
                    {
                        var t = m_array[i];
                        if (t != null)
                        {
                            m_array[o] = t;
                            m_array[i] = null;
                            break;
                        }
                    }
                }
                if (m_array[o] == null)
                {
                    // couldn't find any more receivers in list, truncate
                    m_length = o;
                    break;
                }
            }
        }

        private int IndexOf(IGameEventReceiver<TMessage> t)
        {
            if (m_array == null)
                return -1;

            return Array.IndexOf(m_array, t, 0, m_length);
        }

    }
    /**/

}
