using System;
using UnityEngine;
using UnityEngine.UI;
public class MapLevel : MonoBehaviour
{
    [SerializeField] private Episode m_Episode;
    [SerializeField] private RectTransform m_ResultPanel;
    [SerializeField] private Image[] m_ResultImages;

    public bool isCompleted { get { return gameObject.activeSelf &&
                m_ResultPanel.gameObject.activeSelf; } }

    public int Initialise()
    {
        var score = MapCompletion.Instance.GetEpisodeScore(m_Episode);
        m_ResultPanel.gameObject.SetActive(score > 0);
        for (int i = 0; i < score; i++)
        {
            m_ResultImages[i].color = Color.white;
        }
        return score;
    }
    public void LoadLevel()
    {
       LevelSequenceController.Instance.StartEpisode(m_Episode);
    }        
}
