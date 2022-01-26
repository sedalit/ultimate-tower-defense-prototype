using UnityEngine;
using UnityEngine.EventSystems;

public class OnClickEffect : MonoBehaviour
{
    private void OnMouseDown()
    {
        Sound.Click.Play();
        Destroy(gameObject);
    }
}
