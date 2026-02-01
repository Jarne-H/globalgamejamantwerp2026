using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArrowSpawner : MonoBehaviour
{
    [SerializeField]
    private PlayerInput _playerInput;
    private InputAction _shoot;
    private float _shootInput;

    //[SerializeField]
    //private float _shootSpeed;
    //private float _timeSinceLastShot = 0f;

    [SerializeField]
    private float _chargingTime = 2.3f;
    private float _currentChargingTime = 0f;
    private bool _chargingIsReady = false;

    [SerializeField]
    private GameObject _arrowPrefab;

    [Header("GameJuice")]
    [SerializeField]
    private GameObject _visualisation;
    [SerializeField]
    private Vector3 _originalVisualisationScale = new Vector3(1, 1, 1);
    [SerializeField]
    private GameManager _gameManager;
    [SerializeField]
    private Animator _bowAnimation;

    [Header("PowerUpStuff")]
    [SerializeField]
    private int _improvedPierceValue;
    [SerializeField]
    private float _multishotSpreadAngle;

    private int _chargePulseCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _originalVisualisationScale = _visualisation.transform.localScale;
        if (_playerInput == null)
        {
            _playerInput = FindAnyObjectByType<PlayerInput>();
        }
        _shoot = _playerInput.actions["Attack"];
        //while shoot is being pressed, read value
        _shoot.performed += ctx => _shootInput = ctx.ReadValue<float>();
        _shoot.canceled += ctx => _shootInput = ctx.ReadValue<float>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_chargingIsReady && _currentChargingTime < _chargingTime)
        {
            _currentChargingTime += Time.deltaTime;
        }
        else
        {
            _chargingIsReady = true;
        }
        Aim();
        if (!_gameManager.GameIsActive)
        {
            return;
        }
        if (_shootInput > 0)
        {
            Charge();
        }
        else
        {
            if (_chargingIsReady)
            {
                Shoot();
            }
            _currentChargingTime = 0f;
            _chargingIsReady = false;
        }
        ChargeAnimation();
        //else
        //    _timeSinceLastShot += Time.deltaTime;
        ////clamp time since last shot
        //if (_timeSinceLastShot > _shootSpeed)
        //    _timeSinceLastShot = _shootSpeed + 1;
    }

    private void Aim()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        //rotate the transform to face the mouse position in 2D space
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }

    private void Charge()
    {
        _currentChargingTime += Time.deltaTime;
        //clamp current charging time
        if (_currentChargingTime >= _chargingTime)
        {
            _chargingIsReady = true;
            _currentChargingTime = _chargingTime;
        }
    }

    private void ChargeAnimation()
    {
        //every 1/3 of charging time, do a pulse
        switch (_currentChargingTime)
        {
            case float n when (n >= (_chargingTime / 3) * (_chargePulseCount + 1)) && (_chargePulseCount < 3):
                StartCoroutine(BowPulse());
                _chargePulseCount++;
                break;
            case float n when (n < (_chargingTime / 3)):
                _chargePulseCount = 0;
                break;
        }
        if (!_gameManager.GameIsActive)
        {
            return;
        }
        if (_shootInput > 0)
        {
            _bowAnimation.SetBool("IsCharging", true);
        }
        else
        {
            _bowAnimation.SetBool("IsCharging", false);
        }
            if (!_chargingIsReady)
        {
            _bowAnimation.SetBool("IsCharged", false);
            //scale over y axis based on charging time
            //float scaleY = Mathf.Lerp(1, 1.5f, _currentChargingTime / _chargingTime);
            //_visualisation.transform.localScale = new Vector3(_originalVisualisationScale.x, scaleY, _originalVisualisationScale.z);
        }
        else
        {
            _bowAnimation.SetBool("IsCharged", true);
            //_visualisation.transform.localScale = _originalVisualisationScale;
        }
    }

    private IEnumerator BowPulse()
    {
        //increase visualisation scale quickly to 1.1f over 0.1 seconds
        Vector3 targetScale = _originalVisualisationScale * 1.1f;
        float elapsedTime = 0f;
        while (elapsedTime < 0.1f)
        {
            _visualisation.transform.localScale = Vector3.Lerp(_originalVisualisationScale, targetScale, (elapsedTime / 0.1f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private void Shoot()
    {
        if (_chargingIsReady)
        {
            Debug.Log("Shoot arrow");
            _bowAnimation.SetBool("IsCharging", false);
            _bowAnimation.SetBool("IsCharged", false);
            //Instantiate arrow prefab
            GameObject arrow = Instantiate(_arrowPrefab, transform.position, transform.rotation);
            if (_gameManager._nextArrowsPiercing > 0)
            {
                --_gameManager._nextArrowsPiercing;
                Arrow arrowScript = arrow.GetComponent<Arrow>();
                arrowScript.pierceLevel = 3;
            }
            //rotate arrow to face mouse position
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            arrow.transform.LookAt(mousePosition);
            //reset shoot input
            _shootInput = 0;
            _chargingIsReady = false;
            if(_gameManager._nextArrowsMultiShot > 0)
            {
                --_gameManager._nextArrowsMultiShot;
                GameObject arrowLeft = Instantiate(_arrowPrefab, transform.position, Quaternion.Euler(30,30,30));
                arrowLeft.GetComponent<Arrow>().angleIncrement = _multishotSpreadAngle;
                GameObject arrowRight = Instantiate(_arrowPrefab, transform.position, Quaternion.Euler(30, 30, 30));
                arrowRight.GetComponent<Arrow>().angleIncrement = -_multishotSpreadAngle;

            }

        }
        else
        {
            Debug.Log("Not charged enough to shoot");
        }
    }
}
