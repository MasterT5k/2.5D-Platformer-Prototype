using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    protected Transform[] _waypoints;
    protected int _currentWaypoint;
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private bool _reverseAtEnd = false;
    private bool _inReverse = false;
    protected bool _atWaypoint = false;

    void FixedUpdate()
    {
        if (_waypoints.Length < 2)
        {
            return;
        }

        CheckPosition();

        GetNextWaypoint();

        CalculateMovement();
    }

    protected virtual void CalculateMovement()
    {
        Vector3 pointPosition = _waypoints[_currentWaypoint].position;
        float adjustedSpeed = _speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, pointPosition, adjustedSpeed);
    }

    protected virtual void CheckPosition()
    {
        if (transform.position == _waypoints[_currentWaypoint].position)
        {
            _atWaypoint = true;
        }
    }

    void GetNextWaypoint()
    {
        if (_atWaypoint == true)
        {
            if (_currentWaypoint == _waypoints.Length - 1)
            {
                if (_reverseAtEnd == true)
                {
                    _inReverse = true;
                    _currentWaypoint--;
                }
                else
                {
                    _currentWaypoint = 0;
                }

                _atWaypoint = false;
            }
            else
            {
                if (_inReverse == true)
                {
                    _currentWaypoint--;
                    if (_currentWaypoint <= 0)
                    {
                        _currentWaypoint = 0;
                        _inReverse = false;
                    }
                }
                else
                {
                    _currentWaypoint++;
                    if (_currentWaypoint >= _waypoints.Length)
                    {
                        _currentWaypoint = _waypoints.Length - 1;
                    }
                }

                _atWaypoint = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.parent = this.transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.parent = null;
        }
    }
}
