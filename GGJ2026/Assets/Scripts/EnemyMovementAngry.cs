using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovementAngry : EnemyMovement
{
    [SerializeField]
    private float _movementSpeed = 2.0f;
    [SerializeField]
    private float _chargeSpeed = 7.0f;
    [SerializeField]
    private float _chargeCooldown = 2.0f;
    private float _timeSinceLastCharge = 0f;
    [SerializeField]
    private float _chargeAdditionalDistance = 5.0f;
    [SerializeField]
    private float _chargeAccelerationTime = 1.0f;
    private float _timeSinceChargeStart = 0f;
    private float _timeSinceChargeEnd = 0f;


    private Vector3 _chargeTarget;
    private Vector3 _chargeOriginalPosition;


    private bool _isCharging = false;



    // Update is called once per frame
    void Update()
    {
        if( _isCharging )
        {
            _timeSinceChargeStart += Time.deltaTime;
            float chargeSpeed = _chargeSpeed;
            if(_timeSinceChargeStart < _chargeAccelerationTime)
            {
                chargeSpeed *= _timeSinceChargeStart / _chargeAccelerationTime;
            }
            transform.position += (_chargeTarget - _chargeOriginalPosition).normalized * chargeSpeed * Time.deltaTime;
            Vector3 posToTargetDirection = (_chargeTarget - transform.position).normalized;
            Vector3 targetToOriginalPosDirection = (_chargeOriginalPosition - _chargeTarget).normalized;
            if (Mathf.Abs(posToTargetDirection.x - targetToOriginalPosDirection.x) < 0.001 &&
                Mathf.Abs(posToTargetDirection.y - targetToOriginalPosDirection.y) < 0.001)
            {
                _isCharging = false;
                _timeSinceLastCharge = 0f;
            }
        }
        else if(_timeSinceLastCharge > _chargeCooldown)
        {
            Vector3 playerDirection = (PlayerTransform.position - transform.position).normalized;
            _chargeTarget = PlayerTransform.position + playerDirection * _chargeAdditionalDistance;
            _chargeOriginalPosition = transform.position;
            _isCharging = true;
            _timeSinceChargeStart = 0f;
        }
        else
        {
            _timeSinceLastCharge += Time.deltaTime;
        }

    }
}
