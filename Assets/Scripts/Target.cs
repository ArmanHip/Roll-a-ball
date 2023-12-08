using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 50f;
    public GameObject explosionEffect;
    public GameObject soundPlayerPrefab;

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
            GameObject instantiatedExplosion = Instantiate(explosionEffect, transform.position, transform.rotation);
            ParticleSystem explosionParticles = instantiatedExplosion.GetComponent<ParticleSystem>();

            if (explosionParticles != null)
            {
                float duration = explosionParticles.main.duration + explosionParticles.main.startLifetime.constantMax;
                Destroy(instantiatedExplosion, duration);
            }
            else
            {
                Destroy(instantiatedExplosion, 1f);
            }
        }

        PlayDestructionSound();

        Destroy(gameObject);
    }

    void PlayDestructionSound()
    {
        if (soundPlayerPrefab != null)
        {
            GameObject soundPlayer = Instantiate(soundPlayerPrefab, transform.position, Quaternion.identity);
            AudioSource audioSource = soundPlayer.GetComponent<AudioSource>();

            if (audioSource != null)
            {
                audioSource.pitch = Random.Range(0.8f, 1.2f);
                audioSource.Play();

                Destroy(soundPlayer, audioSource.clip.length);
            }
        }
    }
}
