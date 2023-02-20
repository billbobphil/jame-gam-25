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
        public GameObject upArrowPrefab;
        public GameObject downArrowPrefab;
        public GameObject leftArrowPrefab;
        public GameObject rightArrowPrefab;
        private List<(ArrowDirections direction, GameObject arrow)> _sequence;
        private bool _allowInput;
        private int _currentArrowIndex;
        private GameObject _ingredientBeingPlayedFor;
        
        public delegate void EndMiniGame(GameObject ingredient);
        public static event EndMiniGame OnEndMiniGame;
        
        private void OnEnable()
        {
            IngredientMiniGameEventNotifier.OnStartMiniGame += StartMiniGame;
        }

        private void OnDisable()
        {
            IngredientMiniGameEventNotifier.OnStartMiniGame -= StartMiniGame;
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
            _sequence[_currentArrowIndex].arrow.GetComponent<TextMeshProUGUI>().color = Color.green;
           
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
            
            for (int i = 0; i <= _currentArrowIndex; i++)
            {
                _sequence[i].arrow.GetComponent<TextMeshProUGUI>().color = Color.red;
            }

            yield return new WaitForSecondsRealtime(1);
            
            for (int i = 0; i <= _currentArrowIndex; i++)
            {
                _sequence[i].arrow.GetComponent<TextMeshProUGUI>().color = Color.white;
            }

            _currentArrowIndex = 0;
            _allowInput = true;
        }

        private void ProcessGameWin()
        {
            uiRegistration.miniGameCanvas.SetActive(false);
            OnEndMiniGame?.Invoke(_ingredientBeingPlayedFor);
            ResetMiniGame();
        }

        private void GenerateSequence()
        {
            //TODO: make dynamic based on number completed
            int sequenceLength = 4;
            const int baseXPosition = -390;
            const int arrowWidth = 101;
            _sequence = new List<(ArrowDirections direction, GameObject arrow)>();

            for (int i = 0; i < sequenceLength; i++)
            {
                int randomDirection = UnityEngine.Random.Range(0, 4);
                ArrowDirections direction = (ArrowDirections) randomDirection;

                GameObject arrow = direction switch
                {
                    ArrowDirections.Up => Instantiate(upArrowPrefab, uiRegistration.sequencePanel.transform),
                    ArrowDirections.Down => Instantiate(downArrowPrefab, uiRegistration.sequencePanel.transform),
                    ArrowDirections.Left => Instantiate(leftArrowPrefab, uiRegistration.sequencePanel.transform),
                    ArrowDirections.Right => Instantiate(rightArrowPrefab, uiRegistration.sequencePanel.transform),
                    _ => Instantiate(upArrowPrefab, uiRegistration.sequencePanel.transform)
                };

                arrow.transform.localPosition = new Vector3(baseXPosition + (arrowWidth * i), 0, 0);

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
