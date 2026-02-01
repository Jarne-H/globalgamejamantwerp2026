using UnityEngine;

public class Wobble : MonoBehaviour
{
    [SerializeField]
    private float _wobbleAmount = 1.1f;
    [SerializeField]
    private float _wobbleSpeed = 1.0f;
    private Vector3 _initialScale;
    void Awake()
    {
        _initialScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        //sin scale X and cos scale Y
        float scaleX = _initialScale.x * (1.0f + (_wobbleAmount - 1.0f) * Mathf.Sin(Time.time * _wobbleSpeed));
        float scaleY = _initialScale.y * (1.0f + (_wobbleAmount - 1.0f) * Mathf.Cos(Time.time * _wobbleSpeed));
        transform.localScale = new Vector3(scaleX, scaleY, _initialScale.z);
    }
}
