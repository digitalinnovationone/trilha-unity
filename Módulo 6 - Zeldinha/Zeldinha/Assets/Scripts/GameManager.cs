using System;
using System.Collections;
using System.Collections.Generic;
using BossBattle;
using Item;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    // Singleton
    public static GameManager Instance {get; private set;}

    [HideInInspector] public bool isGameOver; 
    [HideInInspector] public bool isGameWon; 
        
    // Interaction
    public GameObject player;
    public List<Interaction> interactionList;
    // Rendering
    [Header("Rendering")]
    public Camera worldUiCamera;
    
    // Physics
    [Header("Physics")]
    [SerializeField] public LayerMask groundLayer;
    
    // Inventory
    [Header("Inventory")]
    public int keys;
    public bool hasBossKey;

    // Boss
    [Header("Boss")]
    public GameObject boss;
    public GameObject bossBattleParts;
    public BossBattleHandler bossBattleHandler;
    public GameObject bossDeathSequence;
    
    // Music
    [Header("Music")]
    public AudioSource gameplayMusic;
    public AudioSource bossMusic;
    public AudioSource ambienceMusic;
    
    // UI
    [Header("UI")]
    public GameplayUI gameplayUI;
    
    void Awake() {
        // Singleton
        if(Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }

    private void Start() {
        // Boss battle handler
        bossBattleHandler = new BossBattleHandler();
        
        // Play music
        var musicTargetVolume = gameplayMusic.volume;
        gameplayMusic.volume = 0;
        gameplayMusic.Play();
        StartCoroutine(FadeAudioSource.StartFade(gameplayMusic, musicTargetVolume, 1f));
        
        // Play ambience
        var ambienceTargetVolume = ambienceMusic.volume;
        ambienceMusic.volume = 0;
        ambienceMusic.Play();
        StartCoroutine(FadeAudioSource.StartFade(ambienceMusic, ambienceTargetVolume, 1f));
        
        // Listen to OnGameOver
        GlobalEvents.Instance.OnGameOver += (sender, args) => isGameOver = true;
        GlobalEvents.Instance.OnGameWon += (sender, args) => isGameWon = true;
    }

    void Update() {
        bossBattleHandler.Update();
    }

}
