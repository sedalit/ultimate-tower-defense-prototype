using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSequenceController : SingletoneBase<LevelSequenceController>
{
    public static string MainMenuSceneNickname = "LevelMap";
    public Episode CurrentEpisode { get; private set; }
    public int CurrentLevel { get; private set; }
    public static SpaceShip PlayerShip { get; set; }
    public bool LastLevelResult {get; private set; }
    public LevelStatistics levelStatistics { get; private set; }
    [SerializeField] private GameObject m_PausePanel;
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) m_PausePanel.SetActive(true);
    }
    public void StartEpisode(Episode e)
    {
        CurrentEpisode = e;
        CurrentLevel = 0;

        levelStatistics = new LevelStatistics();
        levelStatistics.Reset();

        SceneManager.LoadScene(e.Levels[CurrentLevel]);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(CurrentEpisode.Levels[CurrentLevel]);
    }
    
    public void FinishCurrentLevel(bool succes)
    {
        LastLevelResult = succes;
        ResultPanelController.Instance.ShowResult(succes);

    }

    public void AvanceLevel()
    {
        levelStatistics.Reset();
        CurrentLevel++;
        if(CurrentEpisode.Levels.Length <= CurrentLevel)
        {
            SceneManager.LoadScene(MainMenuSceneNickname);
        }
        else
        {
            SceneManager.LoadScene(CurrentEpisode.Levels[CurrentLevel]);
        }
    }
    public void GoToLevelMap()
    {
        SceneManager.LoadScene(MainMenuSceneNickname);
    }

    private void CalculateStatistic()
    {
        if((int)LevelController.Instance.LevelTime <= 180)
            {
                levelStatistics.score = Player.Instance.Score * 2;

            }    
            else
            {
                levelStatistics.score = Player.Instance.Score; 
            }

        levelStatistics.numKills = Player.Instance.NumKills;
        levelStatistics.time = (int)LevelController.Instance.LevelTime;
    }
}
