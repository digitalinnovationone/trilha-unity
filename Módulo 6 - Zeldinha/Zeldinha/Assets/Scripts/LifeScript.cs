using System;
using EventArgs;
using UnityEngine;

public class LifeScript : MonoBehaviour {

    public event EventHandler<DamageEventArgs> OnDamage;
    public event EventHandler<HealEventArgs> OnHeal;

    public int maxHealth;
    public int health;
    public bool isVulnerable = true;

    public GameObject healingPrefab;
    public GameObject hitPrefab;

    public delegate bool CanInflictDamageDelegate(GameObject attacker, int damage);
    public CanInflictDamageDelegate canInflictDamageDelegate;
    
    void Start() {
        health = maxHealth;
    }

    public void InflictDamage(GameObject attacker, int damage) {
        if (isVulnerable) {

            // Can inflict damage?
            bool? canInflictDamage = canInflictDamageDelegate?.Invoke(attacker, damage);
            if (canInflictDamage.HasValue && canInflictDamage.Value == false) return;
            
            // Inflict damage
            health -= damage;
            OnDamage?.Invoke(this, new DamageEventArgs {
                attacker = attacker,
                damage = damage
            });
            
            // Create effect
            if (hitPrefab != null) {
                Instantiate(hitPrefab, transform.position, hitPrefab.transform.rotation);
            }
        }
    }

    public void Heal() {
        health = maxHealth;

        // Create effect
        if (healingPrefab != null) {
            var effect = Instantiate(healingPrefab, transform.position, healingPrefab.transform.rotation);
            effect.transform.SetParent(transform);
        }
        
        // Propagate event
        OnHeal?.Invoke(this, new HealEventArgs());
    }

    public bool IsDead() {
        return health <= 0f;
    }
    
    public  interface IInflictDamageCondition{
        bool CanDealDamage(GameObject attacker, int damage);
    }

}
