using UnityEngine;
using UnityEngine.SceneManagement;

public class CampaignEndMenu : MonoBehaviour
{
    public void OnButtonMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void OnButtonExit()
    {
        Application.Quit();
    }
}
