using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    #region Variables
    public float currentHealth;
    public float maxHealth;
    public Image healthBar;
    public AudioClip audioClip;

    #endregion Variables

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;    
    }

    public void TakeDamage(float amount, Pawn source)
    {
        // Take amount away from health
        currentHealth = currentHealth - amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log(source.name + " dealt " + amount + " damage to " + gameObject.name);

        // Set the healthBar to the relevant fill
        healthBar.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            Die(source);
        }
    }

    public void Die(Pawn source)
    {
        Debug.Log(source.name + " destroyed " + gameObject.name);

        if (audioClip != null)
        {
            AudioManager.Instance.Play(audioClip);
        }

        // Destroy said object
        Destroy(gameObject);
    }
}
