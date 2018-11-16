using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityExtensions
{

    public abstract class PointerEventHandler : MonoBehaviour
    {

        private static PhysicsRaycaster s_physicsRaycaster;

        protected virtual void Start()
        {
            if (s_physicsRaycaster == null)
            {
                var camera = Camera.main;
                var cameraObject = camera.gameObject;
                s_physicsRaycaster =
                    cameraObject.GetComponent<PhysicsRaycaster>() ??
                    cameraObject.AddComponent<PhysicsRaycaster>();
            }
            if (EventSystem.current == null)
            {
                new GameObject(
                    "EventSystem",
                    typeof(EventSystem),
                    typeof(StandaloneInputModule));
            }
        }

    }

}