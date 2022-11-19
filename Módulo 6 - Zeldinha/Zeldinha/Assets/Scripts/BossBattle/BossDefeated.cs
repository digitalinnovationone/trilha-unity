using UnityEngine;
using UnityEngine.Rendering;

namespace BossBattle {
    public class BossDefeated : State {

        public BossDefeated() : base("BossDefeated") {}

        public override void Enter() {
            base.Enter();

            // Create death sequence
            var gameManager = GameManager.Instance;
            var boss = gameManager.boss;
            var sequencePrefab = gameManager.bossDeathSequence;
            Object.Instantiate(sequencePrefab, boss.transform.position, sequencePrefab.transform.rotation);
            
            // Stop boss music
            gameManager.StartCoroutine(FadeAudioSource.StartFade(
                gameManager.bossMusic, 0, 0.2f
            ));
            
            // Stop ambience
            gameManager.StartCoroutine(FadeAudioSource.StartFade(
                gameManager.ambienceMusic, 0, 0.2f
            ));
        }

        public override void Exit() {
            base.Exit();
        }

    }
}
