using UnityEngine;

namespace _Unity_Essentials.Scripts
{
    public class DoorOpener : MonoBehaviour
    {
        [SerializeField] private Animator doorAnimator;
        private static readonly int DoorOpen = Animator.StringToHash("Door_Open");

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            if (doorAnimator != null)
            {
                // Trigger the Door_Open animation
                doorAnimator.SetTrigger(DoorOpen);
            }
        }
    }
}
