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
    private float _speed = 5f;
    [SerializeField]
    private float _gravity = 1f;
    [SerializeField]
    private float _jumpHeight = 15f;
    [SerializeField]
    private float _gravityDivider = 4f;
    private float _yVelocity;
    private bool _canDoubleJump = false;
    private int _coins;
    private bool _canWallJump = false;
    private Vector3 _direction, _velocity;
    private Vector3 _wallHitNormal;
    private bool _touchingWall = false;

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

        if (_controller.isGrounded == true)
        {
            _touchingWall = false;
            _canWallJump = false;
            _direction = new Vector3(horizontalInput, 0);
            _velocity = _direction * _speed;

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
                if (_canWallJump == true)
                {
                    _velocity = _wallHitNormal * _speed;
                    _yVelocity = _jumpHeight;
                    _canWallJump = false;
                    _canDoubleJump = false;
                    _touchingWall = false;
                }
                else if (_canDoubleJump == true)
                {
                    _yVelocity += _jumpHeight;
                    _canDoubleJump = false;
                }
            }            
        }

        _velocity.y = _yVelocity;

        if (_controller.enabled != false)
        {
            _controller.Move(_velocity * Time.deltaTime); 
        }
    }

    private void FixedUpdate()
    {
        if (_controller.isGrounded == false)
        {
            if (_touchingWall == false)
            {
                _yVelocity -= _gravity; 
            }
            else
            {
                _yVelocity -= _gravity / _gravityDivider;
            }
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

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (_controller.isGrounded == false && hit.transform.tag == "Wall")
        {
            _touchingWall = true;
            _wallHitNormal = hit.normal;
            _canWallJump = true;
            Debug.DrawRay(hit.point, hit.normal, Color.blue);
        }
    }
}
