using System;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    public static EnemyWaveManager Instance;
    public static event Action<Enemy> OnEnemySpawn;
    [SerializeField] private Path[] m_Paths;
    [SerializeField] private EnemyWave m_FirstWave;
    public EnemyWave FisrtWave => m_FirstWave;
    [SerializeField] private EnemyWave m_CurrentWave;
    public EnemyWave CurrentWave => m_CurrentWave;
    [SerializeField] private Enemy m_EnemyPrefab;
    [SerializeField] private int m_ActiveEnemyCount;
    public int ActiveEnemyCount { get; set; }
    public event Action OnAllWavesEnd;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        m_CurrentWave.Prepare(SpawnEnemies);
    }
    public void ForceNextWave(int bonus)
    {
        if (m_CurrentWave)
        {
            if (m_CurrentWave != m_FirstWave)
            {
                TDPlayer.Instance.ChangeGold(bonus);
            }
            SpawnEnemies();
        }
        else
        {
            if (m_ActiveEnemyCount == 0) OnAllWavesEnd?.Invoke();
        }
    }
    private void SpawnEnemies()
    {
        Sound.EnemySpawn.Play();
        foreach ((EnemyAsset asset, int count, int pathIndex) in m_CurrentWave.EnumerateSquads())
        {
            if (pathIndex < m_Paths.Length)
            {
                for (int i = 0; i < count; i++)
                {
                    var e = Instantiate(m_EnemyPrefab, 
                        m_Paths[pathIndex].m_StartPointArea.GetRandomInsideZone(), Quaternion.identity);
                    e.OnEnd += RecordEnemyDeath;
                    e.UsePreferences(asset);
                    e.GetComponent<PatrolController>().SetPath(m_Paths[pathIndex]);
                    m_ActiveEnemyCount += 1;
                    OnEnemySpawn?.Invoke(e);
                }
            }
        }

        m_CurrentWave = m_CurrentWave.PrepareNext(SpawnEnemies);
    }
    private void RecordEnemyDeath()
    {
        if (--m_ActiveEnemyCount == 0)
        {
            if (m_CurrentWave)
            {
                ForceNextWave(0);
            }
            else
            {
                OnAllWavesEnd?.Invoke();
            }
        }
    }
}
