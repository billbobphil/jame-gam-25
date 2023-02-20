using System;
using UnityEngine;

namespace Player
{
    public class Shooter : MonoBehaviour
    {
        public void FixedUpdate()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Debug.Log("Spacebar pressed");
            }
        }

        private void ShootClaw()
        {
            
        }
    }
}
