using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Image healthBar;
    public float damageInterval = 1f; 
    private bool isTakingDamage = false;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !isTakingDamage)
        {
            StartCoroutine(TakeDamageOverTime());
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            StopCoroutine(TakeDamageOverTime());
            isTakingDamage = false;
        }
    }

    private IEnumerator TakeDamageOverTime()
    {
        isTakingDamage = true;
        while (isTakingDamage)
        {
            TakeDamage(2); 
            yield return new WaitForSeconds(damageInterval); 
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        healthBar.fillAmount = (float)currentHealth / maxHealth;
    }
}
