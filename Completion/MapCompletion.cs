using System;
using UnityEngine;

public class MapCompletion : SingletoneBase<MapCompletion>
{
    public const string filename = "completion.dat";
    [Serializable]
    private class EpisodeScore
    {
        public Episode episode;
        public int score;
    }

    [SerializeField] private EpisodeScore[] m_CompetionData;
    private int m_TotalScore;
    public int TotalScore { get { return m_TotalScore; } }

    private new void Awake()
    {
        base.Awake();
        Saver<EpisodeScore[]>.TryLoad(filename, ref m_CompetionData);
        foreach (var episodeScore in m_CompetionData)
        {
            m_TotalScore += episodeScore.score;
        }
    }
    public static void SaveEpisodeResult(int levelScore)
    {
        Instance.SaveResult(LevelSequenceController.Instance.CurrentEpisode, levelScore);
    }    
    public int GetEpisodeScore(Episode m_Episode)
    {
        foreach (var data in m_CompetionData)
        {
            if (data.episode == m_Episode) return data.score;
        }
        return 0;
    }
    private void SaveResult(Episode currentEpisode, int levelScore)
    {
        foreach (var item in m_CompetionData)
        {
            if (item.episode == currentEpisode)
            {
                if (levelScore > item.score)
                {
                    m_TotalScore += levelScore - item.score;
                    item.score = levelScore;
                    Saver<EpisodeScore[]>.Save(filename, m_CompetionData);
                }    
            }
        }
    }
}
