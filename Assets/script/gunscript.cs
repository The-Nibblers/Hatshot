using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunscript : MonoBehaviour
{
    //shotgun stats
    public float damage = 10f;
    public float range = 300f;
    public int totalAmmo = 2;

    //explosion
    public GameObject exploder;

    //death check
    public bool isdead = false;

    // muzzleflash
    public GameObject muzzleLight;
    public ParticleSystem muzzleParticle;

    //camerashake
    public screenShake cameraShake;

    // shotgun text
    private string s;

    // reload and fire check
    public bool reloading;
    public bool Shooting;

    //animator
    public Animator shortgun;

    // shotgun sounds
    public AudioSource openclick;
    public AudioSource insert;
    public AudioSource shot;
    public AudioSource dry;

    //get the rigidbody of the turrets
    public Rigidbody rb;

    //get the camera for the shotgun
    public Camera fpsCam;

    private void Awake()
    {
        muzzleLight.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (isdead == false)
        {
            //ammotext
            s = Convert.ToString(totalAmmo);
            //find 3dtext
            TextMesh textObject = GameObject.Find("ammo").GetComponent<TextMesh>();
            textObject.text = s;

            //shoot
            if (Input.GetButtonDown("Fire1"))
            {
                if (totalAmmo > 0 && !reloading && !Shooting)
                {
                    StartCoroutine(Shoot());
                }
                else if (totalAmmo == 0)
                {
                    dry.Play();
                }
            }
            //reload input
            if (Input.GetKeyDown("r") && !reloading && !Shooting)
            {
                if (totalAmmo == 1)
                {
                    StartCoroutine(reload1shell());
                }
                if (totalAmmo == 0)
                {
                    StartCoroutine(reload2shell());
                }
            }
        }
    }

    IEnumerator Shoot()
    {
        Shooting = true;
        totalAmmo--;

        //make raycast and check if it hits
        RaycastHit hit;
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            //turret death code
            if(hit.transform.tag == "enemy")
            {
                StartCoroutine(cameraShake.Shake(1f, .10f));
                hit.transform.GetComponentInChildren<GunTurret>()?.Explode();
                GameObject explosion = Instantiate(exploder, hit.transform.position + new Vector3(0.0f, 2f, 0.0f), hit.transform.rotation);
                explosion.GetComponent<ParticleSystem>().Play();
                Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
                hit.transform.gameObject.AddComponent<Rigidbody>();
                rb = hit.transform.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(fpsCam.transform.forward * 1000f);
            }
            else if (hit.transform.tag == "end")
            {
                FindObjectOfType<endscript>().theEnd();
            }
        }

        StartCoroutine(cameraShake.Shake(.20f, .4f));
        muzzleLight.gameObject.SetActive(true);
        muzzleParticle.Play();
        shortgun.SetTrigger("shoot");
        shot.Play();
        yield return new WaitForSeconds(0.30f);
        muzzleLight.gameObject.SetActive(false);
        shortgun.SetTrigger("idler");
        Shooting = false;

    }


    //reload when empty
    IEnumerator reload2shell()
    {
        reloading = true;
        shortgun.SetTrigger("rel2shell");
        yield return new WaitForSeconds(0.17f);
        openclick.Play();
        yield return new WaitForSeconds(0.07f);
        insert.Play();
        yield return new WaitForSeconds(0.33f);
        openclick.Play();
        yield return new WaitForSeconds(0.03f);

        totalAmmo = 2;

        shortgun.SetTrigger("idler");
        reloading = false;
    }

    // reload with a slug in the chamber
    IEnumerator reload1shell()
    {
        reloading = true;
        shortgun.SetTrigger("rel1shell");
        yield return new WaitForSeconds(0.17f);
        openclick.Play();
        yield return new WaitForSeconds(0.07f);
        insert.Play();
        yield return new WaitForSeconds(0.33f);
        openclick.Play();
        yield return new WaitForSeconds(0.03f);

        totalAmmo = 2;

        shortgun.SetTrigger("idler");
        reloading = false;
    }
}
