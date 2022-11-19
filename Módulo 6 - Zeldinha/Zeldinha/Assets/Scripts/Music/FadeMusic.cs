using UnityEngine;

namespace Music {
    public class FadeMusic : MonoBehaviour {

        public AudioSource music;
        private float originalVolume;
        public float fadeInDuration = 1f;
        public float fadeOutDuration = 1f;

        private void Awake() {
            originalVolume = music.volume;
            music.volume = 0;
            FadeIn();
        }

        public void FadeIn() {
            StartCoroutine(FadeAudioSource.StartFade(music, originalVolume, fadeInDuration));
        }

        public void FadeOut() {
            StartCoroutine(FadeAudioSource.StartFade(music, 0, fadeOutDuration));
        }

    }
}
