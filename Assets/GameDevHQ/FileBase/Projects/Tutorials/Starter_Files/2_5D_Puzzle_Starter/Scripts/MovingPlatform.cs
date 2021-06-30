using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private Transform _targetA, _targetB;
    [SerializeField]
    private Transform[] _waypoints;
    [SerializeField]
    private int _currentWaypoint;
    [SerializeField]
    private float _speed = 3.0f;
    private bool _movingToPoint = false;

    void FixedUpdate()
    {
        if (transform.position == _waypoints[_currentWaypoint].position)
        {

        }
        if (_movingToPoint == false)
        {

        }




        //if (_movingToPoint == false)
        //{
        //    transform.position = Vector3.MoveTowards(transform.position, _targetA.position, _speed * Time.deltaTime);
        //}
        //else if (_movingToPoint == true)
        //{
        //    transform.position = Vector3.MoveTowards(transform.position, _targetB.position, _speed * Time.deltaTime);
        //}

        //if (transform.position == _targetB.position)
        //{
        //    _movingToPoint = false;
        //}
        //else if (transform.position == _targetA.position)
        //{
        //    _movingToPoint = true;
        //}
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
