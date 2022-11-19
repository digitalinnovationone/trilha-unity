using UnityEngine;

namespace Player.States {
    public class Hurt : State {

        private PlayerController controller;
        
        private float timePassed;

        public Hurt(PlayerController controller) : base("Hurt") {
            this.controller = controller;
        }

        public override void Enter() {
            base.Enter();

            // Reset timer
            timePassed = 0;
            
            // Pause damage
            controller.thisLife.isVulnerable = false;
            controller.invulnerabilityTimeLeft = controller.invulnerabilityDuration;
            
            // Break attack chain
            controller.currentAttackStage = 1;
            
            // Update animator
            controller.thisAnimator.SetTrigger("tHurt");
            
            // Update UI
            var gameplayUI = GameManager.Instance.gameplayUI;
            gameplayUI.playerHealthBar.SetHealth(controller.thisLife.health);
        }

        public override void Exit() {
            base.Exit();
        }

        public override void Update() {
            base.Update();

            // Switch to dead
            if (controller.thisLife.IsDead()) {
                controller.stateMachine.ChangeState(controller.deadState);
                return;
            }

            // Update timer
            timePassed += Time.deltaTime;
            
            // Switch to idle
            if (timePassed >= controller.hurtDuration) {
                controller.stateMachine.ChangeState(controller.idleState);
                return;
            }
        }

        public override void LateUpdate() {
            base.LateUpdate();
        }

        public override void FixedUpdate() {
            base.FixedUpdate();
        }

    }
}
