using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utilities
{
    public class InGameMenuController : MonoBehaviour
    {
        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
