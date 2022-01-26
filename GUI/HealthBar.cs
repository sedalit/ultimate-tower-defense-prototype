using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Destructible m_Parent;
    [SerializeField] private Slider m_Bar;
    private void Start()
    {
        m_Bar.maxValue = m_Parent.HitPoints;
    }
    private void Update()
    {
        m_Bar.value = m_Parent.m_CurrentHitPoints;
    }
}
