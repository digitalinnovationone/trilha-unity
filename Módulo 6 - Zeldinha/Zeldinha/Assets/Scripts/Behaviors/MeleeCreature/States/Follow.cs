using UnityEngine;

namespace Behaviors.MeleeCreature.States {
    public class Follow : State {

        private MeleeCreatureController controller;
        private MeleeCreatureHelper helper;

        private readonly float updateInterval = 1;
        private float updateCooldown;
        private float ceaseFollowCooldown;

        public Follow(MeleeCreatureController controller) : base("Follow") {
            this.controller = controller;
            this.helper = controller.helper;
        }

        public override void Enter() {
            base.Enter();
  
            // Reset stuff
            updateCooldown = 0;
            ceaseFollowCooldown = controller.ceaseFollowInterval;
        }

        public override void Exit() {
            base.Exit();
            
            // Stop following
            controller.thisAgent.ResetPath();
        }

        public override void Update() {
            base.Update();
            
            // Update destination
            if ((updateCooldown -= Time.deltaTime) <= 0f) {
                updateCooldown = updateInterval;
                var player = GameManager.Instance.player;
                var playerPosition = player.transform.position;
                controller.thisAgent.SetDestination(playerPosition);
            }
            
            // Cease follow
            if ((ceaseFollowCooldown -= Time.deltaTime) <= 0f) {
                if (!helper.IsPlayerOnSight()) {
                    controller.stateMachine.ChangeState(controller.idleState);
                    return;
                }
            }
            
            // Attempt to attack
            if (helper.GetDistanceToPlayer() <= controller.distanceToAttack) {
                controller.stateMachine.ChangeState(controller.attackState);
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
