using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
public class ClickProtection : SingletoneBase<ClickProtection>, IPointerClickHandler
{
    public Action<Vector2> m_OnClickAction;
    private Image m_Blocker;
    private void Start()
    {
        m_Blocker = GetComponent<Image>();
    }
    public void Activate(Action<Vector2> mouseAction)
    {
        m_Blocker.enabled = true;
        m_OnClickAction = mouseAction;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        m_Blocker.enabled = false;
        m_OnClickAction(eventData.pressPosition);
        m_OnClickAction = null;
    }
}