using System;
using Ingredients;
using UnityEngine;
using Utilities;

namespace Player
{
    public class ClawController : MonoBehaviour
    {
        public GameObject claw;
        public Rotator rotator;
        public bool allowRotatorMovement = true;
        public GameObject clawHandPrefab;
        private bool _canCreateClaw = true;
        private bool _isPaused = false;

        private void OnEnable()
        {
            IngredientMiniGameEventNotifier.OnStartMiniGame += Pause;
        }

        private void OnDisable()
        {
            IngredientMiniGameEventNotifier.OnStartMiniGame -= Pause;

        }

        private void Update()
        {
            if (_isPaused) return;
            
            if (_canCreateClaw && Input.GetKeyDown(KeyCode.Space))
            {
                allowRotatorMovement = false;
                _canCreateClaw = false;
                GameObject clawHand = Instantiate(clawHandPrefab, claw.transform.position, claw.transform.rotation);
                clawHand.GetComponent<ClawHandMovementController>().manager = this;
            }
        }
        
        private void FixedUpdate()
        {
            if (_isPaused || !allowRotatorMovement) return;

            if (Input.GetKey(KeyCode.D))
            {
                rotator.MoveOrbiteeInOrbit(Rotator.Direction.Clockwise);
            }
   
            if (Input.GetKey(KeyCode.A))
            {
                rotator.MoveOrbiteeInOrbit(Rotator.Direction.CounterClockwise);
            }
        }

        public void OnClawHandDestroyed()
        {   
            _canCreateClaw = true;
            allowRotatorMovement = true;
        }

        private void Pause()
        {
            _isPaused = true;
        } 
    }
}
