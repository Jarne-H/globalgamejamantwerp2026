using System.Runtime.CompilerServices;
using UnityEngine;

public class SillyEnemies : MonoBehaviour
{
    [SerializeField]
    private GameObject _sillyPrefab;

    [SerializeField]
    private GameObject _spawnPointLeft;

    [SerializeField]
    private GameObject _spawnPointRight;

    [SerializeField]
    private float _spawnTime = 2.0f;
    private float _nextSpawnTime = 0f;
    private float _timer;

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _nextSpawnTime)
        {
            SpawnSillyEnemy();
            _nextSpawnTime = Random.Range(0.1f, _spawnTime);
        }
    }

    private void SpawnSillyEnemy()
    {
        bool flipX = false;
        Vector3 spawnPosition;
        if (Random.value > 0.5f)
        {
            spawnPosition = _spawnPointLeft.transform.position;
            flipX = true;
        }
        else
        {
            spawnPosition = _spawnPointRight.transform.position;

        }
        GameObject silly = Instantiate(_sillyPrefab, spawnPosition, Quaternion.identity);
        silly.GetComponent<Animator>().SetInteger("SillyCharacter", Random.Range(0, 9));
        silly.GetComponent<SillyEnemy>().SetFlipX(flipX);
    }
}