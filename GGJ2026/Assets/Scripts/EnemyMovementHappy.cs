using UnityEngine;

public class EnemyMovementHappy : EnemyMovement
{
    [SerializeField]
    private float _playerForce = 5.0f;
    [SerializeField]
    private float _perpendicularForce = 5.0f;
    [SerializeField]
    private float _maxVelocity = 7.0f;

    private Vector3 _velocityVector;

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
        if (_velocityVector.x < 0 && !_facingLeft)
        {
            GetComponentInChildren<SpriteRenderer>().flipX = true;
            _facingLeft = true;
        }
        else if (_velocityVector.x > 0 && _facingLeft)
        {
            GetComponentInChildren<SpriteRenderer>().flipX = false;
            _facingLeft = false;
        }
        transform.position += _velocityVector * Time.deltaTime;

    }

    private void FixedUpdate()
    {
        if (_gameManager == null)
        {
            _gameManager = FindAnyObjectByType<GameManager>();
        }
        if (!_gameManager.GameIsActive)
        {
            return;
        }
        Vector3 playerDir = (PlayerTransform.position - transform.position).normalized;
        Vector3 perpendicualDir = new Vector3(playerDir.y, playerDir.x, playerDir.z);
        _velocityVector += playerDir * _playerForce * Time.deltaTime;
        _velocityVector += perpendicualDir * _perpendicularForce * Time.deltaTime;

        if(_velocityVector.magnitude > _maxVelocity)
        {
            _velocityVector *= _maxVelocity / _velocityVector.magnitude;
        }

    }
}
