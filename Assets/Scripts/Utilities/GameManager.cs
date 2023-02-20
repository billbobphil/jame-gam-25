using System;
using System.Collections.Generic;
using System.Linq;
using Ingredients;
using TMPro;
using UnityEngine;

namespace Utilities
{
    public class GameManager : MonoBehaviour
    {
        public UiRegistration uiRegistration;
        public Timer timer;
        public CollectionModifierController collectionModifierController;

        public List<GameObject> ingredientPrefabs;
        public List<(bool isCollected, GameObject ingredient, CollectionModifierController.IngredientCollectionModifiers modifier)> Ingredients;

        private bool _hasWonGame;
        private bool _hasLostGame;
        

        private readonly List<int> _orbitDistances = new()
        {
            3, 5, 7, 9, 11, 13
        };

        private readonly List<float> _orbitSpeeds = new()
        {
            1, 1.5f, 2, 2.5f, 3, 3.5f
        };

        private void Start()
        {
            timer.StartTimer();
            SpawnIngredients();
        }
        
        private void OnEnable()
        {
            Timer.OnTimerUpdate += UpdateTimer;
            MiniGameController.OnEndMiniGame += WinMiniGame;
        }

        private void OnDisable()
        {
            Timer.OnTimerUpdate -= UpdateTimer;
            MiniGameController.OnEndMiniGame -= WinMiniGame;
        }
        
        private void UpdateTimer(int minutesRemaining, int secondsRemaining)
        {
            if (_hasLostGame || _hasWonGame) return;
            
            uiRegistration.timerText.GetComponent<TextMeshProUGUI>().text = $"{minutesRemaining:00}:{secondsRemaining:00}";

            if (minutesRemaining == 0 && secondsRemaining == 0)
            {   
                Debug.Log("Trigger Game Over State");
                _hasLostGame = true;
            }
        }

        private void LateUpdate()
        {
            if (_hasWonGame || _hasLostGame) return;
            
            if (Ingredients.All(item => item.isCollected))
            {
                Debug.Log("Trigger Win State All Ingredients Used");
                _hasWonGame = true;
            }
        }

        private void WinMiniGame(GameObject ingredient)
        {
            int i = Ingredients.FindIndex(item => item.ingredient == ingredient);
            Ingredients[i].ingredient.GetComponent<IngredientController>().CollectIngredient();
            collectionModifierController.ApplyModifier(Ingredients[i].modifier);
            Ingredients[i] = (true, ingredient, Ingredients[i].modifier);
        }

        private void SpawnIngredients()
        {
            Ingredients = new List<(bool, GameObject, CollectionModifierController.IngredientCollectionModifiers)>();

            List<int> orbitDistanceCopy = _orbitDistances.ToList();
            List<float> orbitSpeedCopy = _orbitSpeeds.ToList();
            List<CollectionModifierController.IngredientCollectionModifiers> availableCollectionModifiers = new()
            {
                CollectionModifierController.IngredientCollectionModifiers.SizeUp,
                CollectionModifierController.IngredientCollectionModifiers.SizeDown,
                CollectionModifierController.IngredientCollectionModifiers.SpeedUp,
                CollectionModifierController.IngredientCollectionModifiers.SpeedDown,
                CollectionModifierController.IngredientCollectionModifiers.OrbitSpeedDown,
                CollectionModifierController.IngredientCollectionModifiers.OrbitSpeedUp
            };

            foreach (GameObject prefab in ingredientPrefabs)
            {
                //Arbitrary range
                int randomX = UnityEngine.Random.Range(-10, 10);
                int randomY = UnityEngine.Random.Range(-10, 10);
                
                GameObject ingredient = Instantiate(prefab, new Vector3(randomX, randomY, 0), new Quaternion(0, 0, 0, 0));
                IngredientMovementController ingredientMovementController = ingredient.GetComponent<IngredientMovementController>();
                
                int randomOrbitDistanceIndex = UnityEngine.Random.Range(0, orbitDistanceCopy.Count);
                int randomSpeedIndex = UnityEngine.Random.Range(0, orbitSpeedCopy.Count);
                int randomOrbitDirection = UnityEngine.Random.Range(0, 2);
                int randomCollectionModifierIndex = UnityEngine.Random.Range(0, availableCollectionModifiers.Count);
                
                ingredientMovementController.rotator.degreesToMove = orbitSpeedCopy[randomSpeedIndex];
                ingredientMovementController.rotator.distanceBetweenOrbiterAndOrbitee = orbitDistanceCopy[randomOrbitDistanceIndex];
                ingredientMovementController.direction = randomOrbitDirection == 0 ? Rotator.Direction.Clockwise : Rotator.Direction.CounterClockwise;
                Ingredients.Add((false, ingredient, availableCollectionModifiers[randomCollectionModifierIndex]));
                
                orbitDistanceCopy.RemoveAt(randomOrbitDistanceIndex);
                orbitSpeedCopy.RemoveAt(randomSpeedIndex);
                availableCollectionModifiers.RemoveAt(randomCollectionModifierIndex);
                collectionModifierController.ConfigureOrbitStartUpSpeeds();
            }
        }
        
        public int GetNumberOfIngredientsCollected()
        {
            return Ingredients.Count(item => item.isCollected);
        }
    }
}
