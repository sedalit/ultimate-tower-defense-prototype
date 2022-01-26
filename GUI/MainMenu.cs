using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button m_ContinueButton;
    [SerializeField] private GameObject m_Warning;
    private void Start()
    {
        m_ContinueButton.interactable = FileHandler.HasFile(MapCompletion.filename);
    }
    public void ContinueGame()
    {
        SceneManager.LoadScene(1);
    }
    public void NewGame()
    {
        if(m_ContinueButton.interactable)
        {
            m_Warning.gameObject.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void WarningStart()
    {
        FileHandler.Reset(MapCompletion.filename);
        FileHandler.Reset(Upgrades.fileName);
        SceneManager.LoadScene(1);
    }
    public void WarningBack()
    {
        m_Warning.gameObject.SetActive(false);
    }
    
}
