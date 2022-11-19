using Music;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI {
    public class CreditsScreenUI : MonoBehaviour {

        public FadeMusic fadeMusic;

        public void FadeOutMusic() {
            fadeMusic.FadeOut();
        }

        public void SwitchToTitleScreen() {
            SceneManager.LoadScene("Scenes/TitleScreen");
        }

    }
}
