using UnityEngine;

public class SceneSwitcher : MonoBehaviour
{
    private string _initialSceneName = "MENU";

    [SerializeField]
    private string _goTo = "GAME";

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
