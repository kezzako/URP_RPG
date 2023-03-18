using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup _group;

        private void Awake()
        {
            _group = GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            //StartCoroutine(FadeOutIn(1.5f, 1.5f));
        }

        IEnumerator FadeOutIn(float fadeOutDuration, float fadeInDuration)
        {
            yield return FadeOut(fadeOutDuration);
            yield return FadeIn(fadeInDuration);

        }

        public IEnumerator FadeOut(float duration)
        {
            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                _group.alpha = Mathf.Lerp(0, 1, elapsedTime/ duration);
                elapsedTime += Time.deltaTime;
                yield return null;

            }
            _group.alpha = 1;
        }

        public IEnumerator FadeIn(float duration)
        {
            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                _group.alpha = Mathf.Lerp(1, 0, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;

            }
            _group.alpha = 0;
        }
    }
}
