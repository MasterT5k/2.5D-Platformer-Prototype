using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftPlatform : MovingPlatform
{
    [SerializeField]
    private int _startingFloor = 0;
    [SerializeField]
    private float _floorDelay = 5f;
    private bool _isPausing = false;

    private void Start()
    {
        if (_waypoints.Length > 0)
        {
            Transform start = _waypoints[_startingFloor];
            if (start != null)
            {
                transform.position = start.position;
                _currentWaypoint = _startingFloor;
            } 
        }
    }

    protected override void CheckPosition()
    {
        base.CheckPosition();
        if (_atWaypoint == true)
        {
            if (_isPausing == false)
            {
                StartCoroutine(PauseOnFloorRoutine());
            }
        }
    }

    protected override void CalculateMovement()
    {
        if (_isPausing == false)
        {
            base.CalculateMovement();
        }
    }

    IEnumerator PauseOnFloorRoutine()
    {
        _isPausing = true;
        yield return new WaitForSeconds(_floorDelay);
        _isPausing = false;
    }
}
