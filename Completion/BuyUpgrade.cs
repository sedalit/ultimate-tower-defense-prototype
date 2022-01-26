using UnityEngine;
using UnityEngine.UI;

public class BuyUpgrade : MonoBehaviour
{
    [SerializeField] private UpgradeAsset m_Asset;
    [SerializeField] private Text m_CostText;
    [SerializeField] private Button m_BuyUpgradeButton;
    [SerializeField] private GameObject m_DescriptionPanel;
    [SerializeField] private BuyUpgrade m_PreviousUpgrade;
    private int m_Cost = 0;
    private void Start()
    {
        m_DescriptionPanel.SetActive(false);
    }
    public void Initialise()
    {
        var savedLevel = Upgrades.GetUpgradeLevel(m_Asset);
        if (savedLevel >= m_Asset.m_CostByLevel.Length)
        {
            m_Cost = m_Asset.m_CostByLevel[savedLevel - 1];
            m_BuyUpgradeButton.interactable = false;
            m_BuyUpgradeButton.transform.Find("CostPanel").gameObject.SetActive(false);
            m_DescriptionPanel.SetActive(false);
            m_Cost = int.MaxValue;
        }
        else
        {
            m_Cost = m_Asset.m_CostByLevel[savedLevel];
            m_CostText.text = m_Cost.ToString();
        }
        if (m_PreviousUpgrade != null)
        {
            var previousUpgradeLevel = Upgrades.GetUpgradeLevel(m_PreviousUpgrade.m_Asset);

            if (previousUpgradeLevel <= 0)
            {
                m_BuyUpgradeButton.interactable = false;
            }
        }
    }
    public void CheckCost(int m_Score)
    {
        m_BuyUpgradeButton.interactable = m_Score >= m_Cost;
    }
    public void Buy()
    {
        Upgrades.BuyUpgrade(m_Asset);
        Initialise();
        m_DescriptionPanel.SetActive(false);
        EnableAll();
    }
    public void ShowDescription()
    {
        m_DescriptionPanel.SetActive(true);
            foreach (var upgrade in FindObjectsOfType<BuyUpgrade>())
            {
                upgrade.m_BuyUpgradeButton.enabled = false;
            }
    }
    public void EnableAll()
    {
        foreach (var upgrade in FindObjectsOfType<BuyUpgrade>())
        {
            upgrade.m_BuyUpgradeButton.enabled = true;
        }
    }
}
