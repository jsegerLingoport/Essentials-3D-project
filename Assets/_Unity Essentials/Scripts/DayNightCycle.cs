using UnityEngine;

namespace _Unity_Essentials.Scripts
{
    public class DayNightCycle : MonoBehaviour
    {
        [Tooltip("Time in seconds for a full day to pass.")]
        public float dayDuration = 60f; // The duration of a simulated day in seconds.

        private void Update()
        {
            // Calculate rotation speed based on dayDuration.
            var rotationSpeed = 360f / dayDuration * Time.deltaTime;
        
            // Rotate the light around the X-axis to simulate the passing of a day.
            transform.Rotate(Vector3.right * rotationSpeed);
        }
    }
}