using UnityEngine;

public class StandController : MonoBehaviour
{
    private Rigidbody2D m_Rigid;
    private SpriteRenderer m_Sprite;

    private void Start() {
        m_Rigid = transform.root.GetComponent<Rigidbody2D>();
        m_Sprite = GetComponent<SpriteRenderer>();
    }
    private void LateUpdate() {
        transform.up = Vector2.up;
        var xMotion = m_Rigid.velocity.x;
        if(xMotion > 0.01f)
        {
            m_Sprite.flipX = false;
        } else if(xMotion < 0.01f)
        {
            m_Sprite.flipX = true;
        }
    }
}
