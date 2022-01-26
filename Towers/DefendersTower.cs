using UnityEngine;

public class DefendersTower : Tower
{
    [SerializeField] private Defender m_DefenderPrefab;
    [SerializeField] private int m_MaxUnitsNum = 3;
    [SerializeField] private CircleArea m_Area;
    private int m_CurrentUnitsNum;
    private AIPointPatrol[] m_StandPoints;
    private AIPointPatrol m_NearestPoint;
    private void Start()
    {
        m_StandPoints = FindObjectsOfType<AIPointPatrol>();
        m_NearestPoint = FindNearestPoint();
    }
    private void Update()
    {
        if (m_CurrentUnitsNum < m_MaxUnitsNum)
        {
            EscapeDefenders(m_MaxUnitsNum - m_CurrentUnitsNum);
        }
    }
    private void EscapeDefenders(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var defender = Instantiate(m_DefenderPrefab);
            defender.transform.position = m_Area.GetRandomInsideZone();
            defender.GetComponent<PatrolController>().SetPatrolBehaivour(FindNearestPoint());
            m_CurrentUnitsNum++;
        }
    }
    private AIPointPatrol FindNearestPoint()
    {
        float distance = Radius;
        foreach (var point in m_StandPoints)
        {
            Vector3 diff = point.transform.position - transform.position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                m_NearestPoint = point;
            }
        }
        return m_NearestPoint;
    }
}
