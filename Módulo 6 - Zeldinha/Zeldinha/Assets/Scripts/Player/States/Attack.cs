using UnityEngine;

namespace Player.States {
    public class Attack : State {

        private PlayerController controller;

        private float stateTime;
        private bool firstFixedUpdate;

        public Attack(PlayerController controller) : base("Attack") {
            this.controller = controller;
        }

        public override void Enter() {
            base.Enter();

            // ERROR: Invalid stage
            var stage = controller.currentAttackStage;
            if (stage <= 0 || stage > controller.attackStages) {
                controller.stateMachine.ChangeState(controller.idleState);
                return;
            }

            // Reset variables
            stateTime = 0;
            firstFixedUpdate = true;

            // Set animator trigger
            controller.thisAnimator.SetTrigger("tAttack" + stage);

            // Toggle sword hitbox
            controller.swordHitbox.SetActive(true);

            // Effect
            var attackEffect = controller.attackEffects[stage - 1];
            var effectPosition = controller.swordHitbox.transform.position;
            var effectRotation = attackEffect.transform.rotation;
            Object.Instantiate(attackEffect, effectPosition, effectRotation);
            
            // Configure time to next stage
            var stageDuration = controller.attackStageDurations[stage - 1];
            var stageMaxInterval = controller.attackStageMaxIntervals[stage - 1];
            controller.timeLeftToAdvanceAttackStages = stageDuration + stageMaxInterval;
        }

        public override void Exit() {
            base.Exit();

            // Toggle hitbox
            controller.swordHitbox.SetActive(false);
        }

        public override void Update() {
            base.Update();

            // Switch to Attack (again, yes)
            if (controller.AttemptToAttack()) {
                return;
            }

            // Update StateTime
            stateTime += Time.deltaTime;

            // Disable sword hitbox after a certain time
            if (controller.swordHitbox.activeInHierarchy) {
                if (GetStageTimeRate() >= controller.swordActiveThreshold) {
                    controller.swordHitbox.SetActive(false);
                }
            }
            
            // // Exit after time
            if (CanEndState()) {
                controller.stateMachine.ChangeState(controller.idleState);
                return;
            }
        }

        public override void LateUpdate() {
            base.LateUpdate();
        }

        public override void FixedUpdate() {
            base.FixedUpdate();
            var stage = controller.currentAttackStage;

            if (firstFixedUpdate) {
                firstFixedUpdate = false;

                // Look to input
                controller.RotateBodyToFaceInput(1);

                // Impulse
                var impulseValue = controller.attackStageIpulses[stage - 1];
                var impulseVector = controller.thisRigidbody.rotation * Vector3.forward;
                impulseVector *= impulseValue;
                controller.thisRigidbody.AddForce(impulseVector, ForceMode.Impulse);
            }
        }

        private bool CanEndState() {
            var stage = controller.currentAttackStage;
            var stageDuration = controller.attackStageDurations[stage - 1];
            return stateTime >= stageDuration;
        }

        public bool CanSwitchStages() {

            // Get attack variables
            var stage = controller.currentAttackStage;
            var isLastState = stage == controller.attackStages;
            var stageDuration = controller.attackStageDurations[stage - 1];
            var stageMaxInterval = isLastState ? 0 : controller.attackStageMaxIntervals[stage - 1];
            var maxStageDuration = stageDuration + stageMaxInterval;

            // Reply
            return !isLastState && stateTime >= stageDuration && stateTime <= maxStageDuration;
        }

        public float GetStageTimeRate() {
            var stage = controller.currentAttackStage;
            var stageDuration = controller.attackStageDurations[stage - 1];
            return stateTime / stageDuration;
        }

    }
}
