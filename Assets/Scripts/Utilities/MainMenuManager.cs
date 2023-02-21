using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utilities
{
    public class MainMenuManager : MonoBehaviour
    {
        public void Play()
        {
            SceneManager.LoadScene(1);
        }

        public void Tutorial()
        {
            SceneManager.LoadScene(2);
        }

        public void Credits()
        {
            SceneManager.LoadScene(3);
        }

        public void Menu()
        {
            SceneManager.LoadScene(0);
        }

        public void OnDifficultyModified(int newPosition)
        {
            switch (newPosition)
            {
                case 0:
                    Difficulty.SetDifficultyLevel(Difficulty.DifficultyLevel.Normal);
                    break;
                case 1:
                    Difficulty.SetDifficultyLevel(Difficulty.DifficultyLevel.Easy);
                    break;
                case 2:
                    Difficulty.SetDifficultyLevel(Difficulty.DifficultyLevel.Hard);
                    break;
            }
        }
    }
}
