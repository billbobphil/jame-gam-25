using System;
using Ingredients;
using UnityEngine;

namespace Player
{
    public class ClawHandMovementController : MonoBehaviour
    {
        public ClawController manager;
        private float _xDirection;
        private float _yDirection;
        public float speed = 1;
        private bool _isPaused = false;
        
        private void OnEnable()
        {
            IngredientMiniGameEventNotifier.OnStartMiniGame += Pause;
        }

        private void OnDisable()
        {
            IngredientMiniGameEventNotifier.OnStartMiniGame -= Pause;

        }
        
        private void Pause()
        {
            _isPaused = true;
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
            
            Vector3 position = transform.position;
            transform.position = new Vector3(position.x + (_xDirection * speed), position.y  + (_yDirection * speed), position.z);
        }
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Ingredient"))
            {
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
