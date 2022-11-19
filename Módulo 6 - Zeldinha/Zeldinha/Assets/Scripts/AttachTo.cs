using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachTo : MonoBehaviour {

    public GameObject objectToFollow;
    
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if (objectToFollow != null) {
            transform.position = objectToFollow.transform.position;
        }
    }
}
