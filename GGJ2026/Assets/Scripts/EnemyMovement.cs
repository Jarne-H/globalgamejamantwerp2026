using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    private Transform _playerTransform;

    public Transform PlayerTransform
    {
        get {  return _playerTransform; }
        set { _playerTransform = value; }
    }
}
