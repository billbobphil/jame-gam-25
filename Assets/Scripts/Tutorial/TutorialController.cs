using System;
using System.Collections;
using Ingredients;
using TMPro;
using UnityEngine;
using Utilities;

namespace Tutorial
{
    public class TutorialController : MonoBehaviour
    {
        public delegate void TutorialBlockActions();
        public static event TutorialBlockActions OnTutorialBlockActions;
        
        public delegate void TutorialUnblockActions();
        public static event TutorialUnblockActions OnTutorialUnblockActions;
        
        public delegate void TutorialBlockMiniGame();
        public static event TutorialBlockMiniGame OnTutorialBlockMiniGame;
        
        public delegate void TutorialUnblockMiniGame();
        public static event TutorialUnblockMiniGame OnTutorialUnblockMiniGame;
        
        public GameManager gameManager;
        public GameObject youPanel;
        public GameObject movePanPanel;
        public GameObject collectionPanel;
        public GameObject miniGamePanel;
        public AudioSource continueAudioSource;

        private enum TutorialState
        {
            You,
            MovePan,
            TryingPanOut,
            CollectionPhase,
            WaitForMiniGameStart,
            MiniGameActive
        }

        private TutorialState _currentState;

        private void OnEnable()
        {
            IngredientMiniGameEventNotifier.OnStartMiniGame += MiniGameStarted;
        }
        
        private void OnDisable()
        {
            IngredientMiniGameEventNotifier.OnStartMiniGame -= MiniGameStarted;
        }

        private void Awake()
        {
            youPanel.SetActive(false);
            movePanPanel.SetActive(false);
            collectionPanel.SetActive(false);
            miniGamePanel.SetActive(false);
        }
        
        private void Start()
        {
            _currentState = TutorialState.You;
            gameManager.uiRegistration.timerText.GetComponent<TextMeshProUGUI>().text = "∞";
            youPanel.SetActive(true);
            OnTutorialBlockActions?.Invoke();
        }
        
        private void Update()
        {
            switch (_currentState)
            {
                case TutorialState.You:
                    if (Input.GetKeyDown(KeyCode.Backspace))
                    {
                        youPanel.SetActive(false);
                        OnTutorialUnblockActions?.Invoke();
                        _currentState = TutorialState.MovePan;
                        movePanPanel.SetActive(true);
                        continueAudioSource.Play();
                    }
                    break;
                case TutorialState.MovePan:
                    if (Input.GetKeyDown(KeyCode.Backspace))
                    {
                        movePanPanel.SetActive(false);
                        _currentState = TutorialState.TryingPanOut;
                        continueAudioSource.Play();
                        StartCoroutine(TransitionToCollectionPhase());
                    }
                    break;
                case TutorialState.CollectionPhase:
                    if (Input.GetKeyDown(KeyCode.Backspace))
                    {
                        continueAudioSource.Play();
                        collectionPanel.SetActive(false);;
                        _currentState = TutorialState.WaitForMiniGameStart;
                        gameManager.SpawnIngredients();
                    }
                    break;
                case TutorialState.MiniGameActive:
                    if (Input.GetKeyDown(KeyCode.Backspace))
                    {
                        continueAudioSource.Play();
                        miniGamePanel.SetActive(false);
                        StartCoroutine(UnblockMinigameInput());
                    }
                    break;
            }
        }

        private IEnumerator TransitionToCollectionPhase()
        {
            yield return new WaitForSeconds(5f);
            _currentState = TutorialState.CollectionPhase;
            collectionPanel.SetActive(true);
        }

        private void MiniGameStarted(GameObject ingredient)
        {
            StartCoroutine(TransitionToMiniGameStart());
        }

        private IEnumerator TransitionToMiniGameStart()
        {
            miniGamePanel.SetActive(true);
            _currentState = TutorialState.MiniGameActive;
            yield return new WaitForSecondsRealtime(.3f);
            OnTutorialBlockMiniGame?.Invoke();
        }

        private IEnumerator UnblockMinigameInput()
        {
            yield return new WaitForSecondsRealtime(.3f);
            OnTutorialUnblockMiniGame?.Invoke();
        }


    }
}
