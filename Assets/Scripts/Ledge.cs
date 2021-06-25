using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ledge : MonoBehaviour
{
    [SerializeField]
    private Transform _grabPosition;
    [SerializeField]
    private Transform _finalPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "LedgeGrab")
        {
            PlayerController player = other.transform.GetComponentInParent<PlayerController>();
            if (player != null)
            {
                if (_grabPosition != null)
                {
                    player.GrabLedge(_grabPosition.position, this);
                }
            }
        }
    }

    public Vector3 GetFinalPosition()
    {
        Vector3 position = Vector3.zero;
        if (_finalPosition != null)
        {
            position = _finalPosition.position;
        }

        return position;
    }
}
