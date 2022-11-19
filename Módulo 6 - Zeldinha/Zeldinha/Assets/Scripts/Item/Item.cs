using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item {
    
    [CreateAssetMenu]
    public class Item : ScriptableObject {

        public ItemType itemType;
        public string displayName;
        public GameObject objectPrefab;

    }
}
