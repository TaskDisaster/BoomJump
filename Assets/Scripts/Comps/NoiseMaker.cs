using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMaker : MonoBehaviour
{
    #region Variables
    public float volumeDistance;    // Distance of noise
    public float decayPerFrame;     // How much the volume decreases per frame
    #endregion

    public void Start()
    {

    }

    public void Update()
    {
        // Decay volume so I don't have to
        if (volumeDistance > 0)
        {
            volumeDistance -= decayPerFrame;
        }
    }

}
