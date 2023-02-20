using UnityEngine;

namespace Utilities
{
    public class Timer : MonoBehaviour
    {
        public float startTime = 30;
        public bool isTimerRunning = false;
        public int minutesRemaining;
        public int secondsRemaining;
        
        public delegate void TimerUpdate(int minutesRemaining, int secondsRemaining);
        public static event TimerUpdate OnTimerUpdate;
        
        private float _timeRemaining = 30;

        public void StartTimer()
        {
            isTimerRunning = true;
        }

        public void PauseTimer()
        {
            isTimerRunning = false;
        }

        public void StopTimer()
        {
            isTimerRunning = false;
            _timeRemaining = startTime;
        }
        
        private void Update()
        {
            if(!isTimerRunning) return;
            
            if (_timeRemaining > 0)
            {
                _timeRemaining -= Time.deltaTime;
                minutesRemaining = Mathf.FloorToInt(_timeRemaining / 60);
                secondsRemaining = Mathf.FloorToInt(_timeRemaining % 60);
                OnTimerUpdate?.Invoke(minutesRemaining, secondsRemaining);
            }
            else
            {
                _timeRemaining = 0;
                isTimerRunning = false;
                OnTimerUpdate?.Invoke(0, 0);
            }
        }
    }
}
