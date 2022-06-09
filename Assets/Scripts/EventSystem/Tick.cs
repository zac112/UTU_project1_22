using System.Collections;
using UnityEngine;

namespace EventSystem
{
    public class Tick : MonoBehaviour
    {
        // Tick values
        private float tickTimer;
        private float tickTimerMax;
        private int currentTick = 0;

        public int GetCurrentTick() { return currentTick; }   // needed by GameStats for assessing game duration in the end

        [Tooltip("Number of game ticks per second")] [SerializeField]
        private int tickSpeed;

        public int GetTickSpeed() { return tickSpeed; }   // needed by GameStats for assessing game duration in the end

        private void Start()
        {
            StartCoroutine(DoTick());
        }

        private IEnumerator DoTick() {
            while(true){
                currentTick++;
                GameEvents.current.OnTick(currentTick);
                yield return new WaitForSeconds(1f/tickSpeed);
            }
        }
    }
}
