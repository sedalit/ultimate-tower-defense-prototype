using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanelController : SingletoneBase<ResultPanelController>
{
    [SerializeField] private Text m_NumKills;
    [SerializeField] private Text m_Score;
    [SerializeField] private Text m_Time;

    [SerializeField] private GameObject m_WinPanel;
    [SerializeField] private GameObject m_LosePanel;

    private bool m_Succes;

    private void Start() {
        m_LosePanel.gameObject.SetActive(false);
        m_WinPanel.gameObject.SetActive(false);
    }

    public void ShowResult(bool succes)
    {
        gameObject.SetActive(true);
        if (succes == true) m_WinPanel.gameObject.SetActive(true);
        if (succes == false) m_LosePanel.gameObject.SetActive(true);
    }

    public void OnButtonNextAction()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
        if(m_Succes)
        {
            LevelSequenceController.Instance.AvanceLevel();

        }
        else
        {
            LevelSequenceController.Instance.RestartLevel();
        }
    }

    public void GoBack()
    {
        LevelSequenceController.Instance.GoToLevelMap();
    }
}
