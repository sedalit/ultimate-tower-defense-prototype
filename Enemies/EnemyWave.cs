using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWave : MonoBehaviour
{
    [Serializable]
    public class Squad
    {
        public EnemyAsset asset;
        public int count;
        public string squadName;
    }
    [Serializable]
    public class PathGroup
    {
        public Squad[] squads;
    }
    public static Action<float> OnWavePrepare;
    [SerializeField] private PathGroup[] m_PathGroups;
    [SerializeField] private float m_PrepareTimer = 10f;
    public float PrepareTimer => m_PrepareTimer;
    public float GetRemainingTime() { return m_PrepareTimer - Time.time; }
    //-- События Юнити
    private void Awake()
    {
        enabled = false;
    }
    private void Update()
    {
        if (Time.time >= m_PrepareTimer)
        {
            enabled = false;
            OnWaveReady?.Invoke();
        }
    }
    // -- //
    private event Action OnWaveReady;
    public void Prepare(Action spawnEnemies)
    {
        OnWavePrepare?.Invoke(m_PrepareTimer);
        m_PrepareTimer += Time.time;
        enabled = true;
        OnWaveReady += spawnEnemies;
    }
    [SerializeField] private EnemyWave m_NextWave;

    public EnemyWave PrepareNext(Action spawnEnemies)
    {
        OnWaveReady -= spawnEnemies;
        if (m_NextWave) m_NextWave.Prepare(spawnEnemies);
        return m_NextWave;
    }

    public IEnumerable<(EnemyAsset asset, int count, int pathIndex)> EnumerateSquads()
    {
        for (int i = 0; i < m_PathGroups.Length; i++)
        {
            foreach (var squad in m_PathGroups[i].squads)
            {
                yield return (squad.asset, squad.count, i);
            }
        }
    }

}