using UnityEngine;
using UnityEngine.Events;

public class PatrolController : AIController
{
    [SerializeField] private UnityEvent m_OnEndPath;
    public Path m_Path;
    public int pathIndex; 

    public void SetPath(Path newPath)
    {
        m_Path = newPath;
        pathIndex = 0;
        SetPatrolBehaivour(m_Path[pathIndex]);
    }
    public void SetPathForSummon(Path summonPath, EnemySummon summon)
    {
        m_Path = summonPath;
        pathIndex = summon.m_Parent.GetComponent<PatrolController>().pathIndex;
        SetPatrolBehaivour(m_Path[pathIndex]);
    }
    protected override void GetNewMovePoint()
    {
        if (m_Path == null) return;
        if(m_Path.PointsLength > ++pathIndex)
        {
            SetPatrolBehaivour(m_Path[pathIndex]);
        } else
        {
            m_OnEndPath.Invoke();
            Destroy(gameObject);
        }
    }
    
}
