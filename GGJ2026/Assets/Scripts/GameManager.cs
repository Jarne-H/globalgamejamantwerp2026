using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    //only 1 instance of GameManager
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private PlayerInput _playerInput;

    public bool GameIsActive = true;

    [SerializeField]
    private CanvasGroup _fade;
    [SerializeField]
    private float _fadeDuration = 0.2f;
    public float FadeDuration => _fadeDuration;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //if there is already an instance of GameManager, destroy this one
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        //fade in
        float timer = 0.0f;
        while (timer < _fadeDuration)
        {
            timer += Time.deltaTime;
            _fade.alpha = Mathf.Lerp(1.0f, 0.0f, timer / _fadeDuration);
        }
        _fade.alpha = 0.0f;
        //enable player input
        GameIsActive = true;
        _playerInput.enabled = true;
    }

    public void TriggerResetGame()
    {
        ResetGame();
    }

    private void ResetGame()
    {
        //fade to black
        float timer = 0.0f;

        GameIsActive = false;

        while (timer < _fadeDuration)
        {
            timer += Time.deltaTime;
            _fade.alpha = Mathf.Lerp(0.0f, 1.0f, timer / _fadeDuration);
        }
        _fade.alpha = 1.0f;
        //reload the current scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        //fade back in
        timer = 0.0f;
        while (timer < _fadeDuration)
        {
            timer += Time.deltaTime;
            _fade.alpha = Mathf.Lerp(1.0f, 0.0f, timer / _fadeDuration);
        }
        _fade.alpha = 0.0f;
        //enable player input
        GameIsActive = true;
    }
}
