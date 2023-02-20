using System;
using Ingredients;
using UnityEngine;

namespace Utilities
{
    public class MiniGameController : MonoBehaviour
    {
        public UiRegistration uiRegistration;
        
        private void OnEnable()
        {
            IngredientMiniGameEventNotifier.OnStartMiniGame += StartMiniGame;
        }

        private void OnDisable()
        {
            IngredientMiniGameEventNotifier.OnStartMiniGame -= StartMiniGame;
        }

        private void StartMiniGame()
        {
            Debug.Log("FIRING MY EVENT LISTENER METHOD");
            uiRegistration.miniGameCanvas.SetActive(true);
        }
    }
}
