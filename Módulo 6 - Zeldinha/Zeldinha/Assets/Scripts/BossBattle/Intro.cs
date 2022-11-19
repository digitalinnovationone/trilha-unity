using UnityEngine;

namespace BossBattle {
    public class Intro : State {

        private readonly float duration = 3f;
        private float timeElapsed = 0;

        public Intro() : base("Intro") {}

        public override void Enter() {
            base.Enter();

            // Reset stuff
            timeElapsed = 0;

            // Enable hidden parts
            var gameManager = GameManager.Instance;
            gameManager.bossBattleParts.SetActive(true);

            // Stop gameplay music
            var gameplayMusic = gameManager.gameplayMusic;
            gameManager.StartCoroutine(FadeAudioSource.StartFade(
                gameplayMusic, 0, 2f
            ));
            
            // Play boss music
            var bossMusic = gameManager.bossMusic;
            var bossMusicVolume = bossMusic.volume;
            bossMusic.volume = 0;
            gameManager.StartCoroutine(FadeAudioSource.StartFade(
                bossMusic, bossMusicVolume, 0.5f
            ));
            bossMusic.Play();
        }

        public override void Exit() {
            base.Exit();
        }

        public override void Update() {
            base.Update();
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= duration) {
                var bossBattleHandler = GameManager.Instance.bossBattleHandler;
                bossBattleHandler.stateMachine.ChangeState(bossBattleHandler.stateBattle);
            }
        }

    }
}
