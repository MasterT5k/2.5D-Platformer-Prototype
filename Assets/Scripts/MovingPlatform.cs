using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KWP
{
    public class MovingPlatform : MonoBehaviour
    {
        [SerializeField]
        private Transform _pointA = null, _pointB = null;
        [SerializeField]
        private float _speed = 5f;
        private bool _movingToB = false;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (_movingToB == false)
            {
                transform.position = Vector3.MoveTowards(transform.position, _pointA.position, _speed * Time.deltaTime);

                if (transform.position == _pointA.position)
                {
                    _movingToB = true;
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, _pointB.position, _speed * Time.deltaTime);

                if (transform.position == _pointB.position)
                {
                    _movingToB = false;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                other.transform.SetParent(this.transform);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                other.transform.SetParent(null);
            }
        }
    } 
}
