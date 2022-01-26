using UnityEngine;

public class Defender : MonoBehaviour
{
    [SerializeField] private int m_Damage;

    public void UseDefendersPreferences(DefendersAsset asset)
    {
        var sr = transform.Find("VisualModel").GetComponent<SpriteRenderer>();
        sr.color = asset.color;
        sr.transform.localScale = new Vector3(asset.spriteScale.x, asset.spriteScale.y, 1);
        sr.GetComponent<Animator>().runtimeAnimatorController = asset.animation;

        GetComponent<SpaceShip>().UsePreferencesForDefenders(asset);
        GetComponentInChildren<CircleCollider2D>().radius = asset.m_Radius;
    }
}
