using System.Collections;
using System.Collections.Generic;
using Item;
using UnityEngine;

public class GameplayUI : MonoBehaviour {

    [SerializeField] private Canvas thisCanvas;
    public HealthBar playerHealthBar;
    public HealthBar bossHealthBar;
    [SerializeField] private GameObject bossContainer;
    private bool isBossVisible = false;
    
    [Header("Objects")]
    [SerializeField] private GameObject objectContainer;
    [SerializeField] private GameObject normalKeyTemplate;
    [SerializeField] private GameObject bossKeyTemplate;
    private List<ObjectEntry> entries = new();

    public readonly float wakeUpTime = 4f;
    
    // Start is called before the first frame update
    void Start() {
        thisCanvas.enabled = false;
        bossContainer.SetActive(isBossVisible);

        StartCoroutine(EnableCanvas());
    }

    // Update is called once per frame
    void Update() {}

    public void ToggleBossBar(bool enabled) {
        isBossVisible = enabled;
        bossContainer.SetActive(enabled);
    }

    public void AddObject(ItemType type) {
        if (type != ItemType.Key && type != ItemType.BossKey) return;

        // Create widget
        var template = type == ItemType.Key ? normalKeyTemplate : bossKeyTemplate;
        var widget = Instantiate(template, template.transform);
        widget.SetActive(true);
        widget.transform.SetParent(objectContainer.transform);
        
        // Create entry
        ObjectEntry entry = new ObjectEntry {
            type = type,
            widget = widget
        };
        entries.Add(entry);
    }

    public void RemoveObject(ItemType type) {
        for (int i = 0; i < entries.Count; i++) {
            var entry = entries[i];
            if (entry.type == type) {
                Destroy(entry.widget);
                entries.RemoveAt(i);
                return;
            }
        }
    }

    private IEnumerator EnableCanvas() {
        yield return new WaitForSeconds(wakeUpTime);
        thisCanvas.enabled = true;
    }

    private struct ObjectEntry {
        public ItemType type;
        public GameObject widget;
    }

}
