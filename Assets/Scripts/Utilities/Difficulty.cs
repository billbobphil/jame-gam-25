namespace Utilities
{
    public static class Difficulty
    {
        private static DifficultyLevel _difficultyLevel = DifficultyLevel.Normal;
        
        public enum DifficultyLevel
        {
            Easy,
            Normal,
            Hard
        }

        public static void SetDifficultyLevel(DifficultyLevel difficultyLevel)
        {
            _difficultyLevel = difficultyLevel;
        }
        
        public static DifficultyLevel GetDifficultyLevel()
        {
            return _difficultyLevel;
        }
    }
}