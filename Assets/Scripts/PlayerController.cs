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

    private bool _grabbingLedge = false;
    private bool _flip = false;
    private bool _jumping = false;
    private float _yVelocity;
    private Vector3 _direction;
    private Vector3 _velocity;
    private CharacterController _controller = null;
    private Animator _anim = null;
    private Ledge _activeLedge = null;

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
        if (_grabbingLedge == false)
        {
            CalculateMovement();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (_anim != null)
                {
                    _anim.SetTrigger("ClimbUp");
                }
            }
        }
    }

    void CalculateMovement()
    {
        if (_controller.isGrounded == true)
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            _direction = new Vector3(0, 0, horizontalInput);
            _velocity = _direction * _speed;

            if (_jumping == true)
            {
                _jumping = false;
                if (_anim != null)
                {
                    _anim.SetBool("Jump", false);
                }
            }

            if (horizontalInput < 0 && _flip == false)
            {
                _flip = true;
                transform.rotation = Quaternion.Euler(transform.rotation.x, 180f, transform.rotation.z);
            }
            else if (horizontalInput > 0 & _flip == true)
            {
                _flip = false;
                transform.rotation = Quaternion.Euler(transform.rotation.x, 0f, transform.rotation.z);
            }

            if (_anim != null)
            {
                _anim.SetFloat("Speed", Mathf.Abs(horizontalInput));
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

    public void GrabLedge(Vector3 position, Ledge currentLedge)
    {
        transform.position = position;
        _grabbingLedge = true;
        _anim.SetBool("GrabLedge", true);
        _anim.SetBool("Jump", false);
        _anim.SetFloat("Speed", 0);
        _controller.enabled = false;
        if (currentLedge != null)
        {
            _activeLedge = currentLedge;
        }
    }

    public void LedgeClimb()
    {
        if (_activeLedge != null)
        {
            Vector3 position = _activeLedge.GetFinalPosition();
            transform.position = position;
        }
        _grabbingLedge = false;
        _anim.SetBool("GrabLedge", false);
        _controller.enabled = true;
    }
}
