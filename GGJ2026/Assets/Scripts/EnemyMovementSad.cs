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


    // Update is called once per frame
    void Update()
    {
        float distance = (PlayerTransform.position - transform.position).magnitude;
        _waveOffsetCalculated = _waveOffset;
        if (distance < _distanceMaxWaveOffset)
        {
            _waveOffsetCalculated = _waveOffset * distance / _distanceMaxWaveOffset;
        }
        _elapsedTime += Time.deltaTime;
        Vector3 movementDirection = (PlayerTransform.position - transform.position).normalized;
        transform.position += movementDirection * _movementSpeed * Time.deltaTime + new Vector3(movementDirection.y, movementDirection.x, 0f) * _waveOffsetCalculated * Mathf.Sin(_elapsedTime * _waveFrequency) * Time.deltaTime;
    }
}
