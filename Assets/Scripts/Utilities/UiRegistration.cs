using UnityEngine;

namespace Utilities
{
    public class UiRegistration : MonoBehaviour
    {
        public GameObject miniGameCanvas;
        
        void Start()
        {
            miniGameCanvas.SetActive(false);
        }
    }
}
