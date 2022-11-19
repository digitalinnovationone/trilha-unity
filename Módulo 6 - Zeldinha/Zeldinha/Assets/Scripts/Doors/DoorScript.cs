using System.Collections;
using System.Collections.Generic;
using EventArgs;
using Item;
using UnityEngine;

public class DoorScript : MonoBehaviour {

    public Interaction interaction;
    public Item.Item requiredKey;
    public GameObject openEffect;
    
    private bool isOpen;
    private Animator thisAnimator;

    private void Awake() {
        thisAnimator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start() {
        interaction.OnInteraction += OnInteraction;
        interaction.SetActionText("Open Door");
    }

    // Update is called once per frame
    void Update() {
        // If door is still closed...
        if (!isOpen) {

            // Check if player has key
            var hasKey = false;
            if (requiredKey == null) {
                hasKey = true;
            } else if (requiredKey.itemType == ItemType.Key) {
                hasKey = GameManager.Instance.keys > 0;
            } else if (requiredKey.itemType == ItemType.BossKey) {
                hasKey = GameManager.Instance.hasBossKey;
            }

            // Toggle availability
            interaction.SetAvailable(hasKey);
        }
    }

    private void OnInteraction(object sender, InteractionEventArgs args) {
        Debug.Log("Jogador acabou de interagir com a porta!");

        if (!isOpen) {
            OpenDoor();
        } else {
            CloseDoor();
        }
    }

    private void OpenDoor() {
        // Set as open
        isOpen = true;

        // Take key
        if (requiredKey != null) {
            if (requiredKey.itemType == ItemType.Key) {
                GameManager.Instance.keys--;
            } else if (requiredKey.itemType == ItemType.BossKey) {
                GameManager.Instance.hasBossKey = false;
            }
        }

        // Update UI
        var gameplayUI = GameManager.Instance.gameplayUI;
        gameplayUI.RemoveObject(requiredKey.itemType);

        // Disable interaction
        interaction.SetAvailable(false);

        // Update animator
        thisAnimator.SetTrigger("tOpen");

        // Boss door
        var isBossDoor = requiredKey.itemType == ItemType.BossKey;
        if (isBossDoor) {
            GlobalEvents.Instance.InvokeBossDoorOpen(this, new BossDoorOpenArgs());
        }
        
        // Effect
        if (openEffect != null) {
            Instantiate(openEffect, transform.position, openEffect.transform.rotation);
        }
    }

    private void CloseDoor() {
        // Set as close
        isOpen = false;

        // Disable interaction
        // interaction.SetAvailable(false);

        // Update animator
        thisAnimator.SetTrigger("tClose");
    }

}
