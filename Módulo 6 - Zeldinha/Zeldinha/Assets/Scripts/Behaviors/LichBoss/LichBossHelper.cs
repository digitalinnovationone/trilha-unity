using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Behaviors.LichBoss {
    public class LichBossHelper {

        private LichBossController controller;

        public LichBossHelper(LichBossController controller) {
            this.controller = controller;
        }

        public float GetDistanceToPlayer() {
            var player = GameManager.Instance.player;
            var playerPosition = player.transform.position;
            var origin = controller.transform.position;
            var positionDifference = playerPosition - origin;
            var distance = positionDifference.magnitude;
            return distance;
        }

        public bool HasLowHealth() {
            var life = controller.thisLife;
            var lifeRate = (float) life.health / (float) life.maxHealth;
            return lifeRate <= controller.lowHealthThreshold;
        }

        public void StartStateCoroutine(IEnumerator enumerator) {
            controller.StartCoroutine(enumerator);
            controller.stateCoroutines.Add(enumerator);
        }

        public void ClearStateCoroutines() {
            foreach (var enumerator in controller.stateCoroutines) {
                controller.StopCoroutine(enumerator);
            }
            controller.stateCoroutines.Clear();
        }

        public void SpawnCreatures() {
            // Cache vars
            var bossPosition = controller.transform.position;
            var playerPosition = GameManager.Instance.player.transform.position;
            var safetyRadiusSqr = Mathf.Pow(controller.spawnCreatureSafetyRadius, 2);

            // Count
            var minAmount = Mathf.RoundToInt(controller.spawnCreatureAmount.x);
            var maxAmount = Mathf.RoundToInt(controller.spawnCreatureAmount.y);
            int amount = Random.Range(minAmount, maxAmount + 1);
            amount = Mathf.Clamp(amount, 0, controller.creatureSpawners.Count);

            // Get temporary transforms
            List<Transform> transforms = new List<Transform>(controller.creatureSpawners);

            // Iterate
            for (int i = 0; i < amount; i++) {
                // Get transform
                var transformIndex = Random.Range(0, transforms.Count);
                var transform = transforms[transformIndex];
                transforms.RemoveAt(transformIndex);
                var position = transform.position;

                // Check if it's valid
                var dstToBoss = (bossPosition - position).sqrMagnitude;
                var dstToPlayer = (playerPosition - position).sqrMagnitude;
                var isValid = Mathf.Min(dstToBoss, dstToPlayer) > safetyRadiusSqr;
                if (!isValid) continue;
                
                // Spawn
                SpawnCreatureAt(transform.position);
            }
        }

        private void SpawnCreatureAt(Vector3 position) {
            // Creature
            var creaturePrefab = controller.creaturePrefab;
            if (creaturePrefab != null) {
                var creatureRotation = creaturePrefab.transform.rotation;
                Object.Instantiate(creaturePrefab, position, creatureRotation);
            }

            // Effect
            var effectPrefab = controller.creatureEffectPrefab;
            if (effectPrefab != null) {
                var effectRotation = effectPrefab.transform.rotation;
                Object.Instantiate(effectPrefab, position, effectRotation);
            }
        }

    }
}
