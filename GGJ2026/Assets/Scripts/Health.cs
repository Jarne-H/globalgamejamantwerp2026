using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int _maxHealth = 1;
    private int _currentHealth;

    [SerializeField]
    private int _maxLives = 1;
    private int _currentLives;

    [SerializeField]
    private float _healthRegenRate = 0.0f;//amount of time between regenerating 1 health point
    private float _healthRegenTimer = 0.0f;

    [Header("Invincibility settings")]
    [SerializeField]
    private bool _isInvincible = false;

    [SerializeField]
    private float _invincibilityDurationAtStart = 3.0f;
    private float _invincibilityCurrentDuration = 0.0f;
    [SerializeField]
    private float _invincibilityTimeAfterDamage = 2.8f;
    [SerializeField]
    private GameObject _invincibilityShield;

    //die event
    public delegate void DieEvent();
    public event DieEvent OnDie;


    [Header("Player death settings")]
    [SerializeField]
    private bool _isPlayer = false;
    [SerializeField]
    private string _deathSceneName = "GAMEOVER";
    [Header("Enemy death settings")]
    [SerializeField]
    private bool _isEnemy = false;
    [SerializeField]
    private List<GameObject> _enemyPrefabs = new List<GameObject>();

    [Header("GameJuice")]
    [SerializeField]
    private GameManager _gameManager;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentHealth = _maxHealth;
        _currentLives = _maxLives;
        _invincibilityCurrentDuration = _invincibilityDurationAtStart;

        if (_gameManager == null)
        {
            _gameManager = FindAnyObjectByType<GameManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (_gameManager == null)
        {
            _gameManager = FindAnyObjectByType<GameManager>();
        }

        if (_gameManager.GameIsActive)
        {
            if (_isInvincible)
            {
                if (_invincibilityShield != null)
                {
                    _invincibilityShield.SetActive(true);
                }
                _invincibilityCurrentDuration -= Time.deltaTime;
                if (_invincibilityCurrentDuration <= 0.0f)
                {
                    _isInvincible = false;
                    _invincibilityCurrentDuration = _invincibilityDurationAtStart;
                }
            }
            else if (_invincibilityShield != null && _invincibilityShield.activeSelf && !_gameManager._invincibilityEnabled)
            {
                _invincibilityShield.SetActive(false);
            }

            if (_healthRegenRate > 0 && _currentHealth < _maxHealth)
            {
                _healthRegenTimer += Time.deltaTime;
                if (_healthRegenTimer >= _healthRegenRate)
                {
                    AdjustHealth(1);
                    _healthRegenTimer = 0.0f;
                }
            }

            if (_currentHealth <= 0)
            {
                _currentLives--;
                if (_currentLives > 0)
                {
                    _currentHealth = _maxHealth;

                    if (_isPlayer)
                    {
                        HandlePlayerRespawn();
                    }
                    else if (_isEnemy)
                    {
                        HandleEnemyRespawn();
                    }
                }
                else
                {
                    if (_isPlayer)
                    {
                        HandlePlayerDeath();
                    }
                    else if (_isEnemy)
                    {
                        HandleEnemyDeath();
                    }
                }
            }



        }
    }

    public void AdjustHealth(int amount)
    {
        if (amount < 0 && (_isInvincible || (_gameManager != null && _gameManager._invincibilityEnabled)))
        {
            _invincibilityShield.SetActive(true);
            return;
        }
        _currentHealth += amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

        if (!_isPlayer)
        {
            HandleEmotionMeter();
            IncrementScore(1);
        }
        if (_currentHealth <= 0)
        {
            OnDie?.Invoke();
        }
    }

    public void SetInvinsibility(int Time)
    {
        _isInvincible = true;
        _invincibilityCurrentDuration = Time;
    }

    private void HandlePlayerDeath()
    {
        if (_gameManager == null)
        {
            _gameManager = FindAnyObjectByType<GameManager>();
        }

        //start coroutine to reset the game and wait for it to finish
        _gameManager.TriggerResetGame();
    }

    private void HandleEnemyDeath()
    {
        if (_currentLives > 0)
        {
            _currentHealth = _maxHealth;
            HandleEnemyRespawn();
        }
        else
        {
            Debug.Log("EnemyDied");
            Destroy(gameObject);
        }
    }

    private void HandlePlayerRespawn()
    {
        if (_isPlayer)
        {
            _isInvincible = true;
            _invincibilityCurrentDuration = _invincibilityTimeAfterDamage;
        }
        // Implement player respawn logic here (e.g., reset position, play animation, etc.)
        Debug.Log("Player respawned. Lives left: " + _currentLives);
    }

    private void HandleEnemyRespawn()
    {
        //get random enemy type index
        int randomEnemyTypeIndex = Random.Range(0, _enemyPrefabs.Count);
        //spawn new enemy of randomEnemyTypeIndex
        GameObject newEnemyPrefab = _enemyPrefabs[randomEnemyTypeIndex];
        GameObject newEnemy = Instantiate(newEnemyPrefab, transform.position, Quaternion.identity);
        newEnemy.GetComponent<EnemyMovement>().PlayerTransform = GetComponent<EnemyMovement>().PlayerTransform;
        Destroy(gameObject);
    }

    public void AddLife()
    {
        _currentLives++;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_gameManager.GameIsActive)
        {
            //Debug.Log("Collision detected with " + collision.gameObject.name);
            if (_isPlayer && collision.gameObject.CompareTag("Enemy"))
            {
                AdjustHealth(-1);
            }
        }
    }

    private void IncrementScore(int increment)
    {
        _gameManager.Score += increment;
    }
    private void HandleEmotionMeter()
    {
        EnemyMovementSad sadEnemy = GetComponent<EnemyMovementSad>();
        EnemyMovementHappy happyEnemy = GetComponent<EnemyMovementHappy>();
        EnemyMovementCalm calmEnemy = GetComponent<EnemyMovementCalm>();
        EnemyMovementAngry angryEnemy = GetComponent<EnemyMovementAngry>();
        if (happyEnemy != null)
        {
            ++_gameManager._happyValue;
            --_gameManager._sadValue;
            Debug.Log(_gameManager._happyValue);
        }
        if (sadEnemy != null)
        {
            ++_gameManager._sadValue;
            --_gameManager._happyValue;
            Debug.Log(_gameManager._sadValue);

        }
        if (calmEnemy != null)
        {
            ++_gameManager._calmValue;
            --_gameManager._angryValue;
            Debug.Log(_gameManager._calmValue);

        }
        if (angryEnemy != null)
        {
            ++_gameManager._angryValue;
            --_gameManager._calmValue;
            Debug.Log(_gameManager._angryValue);

        }
    }
}
