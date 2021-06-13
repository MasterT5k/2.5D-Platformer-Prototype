using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KWP
{
    public class DeathZone : MonoBehaviour
    {
        [SerializeField]
        private GameObject _respawnPoint = null;
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                Player player = other.GetComponent<Player>();
                if (player != null)
                {
                    player.LoseLife();
                }

                CharacterController controller = other.GetComponent<CharacterController>();
                if (controller != null)
                {
                    controller.enabled = false;
                }

                other.transform.position = _respawnPoint.transform.position;
            }
        }
    } 
}
