using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityExtensions
{

    public class SendGameEventOnPointerClick : PointerClickHandler
    {

        public int clickCount = 1;

        public Transform worldPositionOverride;

        public Vector3GameEvent worldPositionGameEvent;

        public RaycastResultGameEvent raycastResultGameEvent;

        public override void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log($"OnPointerClick({eventData.position})");
            if (clickCount == 0 || clickCount == eventData.clickCount)
            {
                var worldPosition =
                    worldPositionOverride != null
                    ? worldPositionOverride.position
                    : eventData.pointerCurrentRaycast.worldPosition;
                worldPositionGameEvent?.Send(worldPosition);
                var raycastResult = eventData.pointerCurrentRaycast;
                raycastResultGameEvent?.Send(raycastResult);
            }
        }

    }

}