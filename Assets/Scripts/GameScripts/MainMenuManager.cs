using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Animator menuAnimator;

    public void MainMenuToSettings()
    {
        menuAnimator.SetTrigger("SettingsOn");
    }

    public void SettingsToMainMenu()
    {
        menuAnimator.SetTrigger("SettingsOff");
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            QuitGame();
    }
}
