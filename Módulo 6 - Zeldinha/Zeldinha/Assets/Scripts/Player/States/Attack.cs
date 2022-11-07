using UnityEngine;

public class Attack : State {

    private PlayerController controller;
    
    public int stage = 1;
    private float stateTime;
    private bool firstFixedUpdate;
    
    public Attack(PlayerController controller) : base("Attack") {
        this.controller = controller;
    }

    public override void Enter() {
        base.Enter();
        
        // ERROR: Invalid stage
        if (stage <= 0 || stage > controller.attackStages) {
            controller.stateMachine.ChangeState(controller.idleState);
            return;
        }

        // Reset variables
        stateTime = 0;
        firstFixedUpdate = true;
        
        // Set animator trigger
        controller.thisAnimator.SetTrigger("tAttack" + stage);
        
        // Toggle hitbox
        controller.swordHitbox.SetActive(true);
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
        
        // Exit after time
        if (IsStageExpired()) {
            controller.stateMachine.ChangeState(controller.idleState);
            return;
        }
    }

    public override void LateUpdate() {
        base.LateUpdate();
    }

    public override void FixedUpdate() {
        base.FixedUpdate();

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

    public bool CanSwitchStages() {
        // Get attack variables
        var isLastState = stage == controller.attackStages;
        var stageDuration = controller.attackStageDurations[stage - 1];
        var stageMaxInterval = isLastState ? 0 : controller.attackStageMaxIntervals[stage - 1];
        var maxStageDuration = stageDuration + stageMaxInterval;

        // Reply
        return !isLastState && stateTime >= stageDuration && stateTime <= maxStageDuration;
    }

    public bool IsStageExpired() {
        // Get attack variables
        var isLastState = stage == controller.attackStages;
        var stageDuration = controller.attackStageDurations[stage - 1];
        var stageMaxInterval = isLastState ? 0 : controller.attackStageMaxIntervals[stage - 1];
        var maxStageDuration = stageDuration + stageMaxInterval;

        // Reply
        return stateTime > maxStageDuration;
    }
}
