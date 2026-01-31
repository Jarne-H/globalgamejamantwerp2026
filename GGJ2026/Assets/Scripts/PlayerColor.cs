using UnityEngine;

public class PlayerColor : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _visualiser;
    [SerializeField]
    private Color _playerColor = Color.white;
    [SerializeField]
    private bool _rainbowEffect = false;
    [SerializeField]
    private float _rainbowSpeed = 1.0f;
    [SerializeField]
    private bool _randomColorOnStart = false;

    [SerializeField]

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        if (_randomColorOnStart)
        {
            _playerColor = Random.ColorHSV();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_rainbowEffect)
        {
            float hue = Mathf.PingPong(Time.time * _rainbowSpeed, 1);
            Color rainbowColor = Color.HSVToRGB(hue, 1, 1);
            _visualiser.material.color = rainbowColor;
        }
        else
        {
            _visualiser.material.color = _playerColor;
        }
    }

    public void GiveNewRandomColor()
    {
        _playerColor = Random.ColorHSV();
    }

    public void EnableRainbow()
    {
        _rainbowEffect = true;
    }

    public void DisableRainbow()
    {
        _rainbowEffect = false;
    }

    public void EnableRainbowForSeconds(float seconds)
    {
        Invoke("DisableRainbow", seconds);
        EnableRainbow();
    }
}
