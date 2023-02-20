using UnityEngine;

namespace Utilities
{
    public class UiRegistration : MonoBehaviour
    {
        public GameObject miniGameCanvas;
        public GameObject sequencePanel;
        public GameObject gameCanvas;
        public GameObject timerText;
        
        void Start()
        {
            miniGameCanvas.SetActive(false);
        }
    }
}
