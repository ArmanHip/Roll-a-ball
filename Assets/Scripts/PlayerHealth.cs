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
    public GameObject playerDeathPanel;
    private Dictionary<string, Coroutine> damageCoroutines = new Dictionary<string, Coroutine>();

    public static bool IsPlayerDead = false;

    void Start()
    {
        IsPlayerDead = false;
        currentHealth = maxHealth;
        UpdateHealthBar();

        playerDeathPanel.SetActive(false);
        Time.timeScale = 1;
    }

    private void OnCollisionEnter(Collision collision)
    {
        string tag = collision.gameObject.tag;
        if ((tag == "Enemy" || tag == "EnemyFast") && !damageCoroutines.ContainsKey(tag))
        {
            int damage = tag == "Enemy" ? 2 : 1;
            Coroutine damageCoroutine = StartCoroutine(TakeDamageOverTime(damage));
            damageCoroutines[tag] = damageCoroutine;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        string tag = collision.gameObject.tag;
        if (damageCoroutines.ContainsKey(tag))
        {
            StopCoroutine(damageCoroutines[tag]);
            damageCoroutines.Remove(tag);
        }
    }

    private IEnumerator TakeDamageOverTime(int damage)
    {
        while (true)
        {
            TakeDamage(damage);
            yield return new WaitForSeconds(damageInterval);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            HandleDeath();
        }
    }

    void UpdateHealthBar()
    {
        healthBar.fillAmount = (float)currentHealth / maxHealth;
    }

    void HandleDeath()
    {
        foreach (var coroutine in damageCoroutines.Values)
        {
            StopCoroutine(coroutine);
        }
        damageCoroutines.Clear();

        IsPlayerDead = true;
        playerDeathPanel.SetActive(true);
        Time.timeScale = 0;
        return;

        //Debug.Log("Player has died.");
    }
}
