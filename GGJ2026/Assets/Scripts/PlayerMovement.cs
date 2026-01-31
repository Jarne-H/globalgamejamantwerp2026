using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Input settings")]
    [SerializeField]
    private PlayerInput _playerInput;
    private InputAction _moveAction;

    [Header("Movement settings")]
    [SerializeField]
    private float _movementSpeed = 5.0f;
    [SerializeField]
    private float _movementSpeedWhenBoosted = 7.0f;

    private float _currentMovementSpeed;

    //Variables to be rememebered
    private Vector2 _movementInput;

    [Header("GameJuice")]
    [SerializeField]
    private GameManager _gameManager;
    [SerializeField]
    private GameObject _playerVisualization;
    [SerializeField]
    private float _walkBobbingAmount = 0.5f;
    [SerializeField]
    private float _bobbingAmplitude = 10.0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _moveAction = _playerInput.actions["Move"];
        _moveAction.performed += ctx => UpdateMovementInput();
        _moveAction.canceled += ctx => UpdateMovementInput();
        if (_gameManager == null)
        {
            _gameManager = GameManager.Instance;
        }
        _currentMovementSpeed = _movementSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameManager == null)
        {
            _gameManager = GameManager.Instance;
        }
        if (_gameManager.GameIsActive)
        {
            if (_gameManager._speedBoostEnabled)
            {
                _currentMovementSpeed = _movementSpeedWhenBoosted;
            }
            else
            {
                _currentMovementSpeed = _movementSpeed;
            }
            MovePlayer();
            MovementAnimation();
        }
    }

    private void UpdateMovementInput()
    {
        _movementInput = _moveAction.ReadValue<Vector2>();
    }

    private void MovePlayer()
    {
        //when moving left, flip the sprite renderer on X axis
        if (_movementInput.x < 0)
        {
            _playerVisualization.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (_movementInput.x > 0)
        {
            _playerVisualization.GetComponent<SpriteRenderer>().flipX = false;
        }
            Vector3 movement = new Vector3(_movementInput.x, _movementInput.y, 0);
        transform.Translate(movement * _currentMovementSpeed * Time.deltaTime, Space.World);
    }

    private void MovementAnimation()
    {
        //adjust X and Y scale of _playerVisualization based on movement input
        if (_movementInput != Vector2.zero)
        {
            float bobbingX = 1 + Mathf.Sin(Time.time * _bobbingAmplitude) * _walkBobbingAmount;
            float bobbingY = 1 + Mathf.Cos(Time.time * _bobbingAmplitude) * _walkBobbingAmount;
            _playerVisualization.transform.localScale = new Vector3(bobbingX, bobbingY, 0);
            Animator animator = _playerVisualization.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetBool("IsRunning", true);
            }
        }
        else
        {
            _playerVisualization.transform.localScale = Vector3.one;
            Animator animator = _playerVisualization.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetBool("IsRunning", false);
            }   
        }
    }
}
