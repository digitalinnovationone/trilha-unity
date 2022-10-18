using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public List<GameObject> prefabs;
    public Vector3 originPoint = new Vector3(0, 0, 0);
    public float interval = 1;
    private float cooldown = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Ensure game is active
        if(!GameManager.Instance.isGameActive) {
            return;
        }

        // Spawn balls according to interval
        cooldown -= Time.deltaTime;
        if(cooldown <= 0f) {
            cooldown = interval;
            SpawnBall();
        }
    }

    private void SpawnBall() {
        // Get Prefab
        int prefabIndex = Random.Range(0, prefabs.Count);
        GameObject prefab = prefabs[prefabIndex];

        // Get position
        float gameWidth = GameManager.Instance.gameWidth;
        float xOffset = Random.Range(-gameWidth / 2f, gameWidth / 2f);
        Vector3 position = originPoint + new Vector3(xOffset, 0, 0);

        // Get rotation
        Quaternion rotation = prefab.transform.rotation;

        // Spawn Ball
        Instantiate(prefab, position, rotation);
    }
}
