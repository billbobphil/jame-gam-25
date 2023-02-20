using System;
using UnityEngine;

namespace Player
{
    public class Rotator : MonoBehaviour
    {
        //Script requires a parent-child relationship (where the child "orbits" the parent)
        private enum Direction
        {
            Clockwise = 0,
            CounterClockwise = 1
        }
        
        public GameObject pointer;
        public double distanceBetweenPointerAndPlayer = 3;
        public int degreesToMove = 5;
        public bool shouldRotateToCompensate = true;
        private float _rotatorStartingRotation = 0;

        private void Start()
        {
            pointer.transform.localPosition = new Vector3(0, (float)distanceBetweenPointerAndPlayer, 0);

            if (shouldRotateToCompensate)
            {
                _rotatorStartingRotation = pointer.transform.eulerAngles.z;
            }
        }
        
        //TODO: this could be moved out to a separate movement script and this script modified to return the rotation values so it could be used
        //by non-player controlled options.
        private void FixedUpdate()
        {
            if (Input.GetKey(KeyCode.D))
            {
                MovePointerInOrbit(Direction.Clockwise);
            }

            if (Input.GetKey(KeyCode.A))
            {
                MovePointerInOrbit(Direction.CounterClockwise);
            }
        }

        private void MovePointerInOrbit(Direction direction)
        {
            double currentAngleInRadians = GetCurrentAngleInRadians();

            int degreesToMoveWithDirectionality = direction == Direction.Clockwise ? degreesToMove : -degreesToMove;
            double radiansToMove = degreesToMoveWithDirectionality * (Math.PI / 180);
            double newAngleInRadians = currentAngleInRadians + radiansToMove;
            
            double sinValue = Math.Sin(newAngleInRadians);
            double xValue = sinValue * distanceBetweenPointerAndPlayer;

            double cosValue = Math.Cos(newAngleInRadians);
            double yValue = cosValue * distanceBetweenPointerAndPlayer;
            
            pointer.transform.localPosition = new Vector3((float)xValue, (float)yValue, 0);
            
            if (shouldRotateToCompensate)
            {
                double newAngleInDegrees = newAngleInRadians * (180 / Math.PI);
                pointer.transform.eulerAngles = new Vector3(0, 0, (float)-(newAngleInDegrees + _rotatorStartingRotation));
            }
        }

        private double GetCurrentAngleInRadians()
        {
            Vector3 localPosition = pointer.transform.localPosition;
            return Math.Atan2(localPosition.x, localPosition.y);
        }
    }
}
