using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public int _requiredValueForBoost = 5;
    [SerializeField]
    private float _speedBoostDuration = 5.0f;
    private float _speedBoostElapsedTime = 0f;
    [SerializeField]
    private int _invincibilityDuration = 5;
    private float _invincibilityElapsedTime = 0f;
    bool _invincibilityFinished = true;

    public int InvincibilityDuration { get { return _invincibilityDuration; } }

    [SerializeField]
    private PlayerColor _playerColor;
    [SerializeField]
    private GameObject _pauseMenu;

    [SerializeField]
    private List<GameObject> _happyMeterSprites = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _sadMeterSprites = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _calmMeterSprites = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _angryMeterSprites = new List<GameObject>();

    public int _happyValue;
    public int _sadValue;
    public int _calmValue;
    public int _angryValue;

    [SerializeField]
    private GameObject _SpeedIcon;
    [SerializeField]
    private GameObject _ShieldIcon;
    [SerializeField]
    private GameObject _PiercingIcon;
    [SerializeField]
    private GameObject _MultishotIcon;

    private int _score = 0;
    public int Score {  get { return _score; } set { _score = value; } }

    [SerializeField]
    private TextMeshProUGUI _scoreText;

    public bool _speedBoostEnabled = false;
    public bool _invincibilityEnabled = false;
    public int _nextArrowsMultiShot = 0;
    public int _nextArrowsPiercing = 0;

    private bool _speedBoostActiveOnce = false;
    private bool _invincibilityActiveOnce = false;
    private bool _nextArrowsMultiShotActiveOnce = false;
    private bool _nextArrowsPiercingActiveOnce = false;

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

    private void Update()
    {
        if (_pauseMenu.activeSelf)
        {
            GameIsActive = false;
        }
        else
        {
            GameIsActive = true;
        }
        if (_happyValue >= _requiredValueForBoost)
        {
            _sadValue = 0; _calmValue = 0; _angryValue = 0;
            if (!_nextArrowsPiercingActiveOnce)
            {
                _nextArrowsPiercing = 1;
            }
            _nextArrowsPiercingActiveOnce = true;
            if (_nextArrowsPiercing <= 0)
            {
                _happyValue = 0; _sadValue = 0; _calmValue = 0; _angryValue = 0;
                _nextArrowsPiercingActiveOnce = false;
            }
        }
        if (_sadValue >= _requiredValueForBoost)
        {
            _happyValue = 0; _calmValue = 0; _angryValue = 0;
            if (!_nextArrowsMultiShotActiveOnce)
            {
                _nextArrowsMultiShot = 1;
            }
            _nextArrowsMultiShotActiveOnce = true;
            if (_nextArrowsMultiShot <= 0)
            {
                _happyValue = 0; _sadValue = 0; _calmValue = 0; _angryValue = 0;
                _nextArrowsMultiShotActiveOnce = false;
            }
        }
        if (_calmValue >= _requiredValueForBoost)
        {
            _happyValue = 0; _sadValue = 0; _angryValue = 0;
            if (!_invincibilityActiveOnce)
            {
                _invincibilityEnabled = true;
                _invincibilityFinished = false;
            }
            _invincibilityActiveOnce = true;
            if (_invincibilityFinished)
            {
                _happyValue = 0; _sadValue = 0; _calmValue = 0; _angryValue = 0;
                _invincibilityActiveOnce = false;
            }
        }
        if (_angryValue >= _requiredValueForBoost)
        {
            _happyValue = 0; _sadValue = 0; _calmValue = 0;
            if (!_speedBoostActiveOnce)
            {
                _speedBoostEnabled = true;
            }
            _speedBoostActiveOnce = true;
            if (!_speedBoostEnabled)
            {
                _happyValue = 0; _sadValue = 0; _calmValue = 0; _angryValue = 0;
                _speedBoostActiveOnce = false;
            }
        }

        if (_speedBoostEnabled)
        {
            _speedBoostElapsedTime += Time.deltaTime;
            if (_speedBoostElapsedTime > _speedBoostDuration)
            {
                _speedBoostElapsedTime = 0.0f;
                _speedBoostEnabled = false;
            }
        }
        if (_invincibilityEnabled)
        {
            _invincibilityElapsedTime = 0.0f;
        }
        _invincibilityElapsedTime += Time.deltaTime;
        if (_invincibilityElapsedTime > _invincibilityDuration)
        {
            _invincibilityFinished = true;
        }

        VisualiseMeters();
        SetPlayerColor();
        VisualisePowerUps();
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        _scoreText.text = _score.ToString();
    }

    private void VisualisePowerUps()
    {
        _SpeedIcon.SetActive(_speedBoostEnabled);
        _ShieldIcon.SetActive(_invincibilityEnabled);
        _PiercingIcon.SetActive(_nextArrowsPiercing > 0);
        _MultishotIcon.SetActive(_nextArrowsMultiShot > 0);
    }

    private void SetPlayerColor()
    {
        //if any meter is full, enable rainbow effect
        if (_happyValue >= _requiredValueForBoost || _sadValue >= _requiredValueForBoost || _calmValue >= _requiredValueForBoost || _angryValue >= _requiredValueForBoost)
        {
            _playerColor.EnableRainbow();
        }
        //if no meter is full, disable rainbow effect
        else
        {
            _playerColor.DisableRainbow();
        }
    }

    private void VisualiseMeters()
    {
        //visualise meter values and their sprites
        for (int i = 0; i < _happyMeterSprites.Count; i++)
        {
            _happyMeterSprites[i].SetActive(i < _happyValue);
        }
        for (int i = 0; i < _sadMeterSprites.Count; i++)
        {
            _sadMeterSprites[i].SetActive(i < _sadValue);
        }
        for (int i = 0; i < _calmMeterSprites.Count; i++)
        {
            _calmMeterSprites[i].SetActive(i < _calmValue);
        }
        for (int i = 0; i < _angryMeterSprites.Count; i++)
        {
            _angryMeterSprites[i].SetActive(i < _angryValue);
        }
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
        //reload the CUTSCENE scene
        //set parent of main camera to null
        Camera.main.transform.parent = null;
        //delete player from scene
        _playerColor.gameObject.SetActive(false);
        //save score in PlayerPrefs if it's higher than previous high score
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (_score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", _score);
        }
        //wait 0.5 seconds, then go to CUTSCENE scene
        Invoke("MoveToCutScene", 0.5f);
    }

    private void MoveToCutScene()
    {
               UnityEngine.SceneManagement.SceneManager.LoadScene("Cutscene");
    }

    private void SpeedBoost()
    {
        _speedBoostEnabled = true;
    }
    private void Invincibility()
    {
        _invincibilityEnabled = true;
    }
}
