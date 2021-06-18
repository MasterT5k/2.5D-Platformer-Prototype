using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField]
    private int _elevatorID = 0;
    [SerializeField]
    private float _speed = 3f;
    [SerializeField]
    private Transform _bottomPoint = null, _topPoint = null;
    private bool _movingDown = false;

    private void OnEnable()
    {
        ElevatorPanel.OnElevatorCall += OnCallElevator;
    }

    private void OnDisable()
    {
        ElevatorPanel.OnElevatorCall -= OnCallElevator;
    }

    void FixedUpdate()
    {
        if (_movingDown == true)
        {
            if (_bottomPoint != null)
            {
                if (transform.position != _bottomPoint.position)
                {
                    transform.position = Vector3.MoveTowards(transform.position, _bottomPoint.position, _speed * Time.deltaTime); 
                }
            }
        }
        else
        {
            if (_topPoint != null)
            {
                if (transform.position != _topPoint.position)
                {
                    transform.position = Vector3.MoveTowards(transform.position, _topPoint.position, _speed * Time.deltaTime);
                }
            }
        }
    }

    void OnCallElevator(int ID)
    {
        if (ID == _elevatorID)
        {
            _movingDown = !_movingDown;
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
