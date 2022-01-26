using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private TurretMode m_Mode;
    public TurretMode Mode => m_Mode;

    [SerializeField] private TurretProperties m_TurretProperties;
    public TurretProperties TurretProperties => m_TurretProperties;
    private float m_RefireTimer;
    public bool CanFire => m_RefireTimer <= 0;

    private Tower m_Tower;
    private Projectile m_CurrentProjectile;
    public Projectile CurrentProjectile => m_CurrentProjectile;
    private UpgradeAsset m_UpgradeAsset;
    public UpgradeAsset UpgradeAsset => m_UpgradeAsset;


    private void Start() {
        m_Tower = transform.root.GetComponent<Tower>();
        m_UpgradeAsset = m_TurretProperties.CurrentUpgrade;
    }

    private void Update() {
        if (m_RefireTimer >= 0)
        {
        m_RefireTimer -= Time.deltaTime;
        }
        else if (m_TurretProperties.Mode == TurretMode.Auto)
        {
            Fire();
        }
    }
    public Projectile Fire()
    {
        if (m_RefireTimer > 0) return null;
        if (m_TurretProperties == null) return null;
        
        Projectile projectile = Instantiate(m_TurretProperties.ProjectilePrefab).GetComponent<Projectile>();
        projectile.transform.position = transform.position;
        projectile.transform.up = transform.up;
        m_RefireTimer = m_TurretProperties.RateOfFire;  
        projectile.SetParentShooter(m_Tower);
        m_CurrentProjectile = projectile;
        return projectile;
    }
    public void AssignLoadout(TurretProperties props)
    {
        m_RefireTimer = 0;
        m_TurretProperties = props;
    }    
}
