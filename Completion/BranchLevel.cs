using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MapLevel))]
public class BranchLevel : MonoBehaviour
{
    [SerializeField] private MapLevel m_RootLevel;
    [SerializeField] private int m_NeedScore = 3;
    [SerializeField] private Text m_NeedScoreText;
    public bool RootIsActive { get { return m_RootLevel.isCompleted; } }

    /// <summary>
    /// Попытка активации дополнительного уровня.
    /// Требует наличия очков и выполнения предыдущего уровня.
    /// </summary>
    public void TryActivate()
    {
        gameObject.SetActive(m_RootLevel.isCompleted);
        if (m_NeedScore > MapCompletion.Instance.TotalScore)
        {
            m_NeedScoreText.text = m_NeedScore.ToString();
        } else
        {
            m_NeedScoreText.transform.parent.gameObject.SetActive(false);
            GetComponent<MapLevel>().Initialise();
        }  
    }
}
