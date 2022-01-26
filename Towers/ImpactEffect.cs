using UnityEngine;

public class ImpactEffect : MonoBehaviour
{
    [SerializeField] private float m_LifeTime;
    [SerializeField] private TurretMode m_Mode;
    [SerializeField] private float m_Radius;
    [SerializeField] private ParticleSystem m_OnDamageEffect;
    private float m_Timer;

    private void Start()
    {
        m_Timer = m_OnDamageEffect.duration;
    }
    private void Update() {
        if (m_Timer > m_LifeTime)
        {
            m_LifeTime += Time.deltaTime;
            if (m_LifeTime >= m_Timer)
            {
                Destroy(gameObject);
                m_LifeTime = 0;
            }
        }
    }
    public void OnEnemyDamage(Enemy enemy)
    {
        if (m_OnDamageEffect != null)
        {
            var effect = Instantiate(m_OnDamageEffect);
            effect.transform.position = enemy.transform.position;
        }
        if (m_Mode == TurretMode.Primary) return;
        if (m_Mode == TurretMode.Auto) return;
        if (m_Mode == TurretMode.Net)
        {
            enemy.GetComponentInParent<SpaceShip>().Cathced();
        }
        if (m_Mode == TurretMode.Artillery)
        {
            Collider2D[] enemys = Physics2D.OverlapCircleAll(transform.position, m_Radius);
            foreach (var e in enemys)
            {
                e.GetComponent<Destructible>().ApplyDamage(2);
            }
        }
    }

}
