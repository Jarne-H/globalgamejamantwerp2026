using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField]
    private Health _playerHealth;

    [SerializeField]
    private string _deathSceneName = "GAMEOVER";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(_playerHealth == null)
        {
            Debug.LogError("PlayerDeath: Player Health is not assigned, trying to find it on GameObject");
            _playerHealth = GetComponent<Health>();
            if(_playerHealth == null)
            {
                Debug.LogError("PlayerDeath: Player Health is still not found.");
                return;
            }
        }
        _playerHealth.OnDie += HandlePlayerDeath;
    }

    private void HandlePlayerDeath()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(_deathSceneName);
    }
}
