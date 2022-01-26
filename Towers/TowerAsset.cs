using UnityEngine;
public enum TowerType
{
    Archer, Mage, Mortare
}
    [CreateAssetMenu]
public class TowerAsset : ScriptableObject
{
        public TowerType m_Type;
        public int m_GoldCost = 10;
        public Sprite m_GUISprite;
        public Sprite m_TowerSprite;
        public TurretProperties m_TurretProperties;
        [SerializeField] private UpgradeAsset m_RequiredUpgrade;
        [SerializeField] private int m_RequiredUpgradeLevel;
        public TowerAsset[] m_UpgradesTo;
        public bool IsAvaliable() => !m_RequiredUpgrade || 
        m_RequiredUpgradeLevel <= Upgrades.GetUpgradeLevel(m_RequiredUpgrade);
}

