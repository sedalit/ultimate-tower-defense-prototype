using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Projectile : Entity
{
    public void SetFromOtherProjectile(Projectile other)
    {
        other.GetData(out m_Velocity, out m_LifeTime, out m_Damage, out m_ImpactEffectPrefab);
    }

    private void GetData(out float m_Velocity, out float m_LifeTime, out int m_Damage, out ImpactEffect m_ImpactEffectPrefab)
    {
        m_Velocity = this.m_Velocity; m_LifeTime = this.m_LifeTime; m_Damage = this.m_Damage;
        m_ImpactEffectPrefab = this.m_ImpactEffectPrefab;
    }

    [SerializeField] protected float m_Velocity;
    public float Velocity { get { return m_Velocity; } set { m_Velocity = value; } }
    [SerializeField] protected float m_LifeTime;
    [SerializeField] protected int m_Damage;
    [SerializeField] private ImpactEffect m_ImpactEffectPrefab;

    private Turret m_ParentTurret;
    protected float m_Timer;
    private UpgradeAsset m_Upgrade;
    private TypeOfUpgrade m_Type;
    private int m_UpgradeLevel;

    private void Start()
    {
        m_ParentTurret = m_Parent.Turrets[0];
        m_Upgrade = m_ParentTurret.UpgradeAsset;
        m_Type = m_Upgrade.m_Type;
        m_UpgradeLevel = Upgrades.GetUpgradeLevel(m_Upgrade);
    }

    private void Update() {
        float stepLength = Time.deltaTime * m_Velocity;
        Vector2 step = transform.up * stepLength;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, stepLength);

        if(hit)
        {
            OnHit(hit);

            OnProjectileLifeEnd(hit.collider, hit.point);
        }
        m_Timer += Time.deltaTime;
        if(m_Timer > m_LifeTime)
        {
            Destroy(gameObject);
        }
        transform.position += new Vector3(step.x, step.y, 0);
    }

    protected virtual void OnHit(RaycastHit2D hit)
    {
        Enemy enemy = hit.collider.transform.root.GetComponent<Enemy>();
            if (m_ImpactEffectPrefab != null)
            {
                ImpactEffect ie = Instantiate(m_ImpactEffectPrefab, enemy.transform.position, Quaternion.identity);
                ie.OnEnemyDamage(enemy);
                if (enemy == null)
                {
                    Destroy(ie.gameObject);
                }
            }
            Destroy(gameObject);
    }

    private void OnProjectileLifeEnd(Collider2D col, Vector2 pos)
    {
        Destroy(gameObject);
    }
   
    private Tower m_Parent;

    public void SetParentShooter(Tower parent)
    {
        m_Parent = parent;
    }
    public void UpgradeVelocity(int level)
    {
        m_Velocity = m_Velocity + (float)level;
        if (m_Velocity + (float)level >= 5) m_Velocity = 5;
    }
    public void UpgradeFreeze(Rigidbody2D target, int level)
    {
         target.GetComponent<SpaceShip>().DebuffSpeed();
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(Projectile))]
    public class ProjectileInspector: Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Создать TD Projectile"))
            {
                var target = this.target as Projectile;
                var tdProj = target.gameObject.AddComponent<TDProjectile>();
                tdProj.SetFromOtherProjectile(target);
            }
        }
    }
#endif
}


