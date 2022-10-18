using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public List<GameObject> prefabs;
    public float interval = 1f;
    private float cooldown = 0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Update cooldown
        cooldown -= Time.deltaTime;
        if(cooldown <= 0f) {

            // Shoot ball
            ShootBall();

            // Reset cooldown
            cooldown = interval;
        }
    }

    private void ShootBall()
    {
        var prefab = prefabs[Random.Range(0, prefabs.Count)];
        Instantiate(prefab, transform);
    }
}
