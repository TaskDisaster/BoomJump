using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameManager : MonoBehaviour
{
    // Accessible anywhere Singleton
    public static GameManager Instance;

    // Variables

    [Header("Player")] // WHY DIDN'T I FIND THIS SOONER
    public GameObject playerControllerPrefab;
    public GameObject playerPawnPrefab;
    public Transform playerSpawnTransform;
    public Controller playerController1;

    [Header("Ai")] // WOULD'VE BEEN VERY USEFUL FOR THE GAMEMANAGER IN TANK GAME
    public bool doSpawn = true;
    public List<GameObject> aiTankPawnPrefabs = new List<GameObject>();
    public Transform aiSpawnTransform;
    public List<AIController> ai;

    [Header("Spawn Management")]
    public SpawnManager spawnManager;
    public GameObject spawnManagerPrefab;

    [Header("Game State Management")]
    public GameState gameState;
    public enum GameState { Title, MainMenu, Options, Credits, Gameplay, GameOver, Win };
    public GameObject TitleScreenStateObject;
    public GameObject MainMenuStateObject;
    public GameObject OptionsScreenStateObject;
    public GameObject CreditsScreenStateObject;
    public GameObject GameplayStateObject;
    public GameObject GameOverScreenStateObject;
    public GameObject WinScreenStateObject;
    public bool isFound = false;
    public GameObject resetManagerPrefab;
    public GameObject resetManager;
    public GameObject livesManager;
    public KeyCode functionKey;
    public bool timeToReset = false;
    public GameObject winTrigger;
    public bool won = false;

    [Header("Audio Tracks")]
    public AudioClip track1;
    public AudioClip track2;
    private bool isPlaying = false;

    // Awakes is called before Start()
    private void Awake()
    {
        // Check if Instance exists and destroy any copies
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        ai = new List<AIController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (TitleScreenStateObject != null)
        {
            // Sets to title screen first
            gameState = GameState.Title;
        }
        else
        {
            SpawnPlayer();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (TitleScreenStateObject != null)
        {
            // Control FSM
            MakeDecision();
        }
    }

    public void SpawnPlayer()
    {
        // Check if player transform is null, then set it
        if (playerSpawnTransform == null)
        {
            // Get a random player spawn transform from the spawn manager
            Transform spawnPoint = spawnManager.playerSpawnPoints[Random.Range(0, spawnManager.playerSpawnPoints.Count)];
            playerSpawnTransform = spawnPoint;
        }

        if (playerSpawnTransform != null)
        {
            // Sets the playerControllerObj to the playerController1
            GameObject newPlayerObj = Instantiate(playerControllerPrefab, Vector3.zero, Quaternion.identity);
            GameObject newPawnObj = Instantiate(playerPawnPrefab, playerSpawnTransform.position, playerSpawnTransform.rotation);

            Controller newController = newPlayerObj.GetComponent<Controller>();
            Pawn newPawn = newPawnObj.GetComponent<Pawn>();

            newController.pawn = newPawn;
            newPawn.controller = newController;
        }
    }

    public void RespawnPlayer(Controller player)
    {
        // If the controller has a null pawn
        if (player.pawn == null)
        {
            // Create a new pawn and assign it to the controller
            GameObject newPlayer = Instantiate(playerPawnPrefab, playerSpawnTransform.position, playerSpawnTransform.rotation);
            Pawn newPawn = newPlayer.GetComponent<Pawn>();

            UnityEngine.Debug.Log(newPawn + " " + " " + player);

            player.pawn = newPawn;
            newPawn.controller = playerController1;
        }
    }

    public void SpawnAi()
    {
        if (doSpawn == true)
        {
            if (aiSpawnTransform == null)
            {
                // Keep spawning enemies for the amount of spawns for them
                for (int i = 0; i < spawnManager.enemySpawnPoints.Count; i++)
                {
                    // Get a random enemy spawn transform from the map generator
                    EnemySpawnPoint spawnPoint = spawnManager.enemySpawnPoints[i];

                    aiSpawnTransform = spawnPoint.spawnPoint;

                    // Spawn
                    GameObject newAI = Instantiate(aiTankPawnPrefabs[Random.Range(0, aiTankPawnPrefabs.Count)], aiSpawnTransform.position, aiSpawnTransform.rotation);

                    // Hand over patrol points if there are any
                    AIController newAIController = newAI.GetComponent<AIController>();
                    foreach (Transform point in spawnPoint.patrolPoints)
                    {
                        newAIController.waypoints.Add(point);
                    }
                }
            }
        }
    }

    public void GetSpawns()
    {
        if (spawnManagerPrefab != null)
        {
            // Create spawnManager prefab
            GameObject newSpawnsPrefab = Instantiate(spawnManagerPrefab, Vector3.zero, Quaternion.identity);
            // Get the reference to the spawn manager
            spawnManager = newSpawnsPrefab.GetComponent<SpawnManager>();
        }
    }

    #region Game State Functions

    // The brain of setting gamestates
    public void MakeDecision()
    {
        switch (gameState)
        {
            case GameState.Title:

                ActivateTitleScreen();
                // Tanksition
                if (Input.GetKey(functionKey))
                {
                    ChangeState(GameState.MainMenu);
                }
                break;

            case GameState.MainMenu:
                // Play menu music
                if (!isPlaying)
                {
                    AudioManager.Instance.PlayMusic(track2);
                    isPlaying = true;
                }

                ActivateMainMenuScreen();
                break;

            case GameState.Options:
                ActivateOptionsScreen();
                break;

            case GameState.Credits:
                ActivateCreditsScreen();
                break;

            case GameState.Gameplay:
                // Play gameplay music
                if (isPlaying)
                {
                    AudioManager.Instance.PlayMusic(track1);
                    isPlaying = false;
                }

                ActivateGameplayState();
                // Check for transitions
                if (won == true)
                {
                    Instantiate(resetManagerPrefab, Vector3.zero, Quaternion.identity);
                    // Unlock cursor
                    Cursor.lockState = CursorLockMode.None;
                    // Make in invisible
                    Cursor.visible = true;
                    ChangeState(GameState.Win);
                }

                if (playerController1.isDead == true)
                {
                    Instantiate(resetManagerPrefab, Vector3.zero, Quaternion.identity);
                    ChangeState(GameState.GameOver);
                }
                break;

            case GameState.GameOver:
                AudioManager.Instance.Stop();
                isPlaying = false;
                ActivateGameOverScreen();
                break;

            case GameState.Win:
                AudioManager.Instance.Stop();
                isPlaying = false;
                ActivateWinScreen();
                break;

        }
    }

    public virtual void ChangeState(GameState newState)
    {
        // Change the current state
        gameState = newState;
    }
    public void ResetGame()
    {
        UnityEngine.Debug.LogWarning("Resetting");

        // Remove player
        playerController1.isDead = false;
        playerController1.lives = 3;
        Destroy(playerController1.gameObject);

        // Remove all enemies
        foreach (AIController enemy in ai)
        {
            if (enemy.gameObject != null)
            {
                Destroy(enemy.gameObject);
            }
        }
        aiSpawnTransform = null;

        // Destroy the resetManager
        Destroy(resetManager.gameObject);
        resetManager = null;
        isFound = false;
        timeToReset = false;
        won = false;
    }

    #region Deactivate State

    private void DeactivateAllStates()
    {
        // Deactivate all Game States
        TitleScreenStateObject.SetActive(false);
        MainMenuStateObject.SetActive(false);
        OptionsScreenStateObject.SetActive(false);
        CreditsScreenStateObject.SetActive(false);
        GameplayStateObject.SetActive(false);
        GameOverScreenStateObject.SetActive(false);
        WinScreenStateObject.SetActive(false);
    }

    public void DeactivateTitleScreen()
    {
        TitleScreenStateObject.SetActive(false);
    }

    public void DeactivateMainMenuScreen()
    {
        MainMenuStateObject.SetActive(false);
    }

    public void DeactivateOptionsScreen()
    {
        OptionsScreenStateObject.SetActive(false);
    }

    public void DeactivateGameplayState()
    {
        GameplayStateObject.SetActive(false);
    }

    public void DeactivateGameOverScreen()
    {
        GameOverScreenStateObject.SetActive(false);
    }

    public void DeactivateCreditsScreen()
    {
        CreditsScreenStateObject.SetActive(false);
    }

    public void DeactivateWinScreen()
    {
        WinScreenStateObject.SetActive(false);
    }

    #endregion Deactivate State

    #region Activate State
    public void ActivateTitleScreen()
    {
        // Deactivate all states
        DeactivateAllStates();

        gameState = GameState.Title;

        // Activate the title screen
        TitleScreenStateObject.SetActive(true);
    }

    public void ActivateMainMenuScreen()
    {
        // Deactivate all states
        DeactivateAllStates();

        // Resets the game if it's time to reset after playing once
        if (timeToReset)
        {
            ResetGame();
        }

        gameState = GameState.MainMenu;

        MainMenuStateObject.SetActive(true);
    }

    public void ActivateOptionsScreen()
    {
        DeactivateAllStates();
        gameState = GameState.Options;
        OptionsScreenStateObject.SetActive(true);
    }

    public void ActivateGameplayState()
    {
        // Deactivate all states
        DeactivateAllStates();

        // Check if the spawns are made
        if (!isFound)
        {
            GetSpawns();
            SpawnPlayer();
            SpawnAi();
            gameState = GameState.Gameplay;
            isFound = true;
            timeToReset = false;
        }
        else if (playerController1 != false)
        {
            WinTrigger win = winTrigger.GetComponent<WinTrigger>();
            win.owner = playerController1.pawn;
        }

        GameplayStateObject.SetActive(true);
    }

    public void ActivateGameOverScreen()
    {
        // Deactivate all states
        DeactivateAllStates();

        gameState = GameState.GameOver;

        GameOverScreenStateObject.SetActive(true);
    }

    public void ActivateCreditsScreen()
    {
        // Deactivate all states
        DeactivateAllStates();

        gameState = GameState.Credits;

        CreditsScreenStateObject.SetActive(true);
    }

    public void ActivateWinScreen()
    {
        // Deactivate all states
        DeactivateAllStates();

        gameState = GameState.Win;

        WinScreenStateObject.SetActive(true);
    }

    #endregion Activate

    #endregion Game State Functions
}
