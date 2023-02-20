using System;
using UnityEngine;

namespace Utilities
{
    public class Rotator : MonoBehaviour
    {
        public enum Direction
        {
            Clockwise = 0,
            CounterClockwise = 1
        }

        public GameObject orbitee;
        public GameObject orbiter;
        public double distanceBetweenOrbiterAndOrbitee = 3;
        public double degreesToMove = 5;
        public bool shouldRotateToCompensate = true;
        private float _rotatorStartingRotation = 0;
        public bool isParentChildRelationship = false;
        public bool shouldStartAtCenter = false;

        private void Start()
        {
            if (shouldStartAtCenter)
            {
                SetDistanceBetweenOrbiterAndOrbiteeAndCenterOnX();
            }
            else
            {
                SetDistanceBetweenOrbiterAndOrbiteeAndRetainAngle();
            }
            
            if (shouldRotateToCompensate)
            {
                _rotatorStartingRotation = orbiter.transform.eulerAngles.z;
            }
        }
        
        public void MoveOrbiteeInOrbit(Direction direction)
        {
            double currentAngleInRadians = GetCurrentAngleInRadians();
            
            double degreesToMoveWithDirectionality = direction == Direction.Clockwise ? degreesToMove : -degreesToMove;
            double radiansToMove = degreesToMoveWithDirectionality * (Math.PI / 180);
            double newAngleInRadians = currentAngleInRadians + radiansToMove;
            
            Vector3 orbiteePosition = orbitee.transform.position;
            double sinValue = Math.Sin(newAngleInRadians);
            double xValue = sinValue * distanceBetweenOrbiterAndOrbitee + orbiteePosition.x;

            double cosValue = Math.Cos(newAngleInRadians);
            double yValue = cosValue * distanceBetweenOrbiterAndOrbitee + orbiteePosition.y;

            if (isParentChildRelationship)
            {
                orbiter.transform.localPosition = new Vector3((float)xValue, (float)yValue, 0);
            }
            else
            {
                orbiter.transform.position = new Vector3((float)xValue, (float)yValue, 0);
            }
            
            if (shouldRotateToCompensate)
            {
                double newAngleInDegrees = newAngleInRadians * (180 / Math.PI);
                orbiter.transform.eulerAngles = new Vector3(0, 0, (float)-(newAngleInDegrees + _rotatorStartingRotation));
            }
        }

        private void SetDistanceBetweenOrbiterAndOrbiteeAndCenterOnX()
        {
            Vector3 orbiteePosition = orbitee.transform.position;

            if (isParentChildRelationship)
            {
                orbiter.transform.localPosition = new Vector3(orbiteePosition.x, (float)distanceBetweenOrbiterAndOrbitee, orbiteePosition.z);    
            }
            else
            {
                orbiter.transform.position = new Vector3(orbiteePosition.x, (float)distanceBetweenOrbiterAndOrbitee, orbiteePosition.z);
            }    
        }
        
        private void SetDistanceBetweenOrbiterAndOrbiteeAndRetainAngle()
        {
            double currentAngleInRadians = GetCurrentAngleInRadians();
            Vector3 orbiteePosition = orbitee.transform.position;
            double orbiteeX = Math.Sin(currentAngleInRadians) * distanceBetweenOrbiterAndOrbitee + orbiteePosition.x;
            double orbiteeY = Math.Cos(currentAngleInRadians) * distanceBetweenOrbiterAndOrbitee + orbiteePosition.y;
                
            if (isParentChildRelationship)
            {
                orbiter.transform.localPosition = new Vector3((float)orbiteeX, (float)orbiteeY, orbiteePosition.z);
            }
            else
            {
                orbiter.transform.position = new Vector3((float)orbiteeX, (float)orbiteeY, orbiteePosition.z);
            }
        }
        
        private double GetCurrentAngleInRadians()
        {
            Vector3 orbiterPosition = orbiter.transform.position;
            Vector3 orbiteePosition = orbitee.transform.position;
            
            double x = orbiterPosition.x - orbiteePosition.x;
            double y = orbiterPosition.y - orbiteePosition.y;

            return Math.Atan2(x, y);
        }
    }
}
