using UnityEngine;

public class EnemyMovementHappy : EnemyMovement
{
    [SerializeField]
    private float _playerForce = 5.0f;
    [SerializeField]
    private float _ = 5.0f;

    private bool _facingLeft = true;



    private float _elapsedTime = 0f;


    // Update is called once per frame
    void Update()
    {
        Vector3 movementDirection = Vector3.zero;
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
