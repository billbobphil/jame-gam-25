using TMPro;
using UnityEngine;

namespace Utilities
{
    public class UiRegistration : MonoBehaviour
    {
        public GameObject miniGameCanvas;
        public GameObject sequencePanel;
        public GameObject gameCanvas;
        public GameObject timerText;
        public GameObject gameEndCanvas;
        public GameObject collectionAndModifierPanel;
        public TextMeshProUGUI modifierText;
        public TextMeshProUGUI collectedIngredientText;
        public TextMeshProUGUI collectedChickenText;
        public TextMeshProUGUI collectedButterText;
        public TextMeshProUGUI collectedTomatoText;
        public TextMeshProUGUI collectedGarlicText;
        public TextMeshProUGUI collectedOnionText;
        public TextMeshProUGUI collectedCreamText;

        private void Start()
        {
            miniGameCanvas.SetActive(false);
            gameEndCanvas.SetActive(false);
            collectionAndModifierPanel.SetActive(false);
        }
    }
}
