using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    // State Machine
    public StateMachine stateMachine;
    public Idle idleState;
    public Walking walkingState;
    public Jump jumpState;
    public Attack attackState;
    public Defend defendState;
    public Dead deadState;

    // Components
    [HideInInspector] public Rigidbody thisRigidbody;
    [HideInInspector] public Animator thisAnimator;

    // Movement
    [Header("Movement")]
    public float movementSpeed = 10;
    public float maxSpeed = 10;
    [HideInInspector] public Vector2 movementVector;

    // Jump
    [Header("Jump")]
    public float jumpPower = 10;
    public float jumpMovementFactor = 1f;
    [HideInInspector] public bool hasJumpInput;

    // Slope
    [Header("Slope")]
    public float maxSlopeAngle = 45;
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool isOnSlope;
    [HideInInspector] public Vector3 slopeNormal;

    // Attack
    [Header("Attack")] 
    public int attackStages;
    public List<float> attackStageDurations;
    public List<float> attackStageMaxIntervals;
    public List<float> attackStageIpulses;
    public GameObject swordHitbox;
    public float swordKnockbackImpulse = 10;
    
    // Defend
    [Header("Defend")] 
    public GameObject shieldHitbox;
    public float shieldKnockbackImpulse = 10;
    [HideInInspector] public bool hasDefenseInput;

    void Awake() {
        thisRigidbody = GetComponent<Rigidbody>();
        thisAnimator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start() {
        // StateMachine and its states
        stateMachine = new StateMachine();
        idleState = new Idle(this);
        walkingState = new Walking(this);
        jumpState = new Jump(this);
        attackState = new Attack(this);
        defendState = new Defend(this);
        deadState = new Dead(this);
        stateMachine.ChangeState(idleState);
        
        // Toggle hitboxes
        swordHitbox.SetActive(false);
        shieldHitbox.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        // Create input vector
        bool isUp = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        bool isDown = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        bool isLeft = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        bool isRight = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
        float inputX = isRight ? 1 : isLeft ? -1 : 0;
        float inputY = isUp ? 1 : isDown ? -1 : 0;
        movementVector = new Vector2(inputX, inputY);
        hasJumpInput = Input.GetKey(KeyCode.Space);
        
        // CHeck defense input
        hasDefenseInput = Input.GetMouseButton(1);

        // Update Animator
        float velocity = thisRigidbody.velocity.magnitude;
        float velocityRate = velocity / maxSpeed;
        thisAnimator.SetFloat("fVelocity", velocityRate);

        // Physic updates
        DetectGround();
        DetectSlope();
        
        // StateMachine
        stateMachine.Update();
    }

    void LateUpdate() {
        // StateMachine
        stateMachine.LateUpdate();
    }

    void FixedUpdate() {
        // Apply gravity
        Vector3 gravityForce = Physics.gravity * (isOnSlope ? 0.25f : 1f);
        thisRigidbody.AddForce(gravityForce, ForceMode.Acceleration);
        
        // Limit speed
        LimitSpeed();
        
        // StateMachine
        stateMachine.FixedUpdate();
    }

    public void OnSwordCollisionEnter(Collider other) {
        var otherObject = other.gameObject;
        var otherRigidbody = otherObject.GetComponent<Rigidbody>();
        var isTarget = otherObject.layer == LayerMask.NameToLayer("Target");
        if (isTarget && otherRigidbody != null) {
            var positionDiff = otherObject.transform.position - gameObject.transform.position;
            var impulseVector = new Vector3(positionDiff.normalized.x, 0, positionDiff.normalized.z);
            impulseVector *= swordKnockbackImpulse;
            otherRigidbody.AddForce(impulseVector, ForceMode.Impulse);
        }
    }

    public void OnShieldCollisionEnter(Collider other) {
        var otherObject = other.gameObject;
        var otherRigidbody = otherObject.GetComponent<Rigidbody>();
        var isTarget = true;
        if (isTarget && otherRigidbody != null) {
            var positionDiff = otherObject.transform.position - gameObject.transform.position;
            var impulseVector = new Vector3(positionDiff.normalized.x, 0, positionDiff.normalized.z);
            impulseVector *= shieldKnockbackImpulse;
            otherRigidbody.AddForce(impulseVector, ForceMode.Impulse);
        }
    }
    
    public Quaternion GetForward() {
        Camera camera = Camera.main;
        float eulerY = camera.transform.eulerAngles.y;
        return Quaternion.Euler(0, eulerY, 0);
    }

    public void RotateBodyToFaceInput(float alpha = 0.225f) {
        if(movementVector.IsZero()) return;

        // Calculate rotation
        Camera camera = Camera.main;
        Vector3 inputVector = new Vector3(movementVector.x, 0, movementVector.y);
        Quaternion q1 = Quaternion.LookRotation(inputVector, Vector3.up);
        Quaternion q2 = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0);
        Quaternion toRotation = q1 * q2;
        Quaternion newRotation = Quaternion.LerpUnclamped(transform.rotation, toRotation, alpha);

        // Apply rotation
        thisRigidbody.MoveRotation(newRotation);
    }

    public bool AttemptToAttack() {
        if (Input.GetMouseButtonDown(0)) {
            var isAttacking = stateMachine.currentStateName == attackState.name;
            var canAttack = !isAttacking || attackState.CanSwitchStages();
            if (canAttack) {
                var attackStage = isAttacking ? (attackState.stage + 1) : 1;
                attackState.stage = attackStage;
                stateMachine.ChangeState(attackState);
                return true;
            }
        }
        return false;
    }
    
    private void DetectGround() {
        // Reset flag
        isGrounded = false;

        // Detect ground
        Vector3 origin = transform.position;
        Vector3 direction = Vector3.down;
        float maxDistance = 0.1f;
        LayerMask groundLayer = GameManager.Instance.groundLayer;
        if (Physics.Raycast(origin, direction, maxDistance, groundLayer)) {
            isGrounded = true;
        }
    }
    
    private void DetectSlope() {
        // Reset flag
        isOnSlope = false;
        slopeNormal = Vector3.zero;
        
        // Detect ground
        Vector3 origin = transform.position;
        Vector3 direction = Vector3.down;
        float maxDistance = 0.2f;
        if (Physics.Raycast(origin, direction, out var slopeHitInfo, maxDistance)) {
            float angle = Vector3.Angle(Vector3.up, slopeHitInfo.normal);
            isOnSlope = angle < maxSlopeAngle && angle != 0;
            slopeNormal = isOnSlope ? slopeHitInfo.normal : Vector3.zero;
        }
    }

    private void LimitSpeed() {
        Vector3 flatVelocity = new Vector3(thisRigidbody.velocity.x, 0, thisRigidbody.velocity.z);
        if (flatVelocity.magnitude > maxSpeed) {
            Vector3 limitedVelocity = flatVelocity.normalized * maxSpeed;
            thisRigidbody.velocity = new Vector3(limitedVelocity.x, thisRigidbody.velocity.y, limitedVelocity.z);
        }
    }

}
