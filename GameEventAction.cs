using System;
using UnityEngine;
using UnityEngine.Events;

namespace UnityExtensions
{

    [Serializable]
    public abstract class GameEventAction<TMessage> : UnityEvent<TMessage> { }

}