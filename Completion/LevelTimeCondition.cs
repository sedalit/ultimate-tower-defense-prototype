using UnityEngine;

public class LevelTimeCondition : MonoBehaviour, ILevelCondition
{
    [SerializeField] private float m_TimeLimit;
    private void Start()
    {
        m_TimeLimit += Time.time;
    }
    public bool IsCompleted => Time.time > m_TimeLimit;

}
