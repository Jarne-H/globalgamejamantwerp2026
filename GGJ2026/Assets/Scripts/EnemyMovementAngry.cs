using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyMovementAngry : EnemyMovement
{
    [SerializeField]
    private float _movementSpeed = 2.0f;

    [Header("ChargeProperties")]
    [SerializeField]
    private float _chargeSpeed = 7.0f;
    [SerializeField]
    private float _chargeCooldown = 5.0f;
    private float _timeSinceLastCharge = 0f;
    [SerializeField]
    private float _chargeAdditionalDistance = 5.0f;
    [SerializeField]
    private float _chargeRadius = 8.0f;

    [Header("ChargeSmoothing")]
    [SerializeField]
    private float _chargeAccelerationTime = 1.0f;
    private float _timeSinceChargeStart = 0f;
    [SerializeField]
    private float _chargeDecelerationTime = 1.0f;
    private float _timeSinceChargeEnd = 0f;
    //[SerializeField]
    //private float _chargeEndIdleTime = 0.2f;
    //private float _chargeIdleTime = 0f;

    private bool _facingLeft = true;

    private Vector3 _chargeTarget;
    private Vector3 _chargeOriginalPosition;


    private bool _isCharging = false;



    // Update is called once per frame
    void Update()
    {
        //_chargeIdleTime += Time.deltaTime;
        //if (_chargeIdleTime < _chargeEndIdleTime)
        //{
        //    return;
        //}
        if (_isCharging)
        {
            _timeSinceChargeStart += Time.deltaTime;
            float chargeSpeed = _chargeSpeed;
            if(_timeSinceChargeStart < _chargeAccelerationTime)
            {
                chargeSpeed *= _timeSinceChargeStart / _chargeAccelerationTime;
            }

            if(_timeSinceChargeEnd > 0)
            {
                chargeSpeed *= (_chargeDecelerationTime - _timeSinceChargeEnd) / _chargeDecelerationTime;
            }
            transform.position += (_chargeTarget - _chargeOriginalPosition).normalized * (chargeSpeed + _movementSpeed) * Time.deltaTime;

            Vector3 posToTargetDirection = (_chargeTarget - transform.position).normalized;
            Vector3 targetToOriginalPosDirection = (_chargeOriginalPosition - _chargeTarget).normalized;
            if (Vector3.Angle(posToTargetDirection, targetToOriginalPosDirection) < 90)
            {
                _timeSinceChargeEnd += Time.deltaTime;
                if(_timeSinceChargeEnd > _chargeDecelerationTime)
                {
                    _isCharging = false;
                    _timeSinceLastCharge = 0f;
                    _timeSinceChargeEnd = 0f;
                    //_chargeIdleTime = 0;
                }
            }
        }
        else
        {
            _timeSinceLastCharge += Time.deltaTime;

            transform.position += (PlayerTransform.position - transform.position).normalized * _movementSpeed * Time.deltaTime;

            Vector3 movementDirection = PlayerTransform.position - transform.position;
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

            if ((PlayerTransform.position - transform.position).magnitude < _chargeRadius && _timeSinceLastCharge > _chargeCooldown)
            {
                Vector3 playerDirection = (PlayerTransform.position - transform.position).normalized;
                _chargeTarget = PlayerTransform.position + playerDirection * _chargeAdditionalDistance;
                _chargeOriginalPosition = transform.position;
                _isCharging = true;
                _timeSinceChargeStart = 0f;
            }
        }

    }
}
