using UnityEngine;

namespace Runner.Scripts.Service
{
    public static class PlayerService
    {
        private const string HighestScoreKey = "A8CBE85B-12A9-4ED4-967F-5A1590C422E5";

        public static int GetSavedHighestScore()
        {
            return PlayerPrefs.GetInt(HighestScoreKey, 0);
        }

        public static void SaveHighestScore(int currentScore)
        {
            PlayerPrefs.SetInt(HighestScoreKey, currentScore);
        }
    }
}
