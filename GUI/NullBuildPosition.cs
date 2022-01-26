using UnityEngine;
using UnityEngine.EventSystems;

public class NullBuildPosition : BuildPosition
{
    public override void OnPointerDown(PointerEventData eventData)
    {
        HideControls();
    }
}
