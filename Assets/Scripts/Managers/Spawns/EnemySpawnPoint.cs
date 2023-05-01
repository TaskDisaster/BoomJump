using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    public Transform spawnPoint;
    public List<Transform> patrolPoints = new List<Transform>();

    public void Awake()
    {
        spawnPoint = gameObject.transform;
    }
}
