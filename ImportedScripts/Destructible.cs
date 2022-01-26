using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Класс уничтожаемого объекта. То, что может иметь хитпоинты
/// </summary>
public class Destructible : Entity
{
    #region Properties
    /// <summary>
    /// Объект игнорирует повреждения
    /// </summary>
    [SerializeField]
    protected bool m_Indestructible;
    public bool IsIndestructible => m_Indestructible;

    

    /// <summary>
    /// Стартовое количество хитпоинтов
    /// </summary>
    [SerializeField]
   public int m_HitPoints;

    /// <summary>
    /// Текущие хитпоинты
    /// </summary>
    public int m_CurrentHitPoints;
    public int HitPoints => m_CurrentHitPoints;
    [SerializeField] protected AudioSource m_OnDeathSound;
    [SerializeField] private GameObject[] m_AfterDeathParticles;
    [SerializeField] private GameObject m_ExplosionOnDeath;

    #endregion

    #region  Unity Events
    protected virtual void Start()
    {
        m_CurrentHitPoints = m_HitPoints;
    }

    #endregion

    #region Public API
    /// <summary>
    /// Применение урона к объекту
    /// </summary>
    public void ApplyDamage(int damage)
    {
        if (m_Indestructible) return;

        m_CurrentHitPoints -= damage;

        if (m_CurrentHitPoints <= 0)
        {
            OnDeath();
        }
    }
    #endregion

    /// <summary>
    /// Переопределяемое событие уничтожения объекта, когда хитпоинты меньше или равны нулю
    /// </summary>
    protected virtual void OnDeath()
    {
        //GameObject explosion = (GameObject)Instantiate(m_ExplosionOnDeath, transform.position, m_ExplosionOnDeath.transform.rotation);
        //m_OnDeathSound.Play();
        
        Destroy(gameObject);
        m_EventOnDeath?.Invoke();
    }

    private void CreateParticlesAfterDeath()
    {
        for (int i = 0; i < m_AfterDeathParticles.Length; i++)
        {
        float scaleOffsetX = Random.Range(0.3f, 0.7f);
        float scaleOffsetY = Random.Range(0.3f, 0.7f);
        GameObject debris = Instantiate(m_AfterDeathParticles[i], transform.position, Quaternion.identity);
        debris.transform.localScale = new Vector3(scaleOffsetX, scaleOffsetY, 0);
        float offsetX = Random.Range(-1.0f, 1.0f);
        float offsetY = Random.Range(-1.0f, 1.0f);
        Vector3 direction = new Vector3(offsetX, offsetY, 0);
        debris.transform.Translate(offsetX, offsetY, 0);
        }    
    }

    private static HashSet<Destructible> m_AllDestructibles;
    public static IReadOnlyCollection<Destructible> AllDestructibles => m_AllDestructibles;

    protected virtual void OnEnable()
    {
        if(m_AllDestructibles == null)
            m_AllDestructibles = new HashSet<Destructible>();

            m_AllDestructibles.Add(this);
    }

    protected virtual void OnDestroy()
    {
        m_AllDestructibles.Remove(this);
    }

    protected void UsePreferences(EnemyAsset asset)
    {
        m_HitPoints = asset.hitPoints;
        m_ScoreValue = asset.scoreValue;
    }
    protected void UsePreferencesForDefenders(DefendersAsset asset)
    {
        m_HitPoints = asset.m_HitPoints;
    }

    [SerializeField] private int m_ScoreValue;
    public int ScoreValue => m_ScoreValue;

    public const int TeamIdNeutral = 0;
    [SerializeField] private int m_TeamId;
    public int TeamId => m_TeamId;

    [SerializeField] private UnityEvent m_EventOnDeath;
    public UnityEvent EventOnDeath => m_EventOnDeath;


    
}
