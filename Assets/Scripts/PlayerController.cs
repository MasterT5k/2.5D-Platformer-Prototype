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

    [Header("Roll Settings")]
    [SerializeField]
    private float _rollForce = 5f;
    [SerializeField]
    private Vector3 _colliderCenterSmall;
    private Vector3 _colliderCenterStart;
    [SerializeField]
    private float _colliderHeightSmall = 1f;
    private float _colliderHeightStart;

    [Header("Ladder Settings")]
    [SerializeField]
    private float _climbSpeed = 4f;
    [SerializeField]
    private bool _climbingLadder = false;
    [SerializeField]
    private Ladder _currentLadder = null;
    [SerializeField]
    private bool _nextToLadder = false;
    [SerializeField]
    private bool _startAtBottom = false;


    private bool _rolling = false;
    private bool _grabbingLedge = false;
    [SerializeField]
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

        _colliderCenterStart = _controller.center;
        _colliderHeightStart = _controller.height;

        _anim = GetComponentInChildren<Animator>();
        if (_anim == null)
        {
            Debug.LogError("Animator is Null");
        }
    }

    void Update()
    {
        if (_grabbingLedge == false && _climbingLadder == false)
        {
            CalculateMovement();
        }
        else if (_grabbingLedge == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (_anim != null)
                {
                    _anim.SetTrigger("ClimbUp");
                }
            }
        }
        else if (_climbingLadder == true)
        {
            if (_startAtBottom == true)
            {
                _direction = new Vector3(0, 1, 0); 
            }
            else
            {
                _direction = new Vector3(0, -1, 0);
                if (transform.position.y <= _currentLadder.GetBottomEndPosition().y)
                {
                    EndLadderClimb();
                    return;
                }
            }
            _velocity = _direction * _climbSpeed;
            _controller.Move(_velocity * Time.deltaTime);
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
                FlipPlayer(_flip);
            }
            else if (horizontalInput > 0 & _flip == true)
            {
                _flip = false;
                FlipPlayer(_flip);
            }

            if (_anim != null)
            {
                _anim.SetFloat("Speed", Mathf.Abs(horizontalInput));
            }

            if (Input.GetKeyDown(KeyCode.Space) && _rolling == false)
            {
                _jumping = true;
                _yVelocity = _jumpHeight;
                _anim.SetBool("Jump", true);
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && _rolling == false)
            {
                _anim.SetTrigger("Roll");
                _rolling = true;
                _controller.center = _colliderCenterSmall;
                _controller.height = _colliderHeightSmall;
            }

            if (_rolling == true)
            {
                if (_flip == false)
                {
                    _velocity.z += _rollForce;
                }
                else
                {
                    _velocity.z -= _rollForce;
                }
            }
        }
        else
        {
            _yVelocity -= _gravity * Time.deltaTime;
        }

        _velocity.y = _yVelocity;
        if (_controller.enabled == true)
        {
            _controller.Move(_velocity * Time.deltaTime); 
        }

        if (Input.GetKeyDown(KeyCode.E) && _nextToLadder == true)
        {
            if (_currentLadder != null)
            {
                _controller.enabled = false;
                Vector3 climbPosition = transform.position;
                climbPosition.z = _currentLadder.transform.position.z - _currentLadder.GetPlayerOffset();
                transform.position = climbPosition;
                _climbingLadder = true;
                if (_startAtBottom == true)
                {
                    _anim.SetBool("ClimbUpLadder", true);
                }
                else
                {
                    _anim.SetBool("ClimbDownLadder", true);
                }
                _controller.enabled = true;

                if (CheckLadderFlip() == false)
                {
                    FlipPlayer(!_flip);
                    _flip = !_flip;
                }
            }
        }
    }

    private void FlipPlayer(bool flip)
    {
        if (flip == true)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180f, transform.rotation.z);
        }
        else
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0f, transform.rotation.z);
        }
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

    public void StopRoll()
    {
        _rolling = false;
        _controller.center = _colliderCenterStart;
        _controller.height = _colliderHeightStart;
    }

    public void EnterExitLadder( bool nearLadder, Ladder ladder)
    {
        if (nearLadder == true)
        {
            _nextToLadder = true;
            _currentLadder = ladder;
            if (ladder.transform.position.y > transform.position.y)
            {
                //Player at Bottom
                _startAtBottom = true;
            }
            else
            {
                //Player at Top
                _startAtBottom = false;
            }
        }
        else
        {
            if (_currentLadder == ladder)
            {
                _nextToLadder = false;
                _currentLadder = null;
            }
        }
    }

    public bool StartedAtLadderBottom()
    {
        return _startAtBottom;
    }

    public bool CheckClimbingLadder()
    {
        return _climbingLadder;
    }

    public void EndLadderClimb()
    {
        if (_currentLadder != null)
        {
            _climbingLadder = false;
            if (_startAtBottom == true)
            {
                _anim.SetBool("ClimbUpLadder", false);
                StartCoroutine(LadderEndRoutine(_startAtBottom, _currentLadder.GetTopEndPosition()));
            }
            else
            {
                _anim.SetBool("ClimbDownLadder", false);
                StartCoroutine(LadderEndRoutine(_startAtBottom, _currentLadder.GetBottomEndPosition()));
            }
        }
    }

    private bool CheckLadderFlip()
    {
        if (transform.position.z > _currentLadder.transform.position.z)
        {
            if (_flip == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (_flip == true)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    IEnumerator LadderEndRoutine(bool goingUp, Vector3 endPosition)
    {
        if (goingUp == false)
        {
            if (CheckLadderFlip() == true)
            {
                FlipPlayer(!_flip);
                _flip = !_flip;
            }
        }

        _controller.enabled = false;
        float distance = Mathf.Infinity;
        while (distance > 0.1f)
        {
            distance = Vector3.Distance(transform.position, endPosition);
            transform.position = Vector3.MoveTowards(transform.position, endPosition, _speed * Time.deltaTime);
            yield return null;
        }
        _startAtBottom = !_startAtBottom;
        _controller.enabled = true;
    }
}
