using UnityEngine;

public class TDLevelController : LevelController
{
    private int m_LevelScore = 3;
    private new void Start()
    {
        base.Start();
        Time.timeScale = 1;
        TDPlayer.Instance.OnPlayerDeath += () =>
        {
            StopLevelActivity();
            ResultPanelController.Instance.ShowResult(false);
        };
        m_ReferenceTime += Time.time;
        m_EventOnLevelComplete.AddListener(() =>
            {
                StopLevelActivity();
                if (m_ReferenceTime <= Time.time) m_LevelScore -= 1;
                MapCompletion.SaveEpisodeResult(m_LevelScore);
            });
        void LifeScoreChange(int _)
        {
            m_LevelScore -= 1;
            TDPlayer.Instance.OnLifeUpdate -= LifeScoreChange;
        }
        TDPlayer.Instance.OnLifeUpdate += LifeScoreChange;
    }

    private void StopLevelActivity()
    {
        foreach (var obj in FindObjectsOfType<BuildPosition>())
        {
            obj.enabled = false;
        }
        Time.timeScale = 0;
    }
    
}
