using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 50f;
    public GameObject explosionEffect; 

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        if (explosionEffect != null)
        {
            GameObject instantiatedExplosionEffect = Instantiate(explosionEffect, transform.position, transform.rotation);

            ParticleSystem ps = instantiatedExplosionEffect.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                float totalDuration = ps.main.duration + ps.main.startLifetime.constantMax;

                Destroy(instantiatedExplosionEffect, totalDuration);
            }
            else
            {
                Destroy(instantiatedExplosionEffect, 5f);
            }
        }

        Destroy(gameObject);
    }
}
