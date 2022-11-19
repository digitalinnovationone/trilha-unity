using System.Collections;
using UnityEngine;

namespace Behaviors.MeleeCreature.States {
    public class Attack : State {

        private MeleeCreatureController controller;
        private MeleeCreatureHelper helper;
        private float stateTime;
        private IEnumerator attackCoroutine;

        public Attack(MeleeCreatureController controller) : base("Attack") {
            this.controller = controller;
            this.helper = controller.helper;
        }

        public override void Enter() {
            base.Enter();

            // Set variables
            stateTime = 0;

            // Schedule attack
            attackCoroutine = ScheduleAttack();
            controller.StartCoroutine(attackCoroutine);
            
            // Update animator
            controller.thisAnimator.SetTrigger("tAttack");
        }

        public override void Exit() {
            base.Exit();

            // Cancel attack
            if (attackCoroutine != null) {
                controller.StopCoroutine(attackCoroutine);
            }
        }

        public override void Update() {
            base.Update();
            stateTime += Time.deltaTime;
            
            // Face player
            if (stateTime <= controller.damageDelay) {
                helper.FacePlayer();
            }
            
            // End attack
            if (stateTime >= controller.attackDuration) {
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

        private IEnumerator ScheduleAttack() {
            yield return new WaitForSeconds(controller.damageDelay);
            PerformAttack();
        }

        private void PerformAttack() {
            // Get variables
            var origin = controller.transform.position;
            var direction = controller.transform.rotation * Vector3.forward;
            var radius = controller.attackRadius;
            var maxDistance = controller.attackSphereRadius;

            // OverlapSphere
            var attackPosition = direction * radius + origin;
            var layerMask = LayerMask.GetMask("Player");
            Collider[] colliders = Physics.OverlapSphere(attackPosition, maxDistance, layerMask);
            foreach (var collider in colliders) {
                var hitObject = collider.gameObject;

                // Perform attack!
                var hitLifeScript = hitObject.GetComponent<LifeScript>();
                if (hitLifeScript != null) {
                    var attacker = controller.gameObject;
                    var attackDamage = controller.attackDamage;
                    hitLifeScript.InflictDamage(attacker, attackDamage);
                }
            }
        }

    }
}
