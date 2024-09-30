using System.Collections;
using UnityEngine;

namespace _Unity_Essentials.Source_Files.Scripts
{
    public class PlaySoundAtRandomIntervals : MonoBehaviour
    {
        public float minSeconds = 5f; // Minimum interval to wait before playing sound.
        public float maxSeconds = 15f; // Maximum interval to wait before playing sound.

        private AudioSource _audioSource;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            StartCoroutine(PlaySound());
        }

        private IEnumerator PlaySound()
        {
            while (true)
            {
                float waitTime = Random.Range(minSeconds, maxSeconds);
                yield return new WaitForSeconds(waitTime);
                _audioSource.Play();
            }
        }
    }
}