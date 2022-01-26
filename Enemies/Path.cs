using UnityEngine;

public class Path : MonoBehaviour
{
    [SerializeField] private AIPointPatrol[] m_Points;
    public CircleArea m_StartPointArea;
    public int PointsLength { get { return m_Points.Length; } }
    public AIPointPatrol this[int i] { get => m_Points[i]; }
    [SerializeField] private float m_Radius;
    private static readonly Color GizmoColor = new Color(1, 0, 0, 0.3f);

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = GizmoColor;
        foreach (var point in m_Points)
        {
            Gizmos.DrawSphere(point.transform.position, m_Radius);
        }
    }
}
