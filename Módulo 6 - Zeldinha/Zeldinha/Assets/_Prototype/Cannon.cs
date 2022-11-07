using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {

    public GameObject prefab;
    public float interval;
    public float impulse;
    
    private float cooldown;

    // Start is called before the first frame update
    void Start() {
        cooldown = interval;
    }

    // Update is called once per frame
    void Update() {
        if ((cooldown -= Time.deltaTime) < 0) {
            cooldown = interval;
            var projectile = Instantiate(prefab, transform.position, transform.rotation);
            var impulseVector = projectile.transform.rotation * Vector3.forward * impulse;
            projectile.GetComponent<Rigidbody>().AddForce(impulseVector, ForceMode.Impulse);
            Destroy(projectile, 6);
        }
    }

}
