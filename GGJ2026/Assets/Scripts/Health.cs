using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 1;
    private int currentHealth;

    [SerializeField]
    private bool isInvincible = false;

    [SerializeField]
    private float invincibilityDuration = 1.0f;
    private float invincibilityTimer = 0.0f;

    [SerializeField]
    private float healthRegenRate = 0.0f;//amount of time between regenerating 1 health point
    private float healthRegenTimer = 0.0f;

    //die event
    public delegate void DieEvent();
    public event DieEvent OnDie;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInvincible)
        {
            invincibilityTimer += Time.deltaTime;
            if (invincibilityTimer >= invincibilityDuration)
            {
                isInvincible = false;
                invincibilityTimer = 0.0f;
            }
        }

        if (healthRegenRate > 0 && currentHealth < maxHealth)
        {
            healthRegenTimer += Time.deltaTime;
            if (healthRegenTimer >= healthRegenRate)
            {
                AdjustHealth(1);
                healthRegenTimer = 0.0f;
            }
        }
    }

    public void AdjustHealth(int amount)
    {
        if (amount < 0 && isInvincible)
        {
            return;
        }
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (amount < 0)
        {
            isInvincible = true;
            invincibilityTimer = 0.0f;
        }
        if (currentHealth <= 0)
        {
            OnDie?.Invoke();
        }
    }
}
