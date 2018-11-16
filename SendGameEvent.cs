using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityExtensions
{

    public abstract class SendGameEvent : MonoBehaviour
    {
        internal SendGameEvent() { }
    }

    //==========================================================================

    public class SendGameEvent<TGameEvent> : SendGameEvent
    where TGameEvent : GameEvent
    {

        public TGameEvent gameEvent;

    }

}