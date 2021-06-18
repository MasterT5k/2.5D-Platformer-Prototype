using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private float _gravity = 1.0f;
    [SerializeField]
    private float _jumpHeight = 15.0f;
    private float _yVelocity;
    private bool _canDoubleJump = false;
    private int _coins;

    private CharacterController _controller;
    private UIManager _uiManager;

    void Start()
    {
        _controller = GetComponent<CharacterController>();

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL."); 
        }
        else
        {
            _uiManager.UpdateCoinDisplay(_coins);
            _uiManager.UpdateLivesDisplay(_lives);
        }        
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 direction = new Vector3(horizontalInput, 0, 0);
        Vector3 velocity = direction * _speed;

        if (_controller.isGrounded == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _yVelocity = _jumpHeight;
                _canDoubleJump = true;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_canDoubleJump == true)
                {
                    _yVelocity += _jumpHeight;
                    _canDoubleJump = false;
                }
            }

            _yVelocity -= _gravity;
        }

        velocity.y = _yVelocity;

        if (_controller.enabled != false)
        {
            _controller.Move(velocity * Time.deltaTime); 
        }
    }

    public void AddCoins()
    {
        _coins++;

        if (_uiManager != null)
        {
            _uiManager.UpdateCoinDisplay(_coins); 
        }
    }

    public void Damage()
    {
        _lives--;

        if (_uiManager != null)
        {
            _uiManager.UpdateLivesDisplay(_lives); 
        }

        if (_lives < 1)
        {
            SceneManager.LoadScene(0);
        }
    }

    public int CheckCoins()
    {
        return _coins;
    }
}
