using System;
using UnityEngine;
public enum AbilityMode
{
    None,
    Summon
}
[CreateAssetMenu]
public class AbilityAsset : ScriptableObject
{
    
    [Header("General")]
    public AbilityMode m_AbilityMode;
    public float m_MaxTime = 5;
    public EnemyAsset m_EnemyAsset;
    public float m_CurrentTime;
    public GameObject m_AbilitySpecialEffect;

    [Header("Summoners")]
    public EnemyAsset m_SummonSettings;
    public EnemySummon m_SummonPrefab;
    public CircleArea m_SummonArea;
    public Path m_SummonPath;
    

    
}
