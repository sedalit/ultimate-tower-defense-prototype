using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        StopLevelActivity();
    }

    public void OnButtonContinue()
    {
        foreach (var obj in FindObjectsOfType<BuildPosition>())
        {
            obj.enabled = true;
        }
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
    public void OnButtonMainMenu()
    {
        foreach (var obj in FindObjectsOfType<BuildPosition>())
        {
            obj.enabled = true;
        }
        Time.timeScale = 1;
        gameObject.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }
    private void StopLevelActivity()
    {
        foreach (var obj in FindObjectsOfType<BuildPosition>())
        {
            obj.enabled = false;
        }
        Time.timeScale = 0;
    }
}
