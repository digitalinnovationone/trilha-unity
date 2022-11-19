using System.Collections;
using UnityEngine;

public static class FadeAudioSource {

    public static IEnumerator StartFade(AudioSource audioSource, float targetVolume, float duration) {
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration) {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
    }

}
