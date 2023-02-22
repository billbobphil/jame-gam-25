using System.Collections.Generic;
using Ingredients;
using Player;
using UnityEngine;

namespace Utilities
{
    public class CollectionModifierController : MonoBehaviour
    {
        public GameObject claw;
        public GameManager gameManager;
        public GameObject clawHandPrefab;

        private float _startingClawSize;
        private float _startingClawHandSpeed;
        private List<double> _startingOrbitSpeeds;
        public float currentClawHandSpeed;
        
        public enum IngredientCollectionModifiers
        {
            SizeUp,
            SizeDown,
            SpeedUp,
            SpeedDown,
            OrbitSpeedDown,
            OrbitSpeedUp
        };

        public void Start()
        {
            _startingClawSize = claw.transform.localScale.x;
            _startingClawHandSpeed = clawHandPrefab.GetComponent<ClawHandMovementController>().baseSpeed;
            currentClawHandSpeed = _startingClawHandSpeed;
        }

        public void ConfigureOrbitStartUpSpeeds()
        {
            _startingOrbitSpeeds = new List<double>();

            foreach ((bool isCollected, GameObject ingredient, IngredientCollectionModifiers modifier) item in gameManager.Ingredients)
            {
                _startingOrbitSpeeds.Add(item.ingredient.GetComponent<IngredientMovementController>().rotator.degreesToMove);
            }
        }

        public void ApplyModifier(IngredientCollectionModifiers modifier)
        {
            Reset();
            switch (modifier)
            {
                case IngredientCollectionModifiers.SizeUp:
                    claw.transform.localScale = new Vector3(claw.transform.localScale.x * 2f, claw.transform.localScale.y * 2f, claw.transform.localScale.z);
                    break;
                case IngredientCollectionModifiers.SizeDown:
                    claw.transform.localScale = new Vector3(claw.transform.localScale.x * 0.65f, claw.transform.localScale.y * 0.65f, claw.transform.localScale.z);
                    break;
                case IngredientCollectionModifiers.SpeedUp:
                    currentClawHandSpeed = _startingClawHandSpeed * 1.5f;
                    break;
                case IngredientCollectionModifiers.SpeedDown:
                    currentClawHandSpeed = _startingClawHandSpeed * .5f;
                    break;
                case IngredientCollectionModifiers.OrbitSpeedUp:
                    foreach ((bool isCollected, GameObject ingredient, IngredientCollectionModifiers modifier) item in gameManager.Ingredients)
                    {
                        item.ingredient.GetComponent<IngredientMovementController>().rotator.degreesToMove += 1;
                    }
                    break;
                case IngredientCollectionModifiers.OrbitSpeedDown:
                    foreach ((bool isCollected, GameObject ingredient, IngredientCollectionModifiers modifier) item in gameManager.Ingredients)
                    {
                        item.ingredient.GetComponent<IngredientMovementController>().rotator.degreesToMove -= 1;
                    }
                    break;
            }
        }

        private void Reset()
        {
            claw.transform.localScale = new Vector3(_startingClawSize, _startingClawSize, claw.transform.localScale.z);
            currentClawHandSpeed = _startingClawHandSpeed;
            
            for(int i = 0; i < gameManager.Ingredients.Count; i++)
            {
                gameManager.Ingredients[i].ingredient.GetComponent<IngredientMovementController>().rotator.degreesToMove = _startingOrbitSpeeds[i];
            }
        }
    }
}
