using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _enemyPrefabs;

    [SerializeField]
    private int _timeBetweenSpawns = 5;
    private float _timeSinceLastSpawn = 0f;

    [SerializeField]
    private Transform _playerTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _timeSinceLastSpawn += Time.deltaTime;
        if(_timeSinceLastSpawn > _timeBetweenSpawns)
        { 
            _timeSinceLastSpawn -= _timeBetweenSpawns;
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        if (_playerTransform == null) { return; }
        int enemyPrefabIdx = Random.Range(0, _enemyPrefabs.Count);
        GameObject enemy = GameObject.Instantiate(_enemyPrefabs[enemyPrefabIdx]);
        EnemyMovement enemyMovement = (EnemyMovement) enemy.GetComponent("EnemyMovement");
        enemyMovement.PlayerTransform = _playerTransform;
    }
}
