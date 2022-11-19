using System.Collections;
using UnityEngine;

namespace Behaviors.LichBoss.States {
    public class AttackRitual : State {

        private LichBossController controller;
        private LichBossHelper helper;

        private float endAttackCooldown;

        public AttackRitual(LichBossController controller) : base("AttackRitual") {
            this.controller = controller;
            this.helper = controller.helper;
        }

        public override void Enter() {
            base.Enter();

            // Pause damage
            controller.thisLife.isVulnerable = false;

            // Set variables
            endAttackCooldown = controller.attackRitualDuration;

            // Update animator
            controller.thisAnimator.SetTrigger("tAttackRitual");

            // Schedule attack
            helper.StartStateCoroutine(
                ScheduleAttack(controller.attackRitualDelay)
            );

            // Create effect
            Object.Instantiate(
                controller.preRitualPrefab,
                controller.transform.position,
                controller.preRitualPrefab.transform.rotation
            );
        }

        public override void Exit() {
            base.Exit();
            helper.ClearStateCoroutines();

            // Resume damage
            controller.thisLife.isVulnerable = true;
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

            // Create effect
            Object.Instantiate(
                controller.ritualPrefab,
                controller.staffBottom.position,
                controller.ritualPrefab.transform.rotation
            );

            // Damage player
            if (helper.GetDistanceToPlayer() <= controller.distanceToRitual) {
                var playerLife = GameManager.Instance.player.GetComponent<LifeScript>();
                playerLife.InflictDamage(controller.gameObject, controller.attackDamage);
            }
            
            // Spawn Creatures
            if (helper.HasLowHealth()) {
                helper.SpawnCreatures();
            }
        }

    }
}
