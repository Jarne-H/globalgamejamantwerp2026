using UnityEngine;

public class EnemyMovementSad : EnemyMovement
{
    [SerializeField]
    private float _movementSpeed = 5.0f;
    [SerializeField]
    private float _waveOffset = 1.0f;
    [SerializeField]
    private float _waveFrequency = 1.0f;
    [SerializeField]
    private float _distanceMaxWaveOffset = 6f;
    private float _waveOffsetCalculated = 0f;

    private float _elapsedTime = 0f;

    private bool _facingLeft;

    private GameManager _gameManager;

    private void Start()
    {
        if (PlayerTransform.position.x < transform.position.x)
        {
            _facingLeft = false;
        }
        else
        {
            _facingLeft = true;
        }
    }
    // Update is called once per frame
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
        float distance = (PlayerTransform.position - transform.position).magnitude;
        _waveOffsetCalculated = _waveOffset;
        if (distance < _distanceMaxWaveOffset)
        {
            _waveOffsetCalculated = _waveOffset * distance / _distanceMaxWaveOffset;
        }
        _elapsedTime += Time.deltaTime;
        Vector3 movementDirection = (PlayerTransform.position - transform.position).normalized;
        if (movementDirection.x < 0 && !_facingLeft)
        {
            GetComponentInChildren<SpriteRenderer>().flipX = true;
            _facingLeft = true;
        }
        else if (movementDirection.x > 0 && _facingLeft)
        {
            GetComponentInChildren<SpriteRenderer>().flipX = false;
            _facingLeft = false;
        }
        transform.position += movementDirection * _movementSpeed * Time.deltaTime + new Vector3(movementDirection.y, movementDirection.x, 0f) * _waveOffsetCalculated * Mathf.Sin(_elapsedTime * _waveFrequency) * Time.deltaTime;
    }
}
