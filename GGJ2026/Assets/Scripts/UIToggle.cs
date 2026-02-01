using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIToggle : MonoBehaviour
{
    [SerializeField]
    private GameObject _uiElement;
    [SerializeField]
    private PlayerInput _playerInput;
    [SerializeField]
    private string _connectedAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_uiElement != null)
        {
            _uiElement.SetActive(!_uiElement.activeSelf);
        }

        if (_playerInput != null && !string.IsNullOrEmpty(_connectedAction))
        {
            var action = _playerInput.actions[_connectedAction];
            if (action != null)
            {
                action.performed += ctx => ToggleUIElement();
            }
            else
            {
                Debug.LogWarning($"Action '{_connectedAction}' not found in PlayerInput.");
            }
        }
        else
        {
            Debug.LogWarning("PlayerInput or ConnectedAction is not set.");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleUIElement()
    {
        if (_uiElement != null)
        {
            _uiElement.SetActive(!_uiElement.activeSelf);
        }
    }
}
