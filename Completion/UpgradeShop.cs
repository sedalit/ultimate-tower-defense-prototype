using UnityEngine;
using UnityEngine.UI;

public class UpgradeShop : MonoBehaviour
{
    
    [SerializeField] private int m_Score;
    [SerializeField] private Text m_ScoreText;
    [SerializeField] private BuyUpgrade[] m_UpgradesOnSale;
    private void Start()
    {
        foreach (var slot in m_UpgradesOnSale)
        {
            slot.Initialise();
            slot.transform.Find("DescriptionPanel").GetComponentInChildren<Button>().onClick.AddListener(UpdateScore);
        }
        UpdateScore();
    }
    public void UpdateScore()
    {
        m_Score = MapCompletion.Instance.TotalScore;
        m_Score -= Upgrades.GetTotalCost();
        m_ScoreText.text = m_Score.ToString();
        foreach (var slot in m_UpgradesOnSale)
        {
            slot.CheckCost(m_Score);
        }
    }
        
    
}
