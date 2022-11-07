using System;
using UnityEngine;

public class ShieldHitboxScript : MonoBehaviour {
    public PlayerController playerController;
    
    private void OnTriggerEnter(Collider collision) {
        playerController.OnShieldCollisionEnter(collision);
    }

}
