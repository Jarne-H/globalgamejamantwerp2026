using UnityEngine;

public class SmokeLogic : MonoBehaviour
{
    [SerializeField]
    private float _lifetime = 2.0f;
    private float _timeElapsed = 0.0f;

    // Update is called once per frame
    void Update()
    {
        _timeElapsed += Time.deltaTime;
        if (_timeElapsed >= _lifetime)
        {
            Destroy(gameObject);
        }
    }
}
