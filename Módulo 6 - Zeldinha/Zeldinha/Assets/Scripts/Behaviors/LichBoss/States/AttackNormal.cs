using System.Collections;
using Projectiles;
using UnityEngine;

namespace Behaviors.LichBoss.States {
    public class AttackNormal : State {

        private LichBossController controller;
        private LichBossHelper helper;

        private float endAttackCooldown;

        public AttackNormal(LichBossController controller) : base("AttackNormal") {
            this.controller = controller;
            this.helper = controller.helper;
        }

        public override void Enter() {
            base.Enter();

            // Set variables
            endAttackCooldown = controller.attackNormalDuration;

            // Update animator
            controller.thisAnimator.SetTrigger("tAttackNormal");

            // Schedule attack
            helper.StartStateCoroutine(
                ScheduleAttack(controller.attackNormalMagicDelay)
            );
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
                controller.fireballPrefab,
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
            forceVector *= controller.attackNormalImpulse;
            projectileRigidbody.AddForce(forceVector, ForceMode.Impulse);

            // Schedule destruction
            Object.Destroy(projectile, 30);
        }

    }
}
