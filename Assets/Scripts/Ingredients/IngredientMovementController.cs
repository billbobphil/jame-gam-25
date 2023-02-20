using UnityEngine;
using Utilities;

namespace Ingredients
{
    public class IngredientMovementController : MonoBehaviour
    {
        public Rotator.Direction direction;
        public Rotator rotator;
        
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
        
        private void FixedUpdate()
        {
            if(_isPaused) return;
            
            rotator.MoveOrbiteeInOrbit(direction);
        }
    }
}
