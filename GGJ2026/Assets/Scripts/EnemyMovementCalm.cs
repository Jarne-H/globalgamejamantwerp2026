using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class EnemyMovementCalm : EnemyMovement
{
    [SerializeField]
    private float _movementSpeed = 5.0f;
    [SerializeField]
    private float _playerAngleClampSize_div_2 = 45f;
    private float _movementDirectionAngle = 0f;

    [SerializeField]
    private float _angleTargetChangeSpeed = 90f;
    [SerializeField]
    private float _angleChangeSpeed = 0.5f;
    private float _angleTarget = 0f;

    [SerializeField]
    private float _angleChangeCooldown = 2f;
    private float _timeSinceLastAngleChange = 0f;

    private bool _facingLeft = true;


    private void Start()
    {
        Vector3 playerDirection = (PlayerTransform.position - transform.position).normalized;
        float playerAngle = Mathf.Rad2Deg * Mathf.Atan2(playerDirection.y, playerDirection.x);
        _movementDirectionAngle = playerAngle;

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
        Vector3 playerDirection = (PlayerTransform.position - transform.position).normalized;
        float playerAngle = Mathf.Rad2Deg * Mathf.Atan2(playerDirection.y, playerDirection.x);

        _timeSinceLastAngleChange += Time.deltaTime;
        if(_timeSinceLastAngleChange > _angleChangeCooldown)
        {
            _timeSinceLastAngleChange -= _angleChangeCooldown;
            _angleTarget = playerAngle + (Random.value - 0.5f) * _angleTargetChangeSpeed;
        }

        _movementDirectionAngle = Mathf.MoveTowardsAngle(_movementDirectionAngle, _angleTarget, _angleChangeSpeed);

        Vector3 movementDirection = new Vector3(Mathf.Cos(Mathf.Deg2Rad * _movementDirectionAngle), Mathf.Sin(Mathf.Deg2Rad * _movementDirectionAngle), 0).normalized;
        transform.position += movementDirection * _movementSpeed * Time.deltaTime;

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
    }
}
