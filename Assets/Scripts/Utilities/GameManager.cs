using System;
using System.Collections;
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
        private int _lastMinutesCollected;
        private int _lastSecondsCollected;
        
        public delegate void GameOver();
        public static event GameOver OnGameOver;
        

        private readonly List<int> _orbitDistances = new()
        {
            3, 5, 7, 9, 11, 13
        };

        private readonly List<float> _orbitSpeeds = new()
        {
            1, 1.5f, 2, 2.5f, 3, 3.5f
        };

        private void Awake()
        {
            timer.startTime = Difficulty.GetDifficultyLevel() switch
            {
                Difficulty.DifficultyLevel.Easy => 60,
                Difficulty.DifficultyLevel.Normal => 45,
                Difficulty.DifficultyLevel.Hard => 30,
                _ => timer.startTime
            };
        }
        
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
            
            _lastMinutesCollected = minutesRemaining;
            _lastSecondsCollected = secondsRemaining;

            if (minutesRemaining == 0 && secondsRemaining == 0)
            {   
                timer.PauseTimer();
                uiRegistration.gameCanvas.SetActive(false);
                uiRegistration.miniGameCanvas.SetActive(false);
                uiRegistration.gameEndCanvas.SetActive(true);
                uiRegistration.defeatPanel.SetActive(true);
                _hasLostGame = true;
                OnGameOver?.Invoke();
            }
        }

        private void LateUpdate()
        {
            if (_hasWonGame || _hasLostGame) return;
            
            if (Ingredients.All(item => item.isCollected))
            {
                timer.PauseTimer();
                uiRegistration.gameCanvas.SetActive(false);
                uiRegistration.miniGameCanvas.SetActive(false);
                uiRegistration.gameEndCanvas.SetActive(true);
                uiRegistration.victoryPanel.SetActive(true);
                _hasWonGame = true;
                uiRegistration.endGameVictoryTimeRemainingText.text = $"{_lastMinutesCollected:00}:{_lastSecondsCollected:00}";
                OnGameOver?.Invoke();
            }
        }

        private void WinMiniGame(GameObject ingredient)
        {
            int i = Ingredients.FindIndex(item => item.ingredient == ingredient);
            Ingredients[i].ingredient.GetComponent<IngredientController>().CollectIngredient();
            collectionModifierController.ApplyModifier(Ingredients[i].modifier);
            Ingredients[i] = (true, ingredient, Ingredients[i].modifier);
            StartCoroutine(UpdateTextForGatheredIngredients(Ingredients[i].ingredient.GetComponent<IngredientController>().ingredientType, Ingredients[i].modifier));
        }

        private IEnumerator UpdateTextForGatheredIngredients(IngredientController.IngredientType ingredientType, CollectionModifierController.IngredientCollectionModifiers modifier)
        {
            string ingredientTypeText = "";
            
            switch (ingredientType)
            {
                case IngredientController.IngredientType.Chicken:
                    ingredientTypeText = "Chicken collected!";
                    uiRegistration.collectedChickenText.text = "1";
                    break;
                case IngredientController.IngredientType.Butter:
                    ingredientTypeText = "Butter collected!";
                    uiRegistration.collectedButterText.text = "1";
                    break;
                case IngredientController.IngredientType.Cream:
                    ingredientTypeText = "Cream collected!";
                    uiRegistration.collectedCreamText.text = "1";
                    break;
                case IngredientController.IngredientType.Garlic:
                    ingredientTypeText = "Garlic collected!";
                    uiRegistration.collectedGarlicText.text = "1";
                    break;
                case IngredientController.IngredientType.Onion:
                    ingredientTypeText = "Onion collected!";
                    uiRegistration.collectedOnionText.text = "1";
                    break;
                case IngredientController.IngredientType.Tomato:
                    ingredientTypeText = "Tomato collected!";
                    uiRegistration.collectedTomatoText.text = "1";
                    break;
            };

            string modifierText = modifier switch
            {
                CollectionModifierController.IngredientCollectionModifiers.SizeUp => "Claw size up!",
                CollectionModifierController.IngredientCollectionModifiers.SizeDown => "Claw size down!",
                CollectionModifierController.IngredientCollectionModifiers.SpeedUp => "Claw speed up!",
                CollectionModifierController.IngredientCollectionModifiers.SpeedDown => "Claw speed down!",
                CollectionModifierController.IngredientCollectionModifiers.OrbitSpeedDown => "Ingredient orbit speed down!",
                CollectionModifierController.IngredientCollectionModifiers.OrbitSpeedUp => "Ingredient orbit speed up!",
                _ => ""
            };

            uiRegistration.modifierText.text = modifierText;
            uiRegistration.collectedIngredientText.text = ingredientTypeText;
            uiRegistration.collectionAndModifierPanel.SetActive(true);
            yield return new WaitForSecondsRealtime(3);
            uiRegistration.collectionAndModifierPanel.SetActive(false);
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
