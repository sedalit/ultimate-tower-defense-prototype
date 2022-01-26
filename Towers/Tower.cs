using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public TowerAsset CurrentTowerAsset { get; set; }
    [SerializeField] private TypeOfUpgrade m_TypeOfUpgrade;
    [SerializeField] private UpgradeAsset m_CurrentUpgrade;
    [SerializeField] private float m_Radius;
    public float Radius { get { return m_Radius; } private set { m_Radius = value; } }
    [SerializeField] private float m_Lead;
    private Turret[] m_Turrets;
    public Turret[] Turrets => m_Turrets;
    private Rigidbody2D m_Target = null;

    public void Use(TowerAsset asset)
    {
        CurrentTowerAsset = asset;
        GetComponentInChildren<SpriteRenderer>().sprite = asset.m_TowerSprite;
        m_Turrets = GetComponentsInChildren<Turret>();
        foreach (var turret in m_Turrets)
        {
            turret.AssignLoadout(asset.m_TurretProperties);
        }
        var buildPosition = GetComponentInChildren<BuildPosition>();
        buildPosition.SetBuildableTowers(asset.m_UpgradesTo);
    }
    private void Start() {
        m_Turrets = GetComponentsInChildren<Turret>();
        if (m_CurrentUpgrade != null)
        {
            m_CurrentUpgrade = m_Turrets[0].TurretProperties.CurrentUpgrade;
            m_TypeOfUpgrade = m_CurrentUpgrade.m_Type;
        }
        switch (CurrentTowerAsset.m_Type)
        {
            case TowerType.Archer:
                m_Lead = 0.3f;
                break;
            case TowerType.Mage:
                m_Lead = 0.4f;
                break;
            default:
                m_Lead = 0.3f;
                break;
        }
    }

    private void Update() {
            if (m_Target)
            {
                if (Vector3.Distance(m_Target.transform.position, transform.position) < m_Radius)
                {
                    foreach (var turret in m_Turrets)
                    {
                         turret.transform.up = m_Target.transform.position
                            -turret.transform.position + (Vector3)m_Target.velocity * m_Lead;
                         AssignUpgrade(turret.UpgradeAsset);
                         turret.Fire();
                    }
                }
                else
                {
                    m_Target = null;
                }
            }
            else
            {
                var enemy = Physics2D.OverlapCircle(transform.position, m_Radius);
                if (enemy)
                {
                    m_Target = enemy.transform.root.GetComponent<Rigidbody2D>();
                }
            } 
    }
    private void AssignUpgrade(UpgradeAsset upgrade)
    {
        var upgradeLevel = Upgrades.GetUpgradeLevel(upgrade);
        if (m_TypeOfUpgrade == TypeOfUpgrade.Archer)
        {
            if (upgradeLevel >= 1)
            {
                foreach (var turret in m_Turrets)
                {
                    turret.CurrentProjectile.UpgradeVelocity(upgradeLevel);
                }
            }
        }
        if (m_TypeOfUpgrade == TypeOfUpgrade.Mage)
        {
            if (upgradeLevel >= 1)
            {
                foreach (var turret in m_Turrets)
                {
                    turret.CurrentProjectile.UpgradeFreeze(m_Target, upgradeLevel);
                }
            }
        }
    }
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, m_Radius);
    }
}
