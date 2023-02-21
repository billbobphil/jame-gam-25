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
        public CollectionModifierController collectionModifierController;

        private void OnEnable()
        {
            IngredientMiniGameEventNotifier.OnStartMiniGame += Pause;
            MiniGameController.OnEndMiniGame += Resume;
        }

        private void OnDisable()
        {
            IngredientMiniGameEventNotifier.OnStartMiniGame -= Pause;
            MiniGameController.OnEndMiniGame -= Resume;
        }

        private void Update()
        {
            if (_isPaused) return;
            
            if (_canCreateClaw && Input.GetKeyDown(KeyCode.Space))
            {
                allowRotatorMovement = false;
                _canCreateClaw = false;
                GameObject clawHand = Instantiate(clawHandPrefab, claw.transform.position, claw.transform.rotation);
                clawHand.transform.localScale = claw.transform.localScale * 1.5f;
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

        private void Pause(GameObject ingredient)
        {
            _isPaused = true;
        }

        private void Resume(GameObject ingredient)
        {
            _isPaused = false;
        }
    }
}
