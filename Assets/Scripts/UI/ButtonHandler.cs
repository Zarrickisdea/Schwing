using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(SceneNames.MainScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(SceneNames.StartScene);
    }
}
