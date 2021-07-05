using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    [SerializeField]
    private float _playerOffset = -1f;
    [SerializeField]
    private Transform _bottomEndPos, _topEndPos;

    public float GetPlayerOffset()
    {
        return _playerOffset;
    }

    public Vector3 GetTopEndPosition()
    {
        return _topEndPos.position;
    }

    public Vector3 GetBottomEndPosition()
    {
        return _bottomEndPos.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.EnterExitLadder(true, this);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.EnterExitLadder(false, this);
            }
        }

        if (other.tag == "LedgeGrab")
        {
            PlayerController player = other.GetComponentInParent<PlayerController>();
            if (player != null)
            {
                if (player.CheckClimbingLadder() == true)
                {
                    if (player.StartedAtLadderBottom() == true)
                    {
                        player.EndLadderClimb();
                    } 
                }
            }
        }
    }
}
