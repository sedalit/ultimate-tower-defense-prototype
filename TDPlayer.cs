using UnityEngine;
using System;
public class TDPlayer : Player
{
    [Serializable]
    private class HealthUpgrade
    {
        public UpgradeAsset upgrade;
        public int level;

        public void Use()
        {
            Player.Instance.NumLives += level * 5;
        }
    }
    [Serializable]
    private class ManaUpgrade
    {
        public UpgradeAsset upgrade;
        public int level;
    }
    [Serializable]
    private class CurrentUpgrades
    {
        public HealthUpgrade healthUpgrade;
        public ManaUpgrade manaUpgrade;
    }
    public static new TDPlayer Instance { get {return Player.Instance as TDPlayer;} }
    private event Action<int> OnGoldUpdate;
    public void GoldUpdateSubscribe(Action<int> act)
    {
        OnGoldUpdate += act;
        act(Instance.m_Gold);
    }
    public event Action<int> OnLifeUpdate;
    public void LifeUpdateSubscribe(Action<int> act)
    {
        OnLifeUpdate += act;
        act(Instance.NumLives);
    }
    [Header("Main")]
    [SerializeField] private HealthUpgrade m_HealthUpgrade;
    [SerializeField] private ManaUpgrade m_ManaUpgrade;
    [SerializeField] private int m_Gold = 0;
    [SerializeField] private int m_Mana = 0;
    [SerializeField] private int m_RestoreManaTime = 2;
    public int Mana => m_Mana;
    private float m_Timer;
    
    private new void Start()
    {
        m_HealthUpgrade.level = Upgrades.GetUpgradeLevel(m_HealthUpgrade.upgrade);
        m_ManaUpgrade.level = Upgrades.GetUpgradeLevel(m_ManaUpgrade.upgrade);
        m_HealthUpgrade.Use();
        OnLifeUpdate(NumLives);
    }
    private void Update()
    {
        m_Timer += Time.deltaTime;
        if(m_Timer >= m_RestoreManaTime)
        {
            RestoreMana(m_ManaUpgrade.level);
            m_Timer = 0;
        }
        if (m_Mana >= 100) m_Mana = 100;
    }
    public void ChangeGold(int change)
    {
        m_Gold += change;
        OnGoldUpdate(m_Gold);
    }

    public void ReduceLife(int change)
    {
        TakeDamage(change);
        OnLifeUpdate(NumLives);
    }
    public void ChangeMana(int change)
    {
        m_Mana -= change;
    }
    [SerializeField] private Tower m_TowerPrefab;
    public void TryBuild(TowerAsset towerAsset, Transform buildPosition)
    {
        if(m_Gold >= towerAsset.m_GoldCost)
        {
            Sound.TowerBuilt.Play();
            ChangeGold(-towerAsset.m_GoldCost);
            var tower = Instantiate(m_TowerPrefab, buildPosition.position, Quaternion.identity);
            tower.Use(towerAsset);
            Destroy(buildPosition.gameObject);
        }
    }
    private void RestoreMana(int level)
    {
        if (level == 0) level = 1;
        m_Mana += level * 1;
    }
}
