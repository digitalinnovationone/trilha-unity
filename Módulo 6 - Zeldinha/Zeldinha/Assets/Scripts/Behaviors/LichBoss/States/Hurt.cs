using UnityEngine;

namespace Behaviors.LichBoss.States {
    public class Hurt : State {

        private LichBossController controller;
        private LichBossHelper helper;

        private float timePassed;

        public Hurt(LichBossController controller) : base("Hurt") {
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
            
            // Increment hits taken
            controller.hitsTakenWithoutRitual++;
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
                var isDead = controller.thisLife.IsDead();
                var isHittingBack = controller.hitsTakenWithoutRitual >= controller.hitBackAfterHits;
                if (isDead) {
                    controller.stateMachine.ChangeState(controller.deadState);
                } else if(isHittingBack) {
                    controller.hitsTakenWithoutRitual = 0;
                    controller.stateMachine.ChangeState(controller.attackRitualState);
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
