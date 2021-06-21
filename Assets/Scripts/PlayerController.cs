using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private float _gravity = 1f;
    [SerializeField]
    private float _jumpHeight = 15f;

    private bool _flip = false;
    private bool _jumping = false;
    private float _yVelocity;
    private Vector3 _direction;
    private Vector3 _velocity;
    private CharacterController _controller = null;
    private Animator _anim = null;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _anim = GetComponentInChildren<Animator>();
        if (_anim == null)
        {
            Debug.LogError("Animator is Null");
        }
    }

    void Update()
    {       
        if (_controller.isGrounded == true)
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            _direction = new Vector3(0, 0, horizontalInput);
            _velocity = _direction * _speed;

            if (_jumping == true)
            {
                _jumping = false;
            }

            if (horizontalInput < 0 && _flip == false)
            {
                _flip = true;
                transform.rotation = Quaternion.Euler(transform.rotation.x, -180f, transform.rotation.z);
            }
            else if (horizontalInput > 0 & _flip == true)
            {
                _flip = false;
                transform.rotation = Quaternion.Euler(transform.rotation.x, 0f, transform.rotation.z);
            }

            if (_anim != null)
            {
                _anim.SetFloat("Speed", Mathf.Abs(horizontalInput));
                _anim.SetBool("Jump", false);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _jumping = true;
                _yVelocity = _jumpHeight;
                _anim.SetBool("Jump", true);
            }
        }
        else
        {
            _yVelocity -= _gravity * Time.deltaTime;
        }

        _velocity.y = _yVelocity;
        _controller.Move(_velocity * Time.deltaTime);
    }
}
