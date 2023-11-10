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
    private Dictionary<string, Coroutine> damageCoroutines = new Dictionary<string, Coroutine>();

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
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
    }

    void UpdateHealthBar()
    {
        healthBar.fillAmount = (float)currentHealth / maxHealth;
    }
}
