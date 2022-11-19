using UnityEngine;

namespace Player.States {
    public class Idle : State {

        private PlayerController controller;

        public Idle(PlayerController controller) : base("Idle") {
            this.controller = controller;
        }

        public override void Enter() {
            base.Enter();
        }

        public override void Exit() {
            base.Exit();
        }

        public override void Update() {
            base.Update();

            // Ignore if game is won
            if (GameManager.Instance.isGameWon) return;

            // Ignore if in cutscene
            var bossBattleHandler = GameManager.Instance.bossBattleHandler;
            var isInCutscene = bossBattleHandler.IsInCutscene();
            if (isInCutscene) return;
            
            // Switch to Attack
            if (controller.AttemptToAttack()) {
                return;
            }

            // Switch to Defense
            if (controller.hasDefenseInput) {
                controller.stateMachine.ChangeState(controller.defendState);
                return;
            }

            // Switch to Jump
            if (controller.hasJumpInput) {
                controller.stateMachine.ChangeState(controller.jumpState);
                return;
            }

            // Switch to Walking
            if (!controller.movementVector.IsZero()) {
                controller.stateMachine.ChangeState(controller.walkingState);
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
