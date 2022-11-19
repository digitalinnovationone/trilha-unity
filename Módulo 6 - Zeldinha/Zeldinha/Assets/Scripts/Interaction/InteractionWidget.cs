using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionWidget : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI inputText;
    [SerializeField] private TextMeshProUGUI actionText;
    [SerializeField] private CanvasGroup canvasGroup;
    public Camera worldUiCamera;

    private string inputString = "E";
    private string actionString = "";
    private bool isVisible = false;
    
    // Start is called before the first frame update
    void Start() {
        // Reset text
        inputText.text = inputString;
        actionText.text = actionString;
        
        // Auto hide
        Hide();
    }

    // Update is called once per frame
    void Update() {
        // Visibility
        var targetAlpha = isVisible ? 1 : 0;
        var currentAlpha = canvasGroup.alpha;
        var newAlpha = Mathf.Lerp(currentAlpha, targetAlpha, 0.1f);
        canvasGroup.alpha = newAlpha;
        
        // Face camera
        transform.rotation = worldUiCamera.transform.rotation;
    }

    public void SetInputText(string text) {
        inputString = text;
        inputText.text = inputString;
    }
    
    public void SetActionText(string text) {
        actionString = text;
        actionText.text = actionString;
    }

    public void Show() {
        isVisible = true;
    }

    public void Hide() {
        isVisible = false;
    }
}
