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

    private bool _facingLeft = false;
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
    }

    private void FixedUpdate()
    {
        Vector3 playerDir = (PlayerTransform.position - transform.position).normalized;
        Vector3 perpendicualDir = new Vector3(playerDir.y, playerDir.x, playerDir.z);
        _velocityVector += playerDir * _playerForce * Time.deltaTime;
        _velocityVector += perpendicualDir * _perpendicularForce * Time.deltaTime;

        if(_velocityVector.magnitude > _maxVelocity)
        {
            _velocityVector *= _maxVelocity / _velocityVector.magnitude;
        }

        transform.position += _velocityVector * Time.deltaTime;
    }
}
