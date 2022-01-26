using UnityEngine;

[CreateAssetMenu]
public class DefendersAsset : ScriptableObject
{
    [Header("Visual Model")]
    public Color color = Color.white;
    public Vector2 spriteScale = new Vector2(3, 3);
    public RuntimeAnimatorController animation;

    [Header("Preferences")]
    public int m_HitPoints;
    public int m_Damage;
    public float m_MoveSpeed;
    public float m_Radius = 0.2f;
}
