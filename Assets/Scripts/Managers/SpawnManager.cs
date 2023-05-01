using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    #region Variables
    public List<Transform> playerSpawnPoints = new List<Transform>();
    public List<EnemySpawnPoint> enemySpawnPoints = new List<EnemySpawnPoint>();
    #endregion Variables

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Get all player spawn points
        FindAllPlayerSpawnPoints();

        // Get all enemy spawn points
        FindAllEnemySpawnPoints();
    }


    public void FindAllPlayerSpawnPoints()
    {
        PlayerSpawnPoint[] PlayerSpawnPoints = FindObjectsOfType<PlayerSpawnPoint>();
        foreach (PlayerSpawnPoint PlayerSpawnPoint in PlayerSpawnPoints)
        {
            // Add its transform to spawnPoints list
            playerSpawnPoints.Add(PlayerSpawnPoint.transform);
        }
    }

    // Find all enemy spawn places
    public void FindAllEnemySpawnPoints()
    {
        EnemySpawnPoint[] EnemySpawnPoints = FindObjectsOfType<EnemySpawnPoint>();
        foreach (EnemySpawnPoint EnemySpawnPoint in EnemySpawnPoints)
        {
            // Add the transform to list
            enemySpawnPoints.Add(EnemySpawnPoint);
        }
    }
}
