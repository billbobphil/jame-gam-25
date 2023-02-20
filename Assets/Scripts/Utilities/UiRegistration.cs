using UnityEngine;

namespace Utilities
{
    public class UiRegistration : MonoBehaviour
    {
        public GameObject miniGameCanvas;
        public GameObject sequencePanel;
        
        void Start()
        {
            miniGameCanvas.SetActive(false);
        }
    }
}
