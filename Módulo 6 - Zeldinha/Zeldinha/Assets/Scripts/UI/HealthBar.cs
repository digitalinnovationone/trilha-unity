using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    private int maxHealth;
    private int health;
    [SerializeField] private Slider slider;

    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {
        slider.value = health;
    }

    public void SetMaxHealth(int maxHealth) {
        this.maxHealth = maxHealth;
        this.health = maxHealth;
        slider.maxValue = maxHealth;
    }

    public void SetHealth(int health) {
        this.health = health;
    }

}
