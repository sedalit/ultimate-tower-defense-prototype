using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class SpaceShip : Destructible
{
    #region Properties

    /// <summary>
    /// Масса для автоматической установки у ригида
    /// </summary>
    [Header("Space ship properties")]
    [SerializeField] private float m_Mass;

    /// <summary>
    /// Толкающая вперед сила
    /// </summary>
    [SerializeField] private float m_Thrust;

    /// <summary>
    /// Вращающая сила
    /// </summary>
    [SerializeField] private float m_Mobility;

    /// <summary>
    /// Максимальная линейная скорость
    /// </summary>
    [SerializeField] private float m_MaxLinearVelocity;
    private float m_MaxVelocityBackup;
    public void HalfMaxLinearVelocity() 
    { 
        m_MaxVelocityBackup = m_MaxLinearVelocity; 
        m_MaxLinearVelocity /= 2;
    }
    public void RestoreMaxLinearVelocity() { m_MaxLinearVelocity = m_MaxVelocityBackup; }

    /// <summary>
    /// Максимальная вращательная скорость. В градусах\сек
    /// </summary>
    [SerializeField] private float m_MaxAngularVelocity;
    public float MaxAngularVelocity => m_MaxAngularVelocity;

    [SerializeField] private Sprite m_PreviewImage;
    public Sprite PreviewImage => m_PreviewImage;

    /// <summary>
    /// Сохраненная ссылка на ригид
    /// </summary>
    [SerializeField] private Rigidbody2D m_Rigid;
    private bool m_IsInStun;
    private int m_MaxTime = 4;
    private float m_Timer;
    private EnemyAsset m_Asset;

    #endregion

    #region  Public API
    /// <summary>
    /// Управление линейной тягой. -1.0 до 1.0
    /// </summary>
    public float ThrustControl { get; set; }

    /// <summary>
    /// Управление вращательной тягой. -1.0 до 1.0
    /// </summary>
    public float TorqueControl { get; set; }

    #endregion

    #region Unity Events

    protected override void Start()
    {
        base.Start();
        m_Rigid = GetComponent<Rigidbody2D>();
        m_Rigid.mass = m_Mass;
        m_Rigid.inertia = 1;
    }

    private void FixedUpdate()
    {
        UpdateRigidBody();
        if(m_IsInStun)
        {
            m_Timer += Time.deltaTime;
            if(m_Timer >= m_MaxTime)
            {
                BuffSpeed(m_Asset);
                m_Timer = 0;
            }
        }
    }

    #endregion

    
    /// <summary>
    /// Метод добавления сил кораблю для движения
    /// </summary>
    private void UpdateRigidBody()
    {
        m_Rigid.AddForce(ThrustControl * m_Thrust * transform.up * Time.fixedDeltaTime, ForceMode2D.Force);
        m_Rigid.AddForce(-m_Rigid.velocity * (m_Thrust / m_MaxLinearVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);
        m_Rigid.AddTorque(TorqueControl * m_Mobility * Time.fixedDeltaTime, ForceMode2D.Force);
        m_Rigid.AddTorque(-m_Rigid.angularVelocity * (m_Mobility / m_MaxAngularVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);
    }

    protected override void OnDeath()
    {
        base.OnDeath();
    }

    public void UsePreferences(EnemyAsset asset)
    {
        m_MaxLinearVelocity = asset.moveSpeed;
        m_Asset = asset;
        base.UsePreferences(asset);
    }
    public void UsePreferencesForDefenders(DefendersAsset asset)
    {
        m_MaxLinearVelocity = asset.m_MoveSpeed;
        base.UsePreferencesForDefenders(asset);
    }

    public void Cathced()
    {
        m_Rigid.constraints = RigidbodyConstraints2D.FreezePosition;
    }
    public void UnCathced()
    {
        m_Rigid.constraints = RigidbodyConstraints2D.None;
    }

    public void DebuffSpeed()
    {
        m_MaxLinearVelocity = 0.5f;
        m_IsInStun = true;
    }
    public void BuffSpeed(EnemyAsset asset)
    {
        m_MaxLinearVelocity = asset.moveSpeed;
        m_IsInStun = false;
    }
}
