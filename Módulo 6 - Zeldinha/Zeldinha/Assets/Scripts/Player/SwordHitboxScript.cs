using UnityEngine;

public class SwordHitboxScript : MonoBehaviour {
    public PlayerController playerController;
    
    private void OnTriggerEnter(Collider other) {
        playerController.OnSwordCollisionEnter(other);
    }

}
