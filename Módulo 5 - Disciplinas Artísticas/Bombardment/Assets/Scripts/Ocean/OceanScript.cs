using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other) {
        GameObject otherObject = other.gameObject;
        if(otherObject.CompareTag("Player")) {
            GameManager.Instance.EndGame();
        }
    }
}
