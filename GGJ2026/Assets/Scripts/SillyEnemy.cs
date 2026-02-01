using UnityEngine;

public class SillyEnemy : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private bool _flipX = false;

    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    void Update()
    {
        transform.Translate(Vector3.left * _moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(_flipX)
        {
            if (collision.gameObject.CompareTag("RightBoundary"))
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (collision.gameObject.CompareTag("LeftBoundary"))
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetFlipX(bool flipX)
    {
        //flip the sprite on the X axis if needed
        _flipX = flipX;
        if (_flipX)
        {
            _spriteRenderer.flipX = true;
        }
    }
}
