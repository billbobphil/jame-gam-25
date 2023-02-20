using UnityEngine;
using Utilities;

namespace Ingredients
{
    public class IngredientMovementController : MonoBehaviour
    {
        public Rotator.Direction direction;
        public Rotator rotator;
        
        private void FixedUpdate()
        {
            rotator.MoveOrbiteeInOrbit(direction);
        }
    }
}
