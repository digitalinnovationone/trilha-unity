using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionHandler : MonoBehaviour {

    private Interaction currentInteraction;
    private readonly float scanInterval = 0.5f;
    private float scanCooldown = 0;

    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {
        // Scan objects
        if ((scanCooldown -= Time.deltaTime) <= 0f) {
            scanCooldown = scanInterval;
            ScanObjects();
        }

        // Process input
        if (Input.GetKeyDown(KeyCode.E)) {
            if (currentInteraction != null) {
                currentInteraction.Interact();
                ScanObjects();
            }
        }
    }

    private void ScanObjects() {
        Interaction nearestInteraction = GetNearestInteraction(transform.position);
        if (nearestInteraction != currentInteraction) {
            currentInteraction?.SetActive(false);
            nearestInteraction?.SetActive(true);
            currentInteraction = nearestInteraction;
        }
    }

    public Interaction GetNearestInteraction(Vector3 position) {
        // Create cache
        float closestDst = -1;
        Interaction closestInteraction = null;

        // Iterate through objects
        var interactionList = GameManager.Instance.interactionList;
        foreach (Interaction interaction in interactionList) {
            var dst = (interaction.transform.position - position).magnitude;
            var isAvailable = interaction.IsAvailable();
            var isCloseEnough = dst <= interaction.radius;
            var isCacheInvalid = closestDst < 0;
            if (isCloseEnough && isAvailable) {
                if (isCacheInvalid || dst < closestDst) {
                    closestDst = dst;
                    closestInteraction = interaction;
                }
            }
        }

        return closestInteraction;
    }

}
