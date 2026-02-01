using UnityEngine;

public class StartGameAfterSeconds : MonoBehaviour
{
    [SerializeField]
    private float _secondsToWait = 3.0f;
    [SerializeField]
    private string _sceneToLoad = "GAME";

    // Update is called once per frame
    void Update()
    {
        _secondsToWait -= Time.deltaTime;
        if (_secondsToWait <= 0f)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(_sceneToLoad);
        }
    }
}
