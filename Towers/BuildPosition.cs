using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class BuildPosition : MonoBehaviour, IPointerDownHandler
{
    public TowerAsset[] m_BuildableTowers;
    public void SetBuildableTowers(TowerAsset[] towers) 
    { 
        if (towers == null || towers.Length == 0)
        {
            Destroy(transform.parent.gameObject);
        }
        m_BuildableTowers = towers; 
    }
    public static event Action<BuildPosition> OnClickEvent;
    public static void HideControls()
    {
        OnClickEvent(null);
    }
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        Sound.Click.Play();
        OnClickEvent(this);
    }

}
