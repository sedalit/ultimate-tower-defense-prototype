using UnityEngine;

public enum TypeOfUpgrade
{
    Archer, Mage, Player
}
[CreateAssetMenu]
public class UpgradeAsset : ScriptableObject
{
   
    public TypeOfUpgrade m_Type;
    public int[] m_CostByLevel = { 2 };

}
