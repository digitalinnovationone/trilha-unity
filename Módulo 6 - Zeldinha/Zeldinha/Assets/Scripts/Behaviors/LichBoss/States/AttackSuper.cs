using System.Collections;
using Projectiles;
using UnityEngine;

namespace Behaviors.LichBoss.States {
    public class AttackSuper : State {

        private LichBossController controller;
        private LichBossHelper helper;

        private float endAttackCooldown;

        public AttackSuper(LichBossController controller) : base("AttackSuper") {
            this.controller = controller;
            this.helper = controller.helper;
        }

        public override void Enter() {
            base.Enter();

            // Set variables
            endAttackCooldown = controller.attackSuperDuration;

            // Update animator
            controller.thisAnimator.SetTrigger("tAttackSuper");

            // Schedule attack
            var delayStep = controller.attackSuperMagicDuration / (controller.attackSuperMagicCount - 1);
            for (int i = 0; i < controller.attackSuperMagicCount - 1; i++) {
                var delay = controller.attackSuperMagicDelay + delayStep * i;
                helper.StartStateCoroutine(
                    ScheduleAttack(delay)
                );
            }
        }

        public override void Exit() {
            base.Exit();
            helper.ClearStateCoroutines();
        }

        public override void Update() {
            base.Update();

            // End attack
            if ((endAttackCooldown -= Time.deltaTime) <= 0f) {
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

        private IEnumerator ScheduleAttack(float delay) {
            yield return new WaitForSeconds(delay);

            // Create object
            var spawnTransform = controller.staffTop;
            var projectile = Object.Instantiate(
                controller.energyBallPrefab,
                spawnTransform.position,
                spawnTransform.rotation
            );
            
            // Populate ProjectileCollision
            var projectileCollision = projectile.GetComponent<ProjectileCollision>();
            projectileCollision.attacker = controller.gameObject;
            projectileCollision.damage = controller.attackDamage;
            
            // Get stuff
            var player = GameManager.Instance.player;
            var projectileRigidbody = projectile.GetComponent<Rigidbody>();

            // Apply impulse
            var vectorToPlayer = (player.transform.position + controller.aimOffset - spawnTransform.position).normalized;
            var forceVector = spawnTransform.rotation * Vector3.forward;
            forceVector = new Vector3(forceVector.x, vectorToPlayer.y, forceVector.z);
            forceVector *= controller.attackSuperImpulse;
            projectileRigidbody.AddForce(forceVector, ForceMode.Impulse);

            // Schedule destruction
            Object.Destroy(projectile, 30);
        }

    }
}
