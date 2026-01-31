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
    private bool _isInvincible = false;

    [SerializeField]
    private float _invincibilityDurationAtStart = 3.0f;
    private float _invincibilityCurrentDuration = 0.0f;
    [SerializeField]
    private float _invincibilityTimeAfterDeath = 2.8f;

    [SerializeField]
    private float _healthRegenRate = 0.0f;//amount of time between regenerating 1 health point
    private float _healthRegenTimer = 0.0f;

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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentHealth = _maxHealth;
        _currentLives = _maxLives;
        _invincibilityCurrentDuration = _invincibilityDurationAtStart;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isInvincible)
        {
            _invincibilityCurrentDuration -= Time.deltaTime;
            if (_invincibilityCurrentDuration <= 0.0f)
            {
                _isInvincible = false;
                _invincibilityCurrentDuration = _invincibilityDurationAtStart;
            }
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
        if (_isPlayer)
        {
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
        if (amount < 0 && _isInvincible)
        {
            return;
        }
        _currentHealth += amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

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
        //reset current scene
        string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentSceneName);
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
            Destroy(gameObject);
        }
    }

    private void HandlePlayerRespawn()
    {
        //find all objects with tag "enemy" and destroy them if they are visible on screen with a 5% margin
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Vector3 screenPoint = Camera.main.WorldToViewportPoint(enemy.transform.position);
            if (screenPoint.x >= -0.05f && screenPoint.x <= 1.05f && screenPoint.y >= -0.05f && screenPoint.y <= 1.05f)
            {
                Destroy(enemy);
            }
        }

        // Implement player respawn logic here (e.g., reset position, play animation, etc.)
        Debug.Log("Player respawned. Lives left: " + _currentLives);
    }

    private void HandleEnemyRespawn()
    {
        // Implement enemy respawn logic here (e.g., reset position, play animation, etc.)
        Debug.Log("Enemy respawned.");
    }

    public void AddLife()
    {
        _currentLives++;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isPlayer && collision.gameObject.CompareTag("Enemy"))
        {
            AdjustHealth(-1);
        }
    }
}
