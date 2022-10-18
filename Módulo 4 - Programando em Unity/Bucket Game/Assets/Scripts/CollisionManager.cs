using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision other) {
        // Ensure game is active
        if(!GameManager.Instance.isGameActive) {
            return;
        }

        // Check if other object is a ball
        if(other.gameObject.CompareTag("Ball")) {

            // Increment score
            GameManager.Instance.score++;
            Debug.Log("Score: " + GameManager.Instance.score);

            // Destroy object
            Destroy(other.gameObject);
        }
    }
}
