using UnityEngine;

public class MeleeCreatureHelper {
    private MeleeCreatureController controller;
    
    public MeleeCreatureHelper(MeleeCreatureController controller) {
        this.controller = controller;
    }

    public float GetDistanceToPlayer() {
        var player = GameManager.Instance.player;
        var playerPosition = player.transform.position;
        var origin = controller.transform.position;
        var positionDifference = playerPosition - origin;
        var distance = positionDifference.magnitude;
        return distance;
    }
    
    public bool IsPlayerOnSight() {
        var player = GameManager.Instance.player;
        var playerPosition = player.transform.position;
        var origin = controller.transform.position;
        var positionDifference = playerPosition - origin;
        var direction = positionDifference.normalized;
        var distance = positionDifference.magnitude;
        var searchRadius = controller.searchRadius;
        
        // ERROR: Too far
        if (distance > searchRadius) {
            return false;
        }

        // ERROR: Found obstacle
        var layerMask = LayerMask.GetMask("Default", "Player");
        if (Physics.Raycast(origin, direction, out var hitInfo, searchRadius, layerMask)) {
            if (hitInfo.transform.gameObject != player) {
                return false;
            }
        }
        
        // All good!
        return true;
    }

    public void FacePlayer() {
        var transform = controller.transform;
        var player = GameManager.Instance.player;
        var vecToPlayer = player.transform.position - transform.position;
        vecToPlayer.y = 0;
        vecToPlayer.Normalize();
        var desiredRotation = Quaternion.LookRotation(vecToPlayer);
        var alpha = Time.deltaTime / controller.attackRotationTime;
        var newRotation = Quaternion.LerpUnclamped(transform.rotation, desiredRotation, alpha);
        transform.rotation = newRotation;
    }

}
