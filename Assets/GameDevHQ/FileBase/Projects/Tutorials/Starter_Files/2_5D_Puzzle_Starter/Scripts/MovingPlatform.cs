using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private Transform[] _waypoints;
    private int _currentWaypoint;
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private bool _reverseAtEnd = false;
    private bool _inReverse = false;
    private bool _atWaypoint = false;

    void FixedUpdate()
    {
        if (_waypoints.Length < 2)
        {
            return;
        }

        if (transform.position == _waypoints[_currentWaypoint].position)
        {
            _atWaypoint = true;
        }

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
        else
        {
            Vector3 pointPosition = _waypoints[_currentWaypoint].position;
            float adjustedSpeed = _speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, pointPosition, adjustedSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.parent = this.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.parent = null;
        }
    }
}
