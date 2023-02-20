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

        public List<GameObject> ingredientPrefabs;
        private List<(bool isCollected, GameObject ingredient)> _ingredients;

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
            
            if (_ingredients.All(item => item.isCollected))
            {
                Debug.Log("Trigger Win State All Ingredients Used");
                _hasWonGame = true;
            }
        }

        private void WinMiniGame(GameObject ingredient)
        {
            int i = _ingredients.IndexOf((false, ingredient));
            _ingredients[i].ingredient.GetComponent<IngredientController>().CollectIngredient();
            _ingredients[i] = (true, ingredient);

            foreach ((bool isCollected, GameObject ingredient) item in _ingredients)
            {
                Rotator rotator = item.ingredient.GetComponent<Rotator>();
                rotator.degreesToMove += .33f;
            }
        }

        private void SpawnIngredients()
        {
            _ingredients = new List<(bool, GameObject)>();

            List<int> orbitDistanceCopy = _orbitDistances.ToList();
            List<float> orbitSpeedCopy = _orbitSpeeds.ToList();

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
                
                ingredientMovementController.rotator.degreesToMove = orbitSpeedCopy[randomSpeedIndex];
                ingredientMovementController.rotator.distanceBetweenOrbiterAndOrbitee = orbitDistanceCopy[randomOrbitDistanceIndex];
                ingredientMovementController.direction = randomOrbitDirection == 0 ? Rotator.Direction.Clockwise : Rotator.Direction.CounterClockwise;
                _ingredients.Add((false, ingredient));
                
                orbitDistanceCopy.RemoveAt(randomOrbitDistanceIndex);
                orbitSpeedCopy.RemoveAt(randomSpeedIndex);
            }
        }
        
        public int GetNumberOfIngredientsCollected()
        {
            return _ingredients.Count(item => item.isCollected);
        }
    }
}
