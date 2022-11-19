using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI {
    public class TitleScreenUI : MonoBehaviour {
        public Animator thisAnimator;
        public float fadeOutDuration;

        private bool isFadingOut;

        public void FadeOut() {
            // Debounce
            if (isFadingOut) return;
            isFadingOut = true;

            // Fade out
            thisAnimator.SetTrigger("tFadeOut");

            // Schedule scene
            StartCoroutine(TransitionToNextScene());
        }

        private IEnumerator TransitionToNextScene() {
            yield return new WaitForSeconds(fadeOutDuration);
            SceneManager.LoadScene("Scenes/Dungeon");
        }

    }
}
