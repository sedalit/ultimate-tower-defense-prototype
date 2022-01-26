using UnityEngine;

public class EnemySpawner : Spawner
{
    [SerializeField] private Enemy m_EnemyPrefab;
    [SerializeField] private EnemyAsset[] m_EnemySettings;
    [SerializeField] private CircleArea m_Area;
    [SerializeField] public Path m_Path;
    protected override GameObject GenerateSpawnedEntity()
    {
        var e = Instantiate(m_EnemyPrefab);
        e.transform.position = m_Area.GetRandomInsideZone();
        e.UsePreferences(m_EnemySettings[Random.Range(0, m_EnemySettings.Length)]);
        e.GetComponent<PatrolController>().SetPath(m_Path);
        return e.gameObject;
    }

    
}
