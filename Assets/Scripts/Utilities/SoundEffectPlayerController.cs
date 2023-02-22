using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public class SoundEffectPlayerController : MonoBehaviour
    {
        public List<AudioSource> audioSources;
        private int _currentSourceIndex = 0;

        public void PlayClip(AudioClip clip, float volume = 1f)
        {
            audioSources[_currentSourceIndex].clip = clip;
            audioSources[_currentSourceIndex].Play();
            audioSources[_currentSourceIndex].volume = volume;

            if (_currentSourceIndex == audioSources.Count - 1)
            {
                _currentSourceIndex = 0;                
            }
            else
            {
                _currentSourceIndex++;
            }
        }
    }
}
