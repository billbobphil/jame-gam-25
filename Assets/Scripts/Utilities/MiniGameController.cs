using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ingredients;
using TMPro;
using UnityEngine;

namespace Utilities
{
    public class MiniGameController : MonoBehaviour
    {
        private enum ArrowDirections
        {
            Up,
            Down,
            Left,
            Right
        }

        private readonly Dictionary<ArrowDirections, KeyCode[]> _arrowKeyCodes = new()
        {
            {
                ArrowDirections.Up, new [] { KeyCode.UpArrow, KeyCode.W }
            },
            {
                ArrowDirections.Down, new [] { KeyCode.DownArrow, KeyCode.S }
            },
            {
                ArrowDirections.Left, new [] { KeyCode.LeftArrow, KeyCode.A }
            },
            {
                ArrowDirections.Right, new [] { KeyCode.RightArrow, KeyCode.D }
            }
        };

        public UiRegistration uiRegistration;
        public GameObject arrowPrefab;
        private List<(ArrowDirections direction, GameObject arrow)> _sequence;
        private bool _allowInput;
        private int _currentArrowIndex;
        private GameObject _ingredientBeingPlayedFor;
        public GameManager gameManager;
        public int baseArrowCount = 3;

        public delegate void EndMiniGame(GameObject ingredient);
        public static event EndMiniGame OnEndMiniGame;
        
        private void OnEnable()
        {
            IngredientMiniGameEventNotifier.OnStartMiniGame += StartMiniGame;
            GameManager.OnGameOver += PreventInput;
        }

        private void OnDisable()
        {
            IngredientMiniGameEventNotifier.OnStartMiniGame -= StartMiniGame;
            GameManager.OnGameOver -= PreventInput;
        }

        private void PreventInput()
        {
            _allowInput = false;
        }

        private void StartMiniGame(GameObject ingredient)
        {
            _ingredientBeingPlayedFor = ingredient;
            uiRegistration.miniGameCanvas.SetActive(true);
            GenerateSequence();
            _currentArrowIndex = 0;
            _allowInput = true;
        }

        private void Update()
        {
            if (!_allowInput) return;

            if (!Input.anyKeyDown) return;
            
            KeyCode[] requiredKeyCodes = _arrowKeyCodes[_sequence[_currentArrowIndex].direction];
            bool correctInput = requiredKeyCodes.Any(Input.GetKeyDown);

            if (correctInput)
            {
                ProcessArrowSuccess();
            }
            else
            {
                StartCoroutine(ProcessArrowFailure());
            }
        }

        private void ProcessArrowSuccess()
        {
            _sequence[_currentArrowIndex].arrow.GetComponentInChildren<TextMeshProUGUI>().color = Color.green;
            gameManager.soundEffectPlayer.PlayClip(gameManager.soundEffectRegistration.successArrowSoundEffect, .5f);
           
            if (_sequence.Count - 1 != _currentArrowIndex)
            {
                _currentArrowIndex++;
            }
            else
            {
               ProcessGameWin();
            }
        }

        private IEnumerator ProcessArrowFailure()
        {
            _allowInput = false;
            gameManager.soundEffectPlayer.PlayClip(gameManager.soundEffectRegistration.failArrowSoundEffect);
            
            for (int i = 0; i <= _currentArrowIndex; i++)
            {
                _sequence[i].arrow.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
            }

            yield return new WaitForSecondsRealtime(1);
            
            for (int i = 0; i <= _currentArrowIndex; i++)
            {
                _sequence[i].arrow.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
            }

            _currentArrowIndex = 0;
            _allowInput = true;
        }

        private void ProcessGameWin()
        {
            gameManager.soundEffectPlayer.PlayClip(gameManager.soundEffectRegistration.collectIngredientSoundEffect);
            uiRegistration.miniGameCanvas.SetActive(false);
            OnEndMiniGame?.Invoke(_ingredientBeingPlayedFor);
            ResetMiniGame();
        }

        private void GenerateSequence()
        {
            int sequenceLength = baseArrowCount + gameManager.GetNumberOfIngredientsCollected();
            const int baseXPosition = -390;
            const int arrowWidth = 115;
            _sequence = new List<(ArrowDirections direction, GameObject arrow)>();

            for (int i = 0; i < sequenceLength; i++)
            {
                int randomDirection = UnityEngine.Random.Range(0, 4);
                ArrowDirections direction = (ArrowDirections) randomDirection;
                
                GameObject arrow = Instantiate(arrowPrefab, uiRegistration.sequencePanel.transform);
                
                float rotation = direction switch
                {
                    ArrowDirections.Up => 0,
                    ArrowDirections.Down => 180,
                    ArrowDirections.Left => 90,
                    ArrowDirections.Right => 270,
                    _ => 0
                };

                arrow.transform.localPosition = new Vector3(baseXPosition + (arrowWidth * i), 0, 0);
                arrow.transform.localRotation = Quaternion.Euler(0, 0, rotation);

                _sequence.Add((direction, arrow));
            }
        }

        private void ResetMiniGame()
        {
            _allowInput = false;
            foreach(GameObject arrow in _sequence.Select(tuple => tuple.arrow))
            {
                Destroy(arrow);
            }
            _sequence = null;
            _ingredientBeingPlayedFor = null;
        }
    }
}
