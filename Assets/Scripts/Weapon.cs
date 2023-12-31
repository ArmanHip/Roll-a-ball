using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public Camera fpsCam;
    public GameObject muzzleFlash;
    public Transform gunBarrel;
    public TrailRenderer bulletTrail;

    public GameObject shootSoundPrefab;

    private bool isShooting;
    public float fireRate = 0.25f;
    private float fireTime = 0f;

    void Update()
    {
        if (Input.GetButton("Fire1") && !PauseMenu.Paused && !PlayerHealth.IsPlayerDead && !CountdownTimer.WinPanelActive && Time.time >= fireTime)
        {
            fireTime = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        var bullet = Instantiate(bulletTrail, gunBarrel.position, Quaternion.identity);
        bullet.AddPosition(gunBarrel.position);
        {
            bullet.transform.position = transform.position + (fpsCam.transform.forward * 200);
        }

        muzzleFlash.SetActive(true);
        Invoke(nameof(DisableMuzzleFlash), 0.1f);

        var shootSound = Instantiate(shootSoundPrefab, transform.position, Quaternion.identity); // Use prefab for shoot sound so it can overlap each other
        var audioSource = shootSound.GetComponent<AudioSource>();
        audioSource.pitch = Random.Range(0.9f, 1.1f); // Random pitch so it's more dynamic idk
        audioSource.Play();

        Destroy(shootSound, audioSource.clip.length); // Destroy to save memory

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
    }

    void DisableMuzzleFlash()
    {
        muzzleFlash.SetActive(false);
    }
}
