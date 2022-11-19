using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightIntensityByScale : MonoBehaviour {
    private Light thisLight;
    private float originalIntensity;
    
    void Awake() {
        thisLight = GetComponent<Light>();
        originalIntensity = thisLight.intensity;
    }
    
    void Update() {
        var lossyScaleX = transform.lossyScale.x;
        thisLight.intensity = originalIntensity * lossyScaleX;
    }
}
