using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerBuyControl : MonoBehaviour
{
    [SerializeField] private TowerAsset m_TowerAsset;
    public void SetTowerAsset(TowerAsset asset) { m_TowerAsset = asset; }
    [SerializeField] private Text m_CostText;
    [SerializeField] private Button m_BuyTowerButton;
    [SerializeField] private Transform m_BuildPosition;
    
    private void Start() {
        TDPlayer.Instance.GoldUpdateSubscribe(GoldStatusCheck);
        m_CostText.text = m_TowerAsset.m_GoldCost.ToString();
        m_BuyTowerButton.GetComponent<Image>().sprite = m_TowerAsset.m_GUISprite;
    }

    private void GoldStatusCheck(int gold)
    {
        if(gold >= m_TowerAsset.m_GoldCost != m_BuyTowerButton.interactable)
        {
            m_BuyTowerButton.interactable = !m_BuyTowerButton.interactable;
            m_CostText.color = m_BuyTowerButton.interactable ? Color.white : Color.red;
        }
    }

    public void BuyTower()
    {
        TDPlayer.Instance.TryBuild(m_TowerAsset, m_BuildPosition);
        BuildPosition.HideControls();
    }
    public void SetBuildPosition(Transform value)
    {
        m_BuildPosition = value; 
    }
}
