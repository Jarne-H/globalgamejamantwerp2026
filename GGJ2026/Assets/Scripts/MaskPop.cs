using UnityEngine;

public class MaskPop : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rb;

    [SerializeField]
    private Sprite[] _maskSprites;
    public int SpriteToUse = 1; // 1-based index

    [SerializeField]
    private float _lifetime = 0.5f;
    private float _age = 0f;
    [SerializeField]
    private float _fadeDuration = 1.5f;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    private void Awake()
    {
        if(_rb == null)
            _rb = GetComponent<Rigidbody2D>();
        if(_spriteRenderer == null)
            _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //set sprite based on SpriteToUse
        if (_maskSprites != null && _maskSprites.Length >= SpriteToUse && SpriteToUse > 0)
        {
            if (_spriteRenderer != null)
                _spriteRenderer.sprite = _maskSprites[SpriteToUse - 1];
        }
        //shoot in random direction 45deg to the left or right from up
        float angle = Random.Range(-45f, 45f);
        Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.up;
        float force = Random.Range(200f, 400f);
        _rb.AddForce(direction * force);

        //random angle speed
        float torque = Random.Range(-200f, 200f);
        _rb.AddTorque(torque);
    }

    private void Update()
    {
        _age += Time.deltaTime;
        if(_age >= _lifetime)
        {
            float fadeAmount = (_age - _lifetime) / _fadeDuration;
            Color color = _spriteRenderer.color;
            color.a = Mathf.Lerp(1f, 0f, fadeAmount);
            _spriteRenderer.color = color;
            if(fadeAmount >= 1f)
            {
                Destroy(gameObject);
            }
        }
    }
}
