using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorPanel : MonoBehaviour
{
    [SerializeField]
    private int _requiredCoins = 8;
    [SerializeField]
    private MeshRenderer _callButton = null;
    [SerializeField]
    private Color _activatedColor = Color.green;
    private Color _deactivatedColor;
    [SerializeField]
    private int _elevatorID = 0;

    private bool _elevatorCalled = false;

    public static event Action<int> OnElevatorCall;

    private void Start()
    {
        if (_callButton != null)
        {
            _deactivatedColor = _callButton.material.color;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {                
                Player player = other.GetComponent<Player>();
                if (player != null)
                {
                    int coinsCollected = player.CheckCoins();
                    if (coinsCollected >= _requiredCoins)
                    {
                        if (_elevatorCalled == false)
                        {
                            if (_callButton != null)
                            {
                                _callButton.material.color = _activatedColor;
                            }
                            _elevatorCalled = true;
                        }
                        else
                        {
                            if (_callButton != null)
                            {
                                _callButton.material.color = _deactivatedColor;
                            }
                            _elevatorCalled = false;
                        }

                        OnElevatorCall?.Invoke(_elevatorID);
                    }
                }
            }
        }
    }
}
