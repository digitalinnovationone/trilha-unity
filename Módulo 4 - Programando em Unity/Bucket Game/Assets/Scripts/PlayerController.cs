using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Ensure game is active
        if(!GameManager.Instance.isGameActive) {
            return;
        }

        // Get input values
        bool isPressingLeft = Input.GetKey(KeyCode.LeftArrow);
        bool isPressingRight = Input.GetKey(KeyCode.RightArrow);

        // Abort if no keys are pressed, or both are pressed at the same time
        if(isPressingLeft == isPressingRight) {
            return;
        }
        
        // Move player
        float movement = speed * Time.deltaTime;
        if(isPressingLeft) {
            movement *= -1f;
        }
        transform.position += new Vector3(movement, 0, 0);

        // Limit player boundaries
        float movementLimit = GameManager.Instance.gameWidth / 2;
        if(transform.position.x < -movementLimit) {
            transform.position = new Vector3(-movementLimit, transform.position.y, transform.position.z);
        } else if(transform.position.x > movementLimit) {
            transform.position = new Vector3(movementLimit, transform.position.y, transform.position.z);
        }
    }
}
