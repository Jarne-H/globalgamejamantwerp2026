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

    private int _happySadValue = 0;
    public int HappySadValue { get { return _happySadValue; } set { _happySadValue = value; } }

    private int _calmAngryValue = 0;
    public int CalmAngryValue { get { return _calmAngryValue; } set { _calmAngryValue = value; } }

    private int _score = 0;
    public int Score {  get { return _score; } set { _score = value; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
    }
}
