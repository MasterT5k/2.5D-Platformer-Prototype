using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePad : MonoBehaviour
{
    [SerializeField]
    private float _offset = 0.1f;
    [SerializeField]
    private Color _activatedColor = Color.blue;
    private bool _triggered = false;
    private MeshRenderer _renderer = null;

    private void Start()
    {
        _renderer = GetComponentInChildren<MeshRenderer>();
        if (_renderer == null)
        {
            Debug.LogError("MeshRenderer is Null");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_triggered == false)
        {
            if (other.tag == "Moveable")
            {
                Rigidbody body = other.GetComponent<Rigidbody>();
                if (body != null)
                {
                    float distance = Vector3.Distance(other.transform.position, transform.position);
                    if (distance <= _offset)
                    {
                        body.isKinematic = true;
                        _triggered = true;
                        if (_renderer != null)
                        {
                            _renderer.material.color = _activatedColor;
                        }
                    }
                }
            } 
        }
    }
}
