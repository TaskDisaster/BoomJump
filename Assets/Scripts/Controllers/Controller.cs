using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    #region Variables
    public Pawn pawn;
    public int lives = 3;
    public bool isDead = false;

    #endregion Variables
    // Start is called before the first frame update
    public abstract void Start();

    // Update is called once per frame
    public abstract void Update();
}
