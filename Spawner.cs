using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    public enum SpawnMode
    {
        Start,
        Loop
    }

    protected abstract GameObject GenerateSpawnedEntity();
    [SerializeField] private SpawnMode m_SpawnMode;
    [SerializeField] private int m_NumSpawn;
    [SerializeField] private float m_RespawnTime;
    private float m_Timer;

    //-- События Юнити
    private void Start() {
        if(m_SpawnMode == SpawnMode.Start)
        {
            SpawnEntitys();
        }
        m_Timer = m_RespawnTime;
    }

    private void Update() {
        if (m_Timer > 0)
        {
            m_Timer -= Time.deltaTime;
        }

        if (m_SpawnMode == SpawnMode.Loop && m_Timer < 0)
        {
            SpawnEntitys();
            m_Timer = m_RespawnTime;
        }
    }
    // -- //
    private void SpawnEntitys()
    {
        for (int i = 0; i < m_NumSpawn; i++)
        {
            var e = GenerateSpawnedEntity();
        }
    }
}
