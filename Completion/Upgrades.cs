using System;
using UnityEngine;

public class Upgrades : SingletoneBase<Upgrades>
{
   [Serializable]
   private class UpgradeSave
    {
        public UpgradeAsset asset;
        public int level = 0;
    }
    public const string fileName = "upgrades.dat";
    [SerializeField] private UpgradeSave[] m_Save;
    private new void Awake()
    {
        base.Awake();
        Saver<UpgradeSave[]>.TryLoad(fileName, ref m_Save);
    }
    public static void BuyUpgrade(UpgradeAsset asset)
    {
        foreach (var upgrade in Instance.m_Save)
        {
            if (upgrade.asset == asset)
            {
                Debug.Log("super");
                upgrade.level += 1;
                Saver<UpgradeSave[]>.Save(fileName, Instance.m_Save);
            }
        }
    }
    public static int GetTotalCost()
    {
        int result = 0;
        foreach (var upgrade in Instance.m_Save)
        {
            for (int i = 0; i < upgrade.level; i++)
            {
                result += upgrade.asset.m_CostByLevel[i];
            }
        }
        return result;
    }
        
    public static int GetUpgradeLevel(UpgradeAsset asset)
    {
        foreach (var upgrade in Instance.m_Save)
        {
            if (upgrade.asset == asset)
            {
                return upgrade.level;
            }
        }
        return 0;
    }
}
