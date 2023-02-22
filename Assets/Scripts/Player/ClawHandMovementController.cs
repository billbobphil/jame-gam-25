using System;
using Ingredients;
using UnityEngine;
using Utilities;

namespace Player
{
    public class ClawHandMovementController : MonoBehaviour
    {
        public ClawController manager;
        private float _xDirection;
        private float _yDirection;
        public float baseSpeed = 1;
        private bool _isPaused = false;

        private void OnEnable()
        {
            IngredientMiniGameEventNotifier.OnStartMiniGame += Pause;
            MiniGameController.OnEndMiniGame += Resume;
            GameManager.OnGameOver += EndGame;
        }

        private void OnDisable()
        {
            IngredientMiniGameEventNotifier.OnStartMiniGame -= Pause;
            MiniGameController.OnEndMiniGame -= Resume;
            GameManager.OnGameOver -= EndGame;
        }
        
        private void Pause(GameObject ingredient)
        {
            _isPaused = true;
        }

        private void EndGame()
        {
            _isPaused = true;
        }
        
        private void Resume(GameObject ingredient)
        {
            _isPaused = false;
        }
        
        private void Start()
        {
            Vector3 position = transform.position;
            _xDirection = position.x;
            _yDirection = position.y;
        }
        
        private void FixedUpdate()
        {
            if(_isPaused) return;

            float modifiedSpeed = manager.collectionModifierController.currentClawHandSpeed;
            
            Vector3 position = transform.position;
            transform.position = new Vector3(position.x + (_xDirection * modifiedSpeed), position.y  + (_yDirection * modifiedSpeed), position.z);
        }
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Ingredient"))
            {
                manager.gameManager.soundEffectPlayer.PlayClip(manager.gameManager.soundEffectRegistration.clawHitSoundEffect);
                DestroyClawHand();
            }
        }

        private void DestroyClawHand()
        {
            manager.OnClawHandDestroyed();
            Destroy(gameObject);
        }
        
        private void OnBecameInvisible()
        {
            DestroyClawHand();
        }
    }
}
