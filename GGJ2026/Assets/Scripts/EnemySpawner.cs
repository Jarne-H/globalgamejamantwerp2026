using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _enemyPrefabs;

    [Header("SpawnTime")]
    [SerializeField]
    private float _timeBetweenSpawns = 5f;
    private float _timeSinceLastSpawn = 0f;

    [SerializeField]
    private float _spawnTimeMultiplier = 0.9f;
    [SerializeField]
    private float _spawnTimeMultiplyCooldown = 10f;
    private float _spawnTimeMultiplyElapsedTime = 0f;

    [Header("SpawnDistance")]
    [SerializeField]
    private Transform _playerTransform;
    [SerializeField]
    private float _enemySpawnDistanceMaxX;
    [SerializeField]
    private float _enemySpawnDistanceMaxY;
    [SerializeField]
    private float _enemySpawnDistanceMinX;
    [SerializeField]
    private float _enemySpawnDistanceMinY;

    private GameManager _gameManager;

    void Update()
    {
        if (_gameManager == null)
        {
            _gameManager = FindAnyObjectByType<GameManager>();
        }
        if (!_gameManager.GameIsActive)
        {
            return;
        }
        _timeSinceLastSpawn += Time.deltaTime;
        _spawnTimeMultiplyElapsedTime += Time.deltaTime;
        if (_timeSinceLastSpawn > _timeBetweenSpawns)
        { 
            _timeSinceLastSpawn -= _timeBetweenSpawns;
            SpawnEnemy();
        }
        if(_spawnTimeMultiplyElapsedTime > _spawnTimeMultiplyCooldown)
        {
            _spawnTimeMultiplyElapsedTime -= _spawnTimeMultiplyCooldown;
            _timeBetweenSpawns *= _spawnTimeMultiplier;
        }
    }

    private void SpawnEnemy()
    {
        if (_playerTransform == null) { return; }
        int enemyPrefabIdx = UnityEngine.Random.Range(0, _enemyPrefabs.Count);
        GameObject enemy = GameObject.Instantiate(_enemyPrefabs[enemyPrefabIdx]);
        EnemyMovement enemyMovement = (EnemyMovement) enemy.GetComponent("EnemyMovement");
        enemyMovement.PlayerTransform = _playerTransform;
        bool invertedX = Convert.ToBoolean(UnityEngine.Random.Range(0, 2));
        bool invertedY = Convert.ToBoolean(UnityEngine.Random.Range(0, 2));
        int x;
        int y;
        if (invertedX) { x = -1; }
        else { x = 1; }
        if (invertedY) { y = -1; }
        else { y = 1; }

        enemyMovement.transform.position = _playerTransform.position + new Vector3(
            x * _enemySpawnDistanceMinX + UnityEngine.Random.Range(0, _enemySpawnDistanceMaxX - _enemySpawnDistanceMinX),
            y * _enemySpawnDistanceMinY + UnityEngine.Random.Range(0, _enemySpawnDistanceMaxY - _enemySpawnDistanceMinY),
            0);
    }
}
