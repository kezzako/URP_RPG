using RPG.Control;
using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public IEnumerator FadeOut(float duration)
        {
            CanvasGroup group = GetComponent<CanvasGroup>();

            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                group.alpha = Mathf.Lerp(0, 1, elapsedTime/ duration);
                elapsedTime += Time.deltaTime;
                yield return null;

            }
            group.alpha = 1;
        }

        public IEnumerator FadeIn(float duration)
        {
            //disable control otherwise you can move during the fade out
            //and fade in. Particularly the portal from the previous scene
            //isn't destroyed until the end of FadeIn, so you could walk
            //into it again by accident
            DisableControl();

            CanvasGroup group = GetComponent<CanvasGroup>();

            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                group.alpha = Mathf.Lerp(1, 0, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;

            }
            group.alpha = 0;
            EnableControl();
        }

        private void OnDestroy()
        {
            Debug.Log("Fader destroyed");
        }

        void DisableControl()
        {
            Debug.Log("DisableControl");
            GameObject player = GameObject.FindWithTag("Player");
            Debug.Log(player);
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;

        }

        void EnableControl()
        {
            Debug.Log("EnableControl");

            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<PlayerController>().enabled = true;
        }
    }
}
