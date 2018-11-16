using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityExtensions
{

    public abstract class PointerClickHandler
    : PointerEventHandler
    , IPointerClickHandler
    , IPointerDownHandler
    , IPointerUpHandler
    {

        public abstract void OnPointerClick(PointerEventData eventData);

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData) { }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData) { }

    }

}