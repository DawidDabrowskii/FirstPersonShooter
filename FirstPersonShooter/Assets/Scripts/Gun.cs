using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    [Header("References")]
    public Transform fpsCam;
    public ParticleSystem muzzleFlush;
    public GameObject impactEffect;
    public AudioSource rifleFireSound;
    public Animator animator;
    public GameObject damageToObject;
    public ParticleSystem explodeEffect;

    [Header("vsMaterials")]
    [SerializeField] bool vsMetal;
    [SerializeField] bool vsShield;
    [SerializeField] bool vsEnemy;

    [Header("Attributes")]
    [SerializeField] float range = 20f;
    [SerializeField] float impactForce = 150f;
    [SerializeField] float fireRate = 10f;

    [Header("Ammunition")]
    public int maxAmmo = 10;
    public int magazineSize = 30;
    public int currentAmmo;
    public float reloadTime = 2f;
    private bool isReloading;

    private float nextTimeToFire = 0f;

    InputAction shoot;

    private void Start()
    {
        shoot = new InputAction("Shoot", binding: "mouse/leftButton");

        shoot.Enable();

        currentAmmo = maxAmmo;
    }

    private void OnEnable() // is called every time we enable weapon // removes 'problem' which stops coroutines while we reload weapon and swap in the meantime
    {
        isReloading = false;
        animator.SetBool("isReloading", false);
    }

    private void Update()
    {
        if (currentAmmo == 0 && magazineSize == 0)
        {
            animator.SetBool("isShooting", false);
            return;
        }

        if (isReloading)
            return;
        bool isShooting = shoot.ReadValue<float>() == 1;
        animator.SetBool("isShooting", isShooting); // changing animation to "isShooting" whenever bool is true in this case

        if (isShooting && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Fire();
        }

        if (currentAmmo == 0 && magazineSize > 0 && !isReloading )
        {
            StartCoroutine(Reload());
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
            impact.transform.parent = hit.transform; // bulletEffect becomes child of the objects it hits

            Destroy(impact, 0.2f);
            rifleFireSound.Play();
            muzzleFlush.Play();

            currentAmmo--;
            
            // spawning new Shield on collision with 'hit'
            if (hit.collider.tag == "Shield" && vsShield ==true)
            {
                damageToObject.transform.position = new Vector3(Random.Range(0, 10), Random.Range(9, 15), Random.Range(45, 55));
            }

            // destroying Barrels
            if (hit.collider.tag == "Barrel" && vsMetal == true)
            {
                Destroy(hit.collider.gameObject,1f);

            }
            // destroying Enemies
            if (hit.collider.tag == "Enemy" && vsEnemy == true)
            {
                Destroy(hit.collider.gameObject, 0.5f);
            }
            if (hit.collider.tag == "AllWeapons")
            {
                Destroy(hit.collider.gameObject, 0.2f);
            }
        }
    }
    IEnumerator Reload()
    {
        isReloading = true;
        animator.SetBool("isReloading", true);

        yield return new WaitForSeconds(reloadTime);
        animator.SetBool("isReloading", false);
        if (magazineSize >= maxAmmo)
        {
            currentAmmo = maxAmmo;
            magazineSize -= maxAmmo;
        }
        else
        {
            currentAmmo = magazineSize;
            magazineSize = 0;
        }
        isReloading = false;
    }
}