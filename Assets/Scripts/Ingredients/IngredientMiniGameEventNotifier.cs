using UnityEngine;

namespace Ingredients
{
    public class IngredientMiniGameEventNotifier : MonoBehaviour
    {
        public delegate void StartMiniGame();
        public static event StartMiniGame OnStartMiniGame;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("ClawHand"))
            {
                OnStartMiniGame?.Invoke();
            }
        }
    }
}
