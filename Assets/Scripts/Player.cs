using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KWP
{
    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private int _lives = 3;
        [SerializeField]
        private float _speed = 5f;
        [SerializeField]
        private float _gravity = 20f;
        [SerializeField]
        private float _jumpForce = 15f;
        [SerializeField]
        private float _yVelocity;
        private bool _canDoubleJump = false;
        private int _coinsCollected;

        private CharacterController _controller = null;
        private UIManager _uIManager = null;

        // Start is called before the first frame update
        void Start()
        {
            _controller = GetComponent<CharacterController>();

            _uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
            if (_uIManager == null)
            {
                Debug.LogError("UI Manager is Null");
            }
            else
            {
                _uIManager.UpdateCoins(_coinsCollected);
                _uIManager.UpdateLives(_lives);
            }
        }

        void Update()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            Vector3 direction = new Vector3(horizontalInput, 0);
            Vector3 velocity = direction * _speed;
            if (_controller.isGrounded == true)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _yVelocity = _jumpForce;
                    _canDoubleJump = true;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (_canDoubleJump == true)
                    {
                        _yVelocity += _jumpForce;
                        _canDoubleJump = false;
                    }
                }
                _yVelocity -= _gravity;
            }

            velocity.y = _yVelocity;
            if (_controller.enabled == true)
            {
                _controller.Move(velocity * Time.deltaTime);
            }
        }

        public void AddCoin()
        {
            _coinsCollected++;

            if (_uIManager != null)
            {
                _uIManager.UpdateCoins(_coinsCollected);
            }
        }

        public void LoseLife()
        {
            _lives--;
            if (_lives <= 0)
            {
                SceneManager.LoadScene(0);
            }

            if (_uIManager != null)
            {
                _uIManager.UpdateLives(_lives);
            }

            StartCoroutine(ReactivateController());
        }

        IEnumerator ReactivateController()
        {
            _yVelocity = 0;
            yield return new WaitForEndOfFrame();
            _controller.enabled = true;
        }
    } 
}
