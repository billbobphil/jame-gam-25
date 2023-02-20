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
            MiniGameController.OnEndMiniGame += Resume;
        }

        private void OnDisable()
        {
            IngredientMiniGameEventNotifier.OnStartMiniGame -= Pause;
            MiniGameController.OnEndMiniGame -= Resume;
        }
        
        private void Pause(GameObject ingredient)
        {
            _isPaused = true;
        }

        private void Resume(GameObject ingredient)
        {
            _isPaused = false;
        }
        
        private void FixedUpdate()
        {
            if(_isPaused) return;
            
            rotator.MoveOrbiteeInOrbit(direction);
        }
    }
}
