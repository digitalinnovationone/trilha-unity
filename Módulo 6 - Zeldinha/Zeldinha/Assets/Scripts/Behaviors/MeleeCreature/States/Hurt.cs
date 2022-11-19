using UnityEngine;

namespace Behaviors.MeleeCreature.States {
    public class Hurt : State {

        private MeleeCreatureController controller;
        private MeleeCreatureHelper helper;

        private float timePassed;

        public Hurt(MeleeCreatureController controller) : base("Hurt") {
            this.controller = controller;
            this.helper = controller.helper;
        }

        public override void Enter() {
            base.Enter();
            
            // Reset timer
            timePassed = 0;

            // Pause damage
            controller.thisLife.isVulnerable = false;
            
            // Update animator
            controller.thisAnimator.SetTrigger("tHurt");
            
            // Shift object control from NavMesh to Physics
            controller.thisAgent.enabled = false;
            controller.thisRigidbody.isKinematic = false;
        }

        public override void Exit() {
            base.Exit();

            // Resume damage
            controller.thisLife.isVulnerable = true;
            
            // Shift object control from Physics back to NavMesh
            controller.thisAgent.enabled = true;
            controller.thisRigidbody.isKinematic = true;
        }

        public override void Update() {
            base.Update();

            // Update timer
            timePassed += Time.deltaTime;
            
            // Switch states
            if (timePassed >= controller.hurtDuration) {
                if (controller.thisLife.IsDead()) {
                    controller.stateMachine.ChangeState(controller.deadState);
                } else {
                    controller.stateMachine.ChangeState(controller.idleState);
                }
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
