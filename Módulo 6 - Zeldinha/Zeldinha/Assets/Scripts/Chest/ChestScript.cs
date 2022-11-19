using System;
using EventArgs;
using Item;
using UnityEngine;
using UnityEngine.Events;

namespace Chest {

    public class ChestScript : MonoBehaviour {

        public Interaction interaction;
        public GameObject itemHolder;
        public Item.Item item;
        public GameObject openEffect;
        public ChestOpenEvent onOpen = new();
        
        private Animator thisAnimator;
        
        private void Awake() {
            thisAnimator = GetComponent<Animator>();
        }

        void Start() {
            interaction.OnInteraction += OnInteraction;
            interaction.SetActionText("Open Chest");
        }

        void Update() {}

        private void OnInteraction(object sender, InteractionEventArgs args) {
            if (item == null) {
                Debug.Log("Jogador acabou de interagir com o baú, mas está vazio.");
            } else {
                Debug.Log("Jogador acabou de interagir com o baú, contendo item " + item.displayName + "!");
            }

            // Disable interaction
            interaction.SetAvailable(false);

            // Update animator
            thisAnimator.SetTrigger("tOpen");

            // Create item object
            var itemObjectPrefab = item.objectPrefab;
            var position = itemHolder.transform.position;
            var rotation = itemObjectPrefab.transform.rotation;
            var itemObject = Instantiate(itemObjectPrefab, position, rotation);
            itemObject.transform.SetParent(itemHolder.transform);

            // Update inventory
            var itemType = item.itemType;
            if (itemType == ItemType.Key) {
                GameManager.Instance.keys++;
            } else if (itemType == ItemType.BossKey) {
                GameManager.Instance.hasBossKey = true;
            } else if (itemType == ItemType.Potion) {
                var player = GameManager.Instance.player;
                var playerLife = player.GetComponent<LifeScript>();
                playerLife.Heal();
            }
            
            // Call events
            onOpen?.Invoke(gameObject);
            
            // Update UI
            var gameplayUI = GameManager.Instance.gameplayUI;
            gameplayUI.AddObject(itemType);
            
            // Effect
            if (openEffect != null) {
                Instantiate(openEffect, transform.position, openEffect.transform.rotation);
            }
        }

    }

    [Serializable] public class ChestOpenEvent : UnityEvent<GameObject> {}

}
