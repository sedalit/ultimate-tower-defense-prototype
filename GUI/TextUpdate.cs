using UnityEngine;
using UnityEngine.UI;

public class TextUpdate : MonoBehaviour
{
    public enum UpdateSource { Gold, Life }
    public UpdateSource m_Source = UpdateSource.Gold;
    private Text m_Text;

    void Start()
    {
        m_Text = GetComponent<Text>();
        switch (m_Source)
        {
            case UpdateSource.Gold:
                TDPlayer.Instance.GoldUpdateSubscribe(UpdateText);
                break;

            case UpdateSource.Life:
                TDPlayer.Instance.LifeUpdateSubscribe(UpdateText);
                break;

        }
    }

    void UpdateText(int i)
    {
        m_Text.text = i.ToString();
    }
}