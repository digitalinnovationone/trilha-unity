using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScript : MonoBehaviour
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
        
            // Print message
            Debug.Log("GAME OVER!!!");

            // Destroy object
            Destroy(other.gameObject);

            // End game
            GameManager.Instance.isGameActive = false;
        }
    }
}
