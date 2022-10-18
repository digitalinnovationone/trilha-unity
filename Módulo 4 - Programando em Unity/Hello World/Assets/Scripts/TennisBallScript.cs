using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TennisBallScript : MonoBehaviour
{
    private static readonly float DestructionThreshold = -10f;

    private Rigidbody myRigidbody;
    public Vector2 force = new Vector2(10, 10);

    // Start is called before the first frame update
    void Start()
    {
        // Calculate force to be applied
        float forceAmountY = Random.Range(force.x, force.y);
        float forceAmountX = forceAmountY * Random.Range(-0.05f, 0.05f);
        float forceAmountZ = forceAmountY * Random.Range(-0.05f, 0.05f);
        Vector3 forceVector = new Vector3(forceAmountX, forceAmountY, forceAmountZ);

        // Apply force to ball
        myRigidbody = gameObject.GetComponent<Rigidbody>();
        myRigidbody.AddForce(forceVector, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        float y = transform.position.y;
        if(y <= DestructionThreshold) {
            Destroy(gameObject);
        }
    }
}
