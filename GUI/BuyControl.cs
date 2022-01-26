using UnityEngine;
using System.Collections.Generic;

public class BuyControl : MonoBehaviour
{
    [SerializeField] private TowerBuyControl m_TowerBuyPrefab;
    private List<TowerBuyControl> m_ActiveControls;
    private RectTransform m_GUIRectTransform;

    //-- События Юнити
    private void Awake()
    {
        m_GUIRectTransform = GetComponent<RectTransform>();
        BuildPosition.OnClickEvent += MoveToBuildSite;
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        BuildPosition.OnClickEvent -= MoveToBuildSite;
    }
    // -- //
    private void MoveToBuildSite(BuildPosition buildPosition) 
    {
        if (buildPosition)
        {
            var position = Camera.main.WorldToScreenPoint(buildPosition.transform.root.position);
            m_GUIRectTransform.anchoredPosition = position;
            gameObject.SetActive(true);
            m_ActiveControls = new List<TowerBuyControl>();
            foreach (var asset in buildPosition.m_BuildableTowers)
            {
                if (asset.IsAvaliable())
                {
                    var newControl = Instantiate(m_TowerBuyPrefab, transform);
                    m_ActiveControls.Add(newControl);
                    newControl.SetTowerAsset(asset);
                }
            }
            if (m_ActiveControls.Count > 0)
            {
                var angle = 360 / m_ActiveControls.Count;
                for (int i = 0; i < m_ActiveControls.Count; i++)
                {
                    var offset = Quaternion.AngleAxis(angle * i, Vector3.forward) * (Vector3.up * 120);
                    m_ActiveControls[i].transform.position += offset;
                }
            }
            foreach (var tbc in GetComponentsInChildren<TowerBuyControl>())
            {
                tbc.SetBuildPosition(buildPosition.transform.root);
            }

        } else 
        {
            if (m_ActiveControls != null)
            {
                foreach (var control in m_ActiveControls)
                {
                    if (control != null)
                    {
                        Destroy(control);
                    }
                }
                m_ActiveControls.Clear();
            }
            gameObject.SetActive(false);
        }
    }
    
}
