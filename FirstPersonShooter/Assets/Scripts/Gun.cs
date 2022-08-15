using UnityEngine.InputSystem;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform fpsCam;
    public ParticleSystem muzzleFlush;
    public GameObject impactEffect;
    public AudioSource rifleFireSound;

    [Header("Attributes")]
    [SerializeField] float range = 20f;
    [SerializeField] float impactForce = 150f;
    [SerializeField] float fireRate = 10f;

    private float nextTimeToFire = 0f;

    InputAction shoot;

    private void Start()
    {
        shoot = new InputAction("Shoot", binding: "mouse/leftButton");

        shoot.Enable();
    }

    private void Update()
    {
        bool isShooting = shoot.ReadValue<float>() == 1;

        if (isShooting && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Fire();
        }
    }
    private void Fire()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.position, fpsCam.forward, out hit, range))
        {
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
            Quaternion impactRotation = Quaternion.LookRotation(hit.normal);
            GameObject impact = Instantiate(impactEffect, hit.point, impactRotation);
            Destroy(impact, 2f);
            rifleFireSound.Play();
            muzzleFlush.Play();

        }

        {

        }
    }
}

