using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    // Awake
    public void Awake()
    {
        Destroy(gameObject);
    }
}
