using UnityEngine;
using Utilities;

namespace Player
{
    public class ClawMovementController : MonoBehaviour
    {
        public Rotator rotator;

        private void FixedUpdate()
        {
            if (Input.GetKey(KeyCode.D))
            {
                rotator.MoveOrbiteeInOrbit(Rotator.Direction.Clockwise);
            }
   
            if (Input.GetKey(KeyCode.A))
            {
                rotator.MoveOrbiteeInOrbit(Rotator.Direction.CounterClockwise);
            }
        }
    }
}
