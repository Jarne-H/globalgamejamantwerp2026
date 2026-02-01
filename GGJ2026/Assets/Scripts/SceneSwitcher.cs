using UnityEngine;

public class SceneSwitcher : MonoBehaviour
{
    private string _initialSceneName = "MainMenu";

    [SerializeField]
    private string _goTo = "GAME";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(_initialSceneName);
    }

    public void GoToScene()
    {
        SwitchScene(_goTo);
    }

    public void SwitchScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
