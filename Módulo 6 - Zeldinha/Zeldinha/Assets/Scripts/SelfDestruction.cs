using UnityEngine;

public class SelfDestruction : MonoBehaviour {

    public float delay;

    private void Start() {
        Destroy(gameObject, delay);
    }

}
