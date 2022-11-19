using EventArgs;
using UnityEngine;

namespace Behaviors.LichBoss.States {
    public class Dead : State {

        private LichBossController controller;
        private LichBossHelper helper;

        public Dead(LichBossController controller) : base("Dead") {
            this.controller = controller;
            this.helper = controller.helper;
        }

        public override void Enter() {
            base.Enter();

            // Pause damage
            controller.thisLife.isVulnerable = false;

            // Update animator
            controller.thisAnimator.SetTrigger("tDead");

            // Game won!!!
            GlobalEvents.Instance.InvokeGameWon(this, new GameWonArgs());
        }

        public override void Exit() {
            base.Exit();
        }

        public override void Update() {
            base.Update();
        }

        public override void LateUpdate() {
            base.LateUpdate();
        }

        public override void FixedUpdate() {
            base.FixedUpdate();
        }

    }
}
