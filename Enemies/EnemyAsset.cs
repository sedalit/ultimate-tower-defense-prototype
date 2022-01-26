using UnityEngine;

[CreateAssetMenu]
public sealed class EnemyAsset : ScriptableObject
{
    [Header("Visual Model")]
    public Color color = Color.white;
    public Vector2 spriteScale = new Vector2(3, 3);
    public RuntimeAnimatorController animation;

    [Header("Preferences")]
    public string name;
    public float moveSpeed = 1;
    public int hitPoints = 1;
    public int armor = 0;
    public Enemy.ArmorType armorType;
    public int scoreValue = 1;
    public float radius = 0.2f;
    public int damage = 1;
    public int gold = 1;
    public AbilityAsset enemyAbility;
    
} 

