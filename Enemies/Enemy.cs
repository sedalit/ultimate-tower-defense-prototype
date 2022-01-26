using System;
using UnityEngine;

[RequireComponent(typeof(Destructible))]
[RequireComponent(typeof(PatrolController))]
public class Enemy : MonoBehaviour
{
    public enum ArmorType { Base = 0, Magic = 1 }
    private static Func<int, TDProjectile.DamageType, int, int>[] ArmorDamageFunctions =
    {
        (int power, TDProjectile.DamageType type, int armor) =>
        { //ArmorType.Base
            switch (type)
            {
                case TDProjectile.DamageType.Magic: return power;
                default: return Mathf.Max(power - armor, 1);
            }
        },
        (int power, TDProjectile.DamageType type, int armor) =>
        { //ArmorType.Base
            if (TDProjectile.DamageType.Base == type)
            {
                armor = armor / 2;
            }
            return Mathf.Max(power - armor, 1);
        },
    };

    [Header("General Preferences")]
    [SerializeField] private int m_Damage;
    [SerializeField] private int m_Gold;
    [SerializeField] private int m_Armor;
    [SerializeField] private ArmorType m_ArmorType;

    [SerializeField] private AbilityAsset m_AbilityAsset;
    private Enemy m_EnemyReference;
    private Path m_CurrentPath;
    private Destructible m_Destructible;
    private float m_Timer;
    private float m_MaxTimer;
    public event Action OnEnd;
    //-- События Юнити
    private void Awake()
    {
        m_Destructible = GetComponent<Destructible>();
    }
    private void Start()
    {
        if (m_AbilityAsset != null)
        {
            m_EnemyReference = GetComponent<Enemy>();
            m_Timer = m_AbilityAsset.m_CurrentTime;
            m_MaxTimer = m_AbilityAsset.m_MaxTime;
            m_CurrentPath = GetComponent<PatrolController>().m_Path;
        }
    }

    private void Update()
    {
        m_Timer += Time.deltaTime;
        if (m_Timer > m_MaxTimer)
        {
            UseAbility(m_AbilityAsset);
            m_Timer = 0;
        }
    }
    private void OnDestroy()
    {
        OnEnd?.Invoke();
    }
    // -- //

    /// <summary>
    /// Применение настроек врагом в момент его спавна.
    /// </summary>
    /// <param name="asset"></param>
    public void UsePreferences(EnemyAsset asset)
    {
        var sr = transform.Find("VisualModel").GetComponent<SpriteRenderer>();
        sr.color = asset.color;
        sr.transform.localScale = new Vector3(asset.spriteScale.x, asset.spriteScale.y, 1);
        sr.GetComponent<Animator>().runtimeAnimatorController = asset.animation;
        GetComponent<SpaceShip>().UsePreferences(asset);
        GetComponentInChildren<CircleCollider2D>().radius = asset.radius;
        m_Damage = asset.damage;
        m_Gold = asset.gold;
        m_AbilityAsset = asset.enemyAbility;
        m_Armor = asset.armor;
        m_ArmorType = asset.armorType;
    }

    public void DamagePlayer()
    {
        TDPlayer.Instance.ReduceLife(m_Damage);
    }

    public void GivePlayerGold()
    {
        TDPlayer.Instance.ChangeGold(m_Gold);
    }
    public void TakeDamage(int damage, TDProjectile.DamageType damageType)
    {
        m_Destructible.ApplyDamage(ArmorDamageFunctions[(int)m_ArmorType](damage, damageType, m_Armor));
    }
    private void UseAbility(AbilityAsset ability)
    {
        if (ability.m_AbilityMode == AbilityMode.None ) return;
        if (ability.m_AbilityMode == AbilityMode.Summon)
        {
            Summon(m_AbilityAsset.m_SummonSettings);
        }
    }
    private void Summon(EnemyAsset asset)
    {
        var summon = Instantiate(m_AbilityAsset.m_SummonPrefab, transform.position, Quaternion.identity);
        summon.UsePreferences(asset);
        summon.m_Parent = m_EnemyReference;
        summon.m_ParentPos = transform.position;
        summon.GetComponent<PatrolController>().SetPathForSummon(m_CurrentPath, summon);
        EnemyWaveManager.Instance.ActiveEnemyCount++;
    }
}

