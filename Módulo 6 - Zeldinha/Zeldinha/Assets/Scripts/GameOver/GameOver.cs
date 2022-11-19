using System;
using System.Collections;
using EventArgs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameOver {
    public class GameOver : MonoBehaviour {

        public GameObject objectToEnable;
        public float gameOverDuration = 6f;
        private bool isTriggered;

        private void Start() {
            GlobalEvents.Instance.OnGameOver += OnGameOver;
        }

        private void OnGameOver(object sender, GameOverArgs args) {
            // Debounce
            if (isTriggered) return;
            isTriggered = true;

            // Activate object
            objectToEnable.SetActive(true);

            // Fade out music and ambience
            var gameManager = GameManager.Instance;
            StartCoroutine(FadeAudioSource.StartFade(gameManager.gameplayMusic, 0, 0.2f));
            StartCoroutine(FadeAudioSource.StartFade(gameManager.bossMusic, 0, 0.2f));
            StartCoroutine(FadeAudioSource.StartFade(gameManager.ambienceMusic, 0, 6f));
            
            // Schedule scene reload
            StartCoroutine(ReloadScene());
        }

        private IEnumerator ReloadScene() {
            yield return new WaitForSeconds(gameOverDuration);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }
}
