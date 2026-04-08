using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string gameSceneName = "GameScene";
    public GameObject continueButton;

    public void NewGame()
    {
        // Reset save data
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        SceneManager.LoadScene(gameSceneName);
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }

    

    void Start()
    {
        if (!PlayerPrefs.HasKey("Reputation"))
        {
            continueButton.SetActive(false);
        }
    }
}