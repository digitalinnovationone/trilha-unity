using UnityEngine;
using UnityEngine.SceneManagement;

namespace BossBattle {
    public class BossDeathSequence  : MonoBehaviour {

        public void SwitchToCreditsScene() {
            SceneManager.LoadScene("Scenes/Credits");
        }

    }
}
