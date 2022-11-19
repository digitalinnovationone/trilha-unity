using UnityEngine;

public class FaceCamera : MonoBehaviour {

    private Camera thisCamera;

    private void Start() {
        thisCamera = Camera.main;
    }

    private void Update() {
        transform.LookAt(thisCamera.transform);
    }

}
