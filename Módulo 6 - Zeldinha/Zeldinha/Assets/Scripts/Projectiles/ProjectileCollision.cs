using UnityEngine;

namespace Projectiles {
    public class ProjectileCollision : MonoBehaviour {
        public GameObject hitEffect;
        
        [HideInInspector] public GameObject attacker;
        [HideInInspector] public int damage;
        
        private void OnCollisionEnter(Collision collision) {
            // Process player collision
            var hitObject = collision.gameObject;
            var hitLayer = hitObject.layer;
            var collidedWithPlayer = hitLayer == LayerMask.NameToLayer("Player");
            if (collidedWithPlayer) {
                var hitLife = hitObject.GetComponent<LifeScript>();
                if (hitLife != null) {
                    hitLife.InflictDamage(attacker, damage);
                }
            }

            // Create effect
            if (hitEffect != null) {
                var effect = Instantiate(hitEffect, transform.position, hitEffect.transform.rotation);
                Destroy(effect, 10);
            }
            
            // Destroy projectile
            Destroy(gameObject);
        }

    }
}
