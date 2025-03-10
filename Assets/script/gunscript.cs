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
            
            
            else if (hit.transform.tag == "enemy4")
            {
                StartCoroutine(cameraShake.Shake(1f, .10f));
                FindObjectOfType<turret4>().explode();
                GameObject explosion = Instantiate(exploder, hit.transform.position + new Vector3(0.0f, 2f, 0.0f), hit.transform.rotation);
                explosion.GetComponent<ParticleSystem>().Play();
                Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
                hit.transform.gameObject.AddComponent<Rigidbody>();
                rb = hit.transform.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(fpsCam.transform.forward * 1000f);
            }
            else if (hit.transform.tag == "enemy5")
            {
                StartCoroutine(cameraShake.Shake(1f, .10f));
                FindObjectOfType<turret5>().explode();
                GameObject explosion = Instantiate(exploder, hit.transform.position + new Vector3(0.0f, 2f, 0.0f), hit.transform.rotation);
                explosion.GetComponent<ParticleSystem>().Play();
                Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
                hit.transform.gameObject.AddComponent<Rigidbody>();
                rb = hit.transform.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(fpsCam.transform.forward * 1000f);
            }
            else if (hit.transform.tag == "enemy6")
            {
                StartCoroutine(cameraShake.Shake(1f, .10f));
                FindObjectOfType<turret6>().explode();
                GameObject explosion = Instantiate(exploder, hit.transform.position + new Vector3(0.0f, 2f, 0.0f), hit.transform.rotation);
                explosion.GetComponent<ParticleSystem>().Play();
                Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
                hit.transform.gameObject.AddComponent<Rigidbody>();
                rb = hit.transform.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(fpsCam.transform.forward * 1000f);
            }
            else if (hit.transform.tag == "enemy7")
            {
                StartCoroutine(cameraShake.Shake(1f, .10f));
                FindObjectOfType<turret7>().explode();
                GameObject explosion = Instantiate(exploder, hit.transform.position + new Vector3(0.0f, 2f, 0.0f), hit.transform.rotation);
                explosion.GetComponent<ParticleSystem>().Play();
                Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
                hit.transform.gameObject.AddComponent<Rigidbody>();
                rb = hit.transform.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(fpsCam.transform.forward * 1000f);
            }
            else if (hit.transform.tag == "enemy8")
            {
                StartCoroutine(cameraShake.Shake(1f, .10f));
                FindObjectOfType<turret8>().explode();
                GameObject explosion = Instantiate(exploder, hit.transform.position + new Vector3(0.0f, 2f, 0.0f), hit.transform.rotation);
                explosion.GetComponent<ParticleSystem>().Play();
                Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
                hit.transform.gameObject.AddComponent<Rigidbody>();
                rb = hit.transform.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(fpsCam.transform.forward * 1000f);
            }
            else if (hit.transform.tag == "enemy9")
            {
                StartCoroutine(cameraShake.Shake(1f, .10f));
                FindObjectOfType<turret9>().explode();
                GameObject explosion = Instantiate(exploder, hit.transform.position + new Vector3(0.0f, 2f, 0.0f), hit.transform.rotation);
                explosion.GetComponent<ParticleSystem>().Play();
                Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
                hit.transform.gameObject.AddComponent<Rigidbody>();
                rb = hit.transform.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(fpsCam.transform.forward * 1000f);
            }
            else if (hit.transform.tag == "enemy10")
            {
                StartCoroutine(cameraShake.Shake(1f, .10f));
                FindObjectOfType<turret10>().explode();
                GameObject explosion = Instantiate(exploder, hit.transform.position + new Vector3(0.0f, 2f, 0.0f), hit.transform.rotation);
                explosion.GetComponent<ParticleSystem>().Play();
                Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
                hit.transform.gameObject.AddComponent<Rigidbody>();
                rb = hit.transform.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(fpsCam.transform.forward * 1000f);
            }
            else if (hit.transform.tag == "enemy11")
            {
                StartCoroutine(cameraShake.Shake(1f, .10f));
                FindObjectOfType<turret11>().explode();
                GameObject explosion = Instantiate(exploder, hit.transform.position + new Vector3(0.0f, 2f, 0.0f), hit.transform.rotation);
                explosion.GetComponent<ParticleSystem>().Play();
                Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
                hit.transform.gameObject.AddComponent<Rigidbody>();
                rb = hit.transform.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(fpsCam.transform.forward * 1000f);
            }
            else if (hit.transform.tag == "enemy12")
            {
                StartCoroutine(cameraShake.Shake(1f, .10f));
                FindObjectOfType<turret12>().explode();
                GameObject explosion = Instantiate(exploder, hit.transform.position + new Vector3(0.0f, 2f, 0.0f), hit.transform.rotation);
                explosion.GetComponent<ParticleSystem>().Play();
                Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
                hit.transform.gameObject.AddComponent<Rigidbody>();
                rb = hit.transform.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(fpsCam.transform.forward * 1000f);
            }
            else if (hit.transform.tag == "enemy13")
            {
                StartCoroutine(cameraShake.Shake(1f, .10f));
                FindObjectOfType<turret13>().explode();
                GameObject explosion = Instantiate(exploder, hit.transform.position + new Vector3(0.0f, 2f, 0.0f), hit.transform.rotation);
                explosion.GetComponent<ParticleSystem>().Play();
                Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
                hit.transform.gameObject.AddComponent<Rigidbody>();
                rb = hit.transform.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(fpsCam.transform.forward * 1000f);
            }
            else if (hit.transform.tag == "enemy14")
            {
                StartCoroutine(cameraShake.Shake(1f, .10f));
                FindObjectOfType<turret14>().explode();
                GameObject explosion = Instantiate(exploder, hit.transform.position + new Vector3(0.0f, 2f, 0.0f), hit.transform.rotation);
                explosion.GetComponent<ParticleSystem>().Play();
                Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
                hit.transform.gameObject.AddComponent<Rigidbody>();
                rb = hit.transform.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(fpsCam.transform.forward * 1000f);
            }
            else if (hit.transform.tag == "enemy15")
            {
                StartCoroutine(cameraShake.Shake(1f, .10f));
                FindObjectOfType<turret15>().explode();
                GameObject explosion = Instantiate(exploder, hit.transform.position + new Vector3(0.0f, 2f, 0.0f), hit.transform.rotation);
                explosion.GetComponent<ParticleSystem>().Play();
                Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
                hit.transform.gameObject.AddComponent<Rigidbody>();
                rb = hit.transform.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(fpsCam.transform.forward * 1000f);
            }
            else if (hit.transform.tag == "enemy16")
            {
                StartCoroutine(cameraShake.Shake(1f, .10f));
                FindObjectOfType<turret16>().explode();
                GameObject explosion = Instantiate(exploder, hit.transform.position + new Vector3(0.0f, 2f, 0.0f), hit.transform.rotation);
                explosion.GetComponent<ParticleSystem>().Play();
                Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
                hit.transform.gameObject.AddComponent<Rigidbody>();
                rb = hit.transform.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(fpsCam.transform.forward * 1000f);
            }
            else if (hit.transform.tag == "enemy18")
            {
                StartCoroutine(cameraShake.Shake(1f, .10f));
                FindObjectOfType<turret18>().explode();
                GameObject explosion = Instantiate(exploder, hit.transform.position + new Vector3(0.0f, 2f, 0.0f), hit.transform.rotation);
                explosion.GetComponent<ParticleSystem>().Play();
                Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
                hit.transform.gameObject.AddComponent<Rigidbody>();
                rb = hit.transform.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(fpsCam.transform.forward * 1000f);
            }
            else if (hit.transform.tag == "enemy19")
            {
                StartCoroutine(cameraShake.Shake(1f, .10f));
                FindObjectOfType<turret19>().explode();
                GameObject explosion = Instantiate(exploder, hit.transform.position + new Vector3(0.0f, 2f, 0.0f), hit.transform.rotation);
                explosion.GetComponent<ParticleSystem>().Play();
                Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
                hit.transform.gameObject.AddComponent<Rigidbody>();
                rb = hit.transform.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(fpsCam.transform.forward * 1000f);
            }
            else if (hit.transform.tag == "enemy20")
            {
                StartCoroutine(cameraShake.Shake(1f, .10f));
                FindObjectOfType<turret20>().explode();
                GameObject explosion = Instantiate(exploder, hit.transform.position + new Vector3(0.0f, 2f, 0.0f), hit.transform.rotation);
                explosion.GetComponent<ParticleSystem>().Play();
                Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
                hit.transform.gameObject.AddComponent<Rigidbody>();
                rb = hit.transform.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(fpsCam.transform.forward * 1000f);
            }
            else if (hit.transform.tag == "enemy21")
            {
                StartCoroutine(cameraShake.Shake(1f, .10f));
                FindObjectOfType<turret21>().explode();
                GameObject explosion = Instantiate(exploder, hit.transform.position + new Vector3(0.0f, 2f, 0.0f), hit.transform.rotation);
                explosion.GetComponent<ParticleSystem>().Play();
                Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
                hit.transform.gameObject.AddComponent<Rigidbody>();
                rb = hit.transform.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(fpsCam.transform.forward * 1000f);
            }
            else if (hit.transform.tag == "enemy22")
            {
                StartCoroutine(cameraShake.Shake(1f, .10f));
                FindObjectOfType<turret22>().explode();
                GameObject explosion = Instantiate(exploder, hit.transform.position + new Vector3(0.0f, 2f, 0.0f), hit.transform.rotation);
                explosion.GetComponent<ParticleSystem>().Play();
                Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
                hit.transform.gameObject.AddComponent<Rigidbody>();
                rb = hit.transform.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(fpsCam.transform.forward * 1000f);
            }
            else if (hit.transform.tag == "enemy23")
            {
                StartCoroutine(cameraShake.Shake(1f, .10f));
                FindObjectOfType<turret23>().explode();
                GameObject explosion = Instantiate(exploder, hit.transform.position + new Vector3(0.0f, 2f, 0.0f), hit.transform.rotation);
                explosion.GetComponent<ParticleSystem>().Play();
                Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
                hit.transform.gameObject.AddComponent<Rigidbody>();
                rb = hit.transform.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(fpsCam.transform.forward * 1000f);
            }
            else if (hit.transform.tag == "enemy24")
            {
                StartCoroutine(cameraShake.Shake(1f, .10f));
                FindObjectOfType<turret24>().explode();
                GameObject explosion = Instantiate(exploder, hit.transform.position + new Vector3(0.0f, 2f, 0.0f), hit.transform.rotation);
                explosion.GetComponent<ParticleSystem>().Play();
                Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
                hit.transform.gameObject.AddComponent<Rigidbody>();
                rb = hit.transform.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(fpsCam.transform.forward * 1000f);
            }
            else if (hit.transform.tag == "enemy25")
            {
                StartCoroutine(cameraShake.Shake(1f, .10f));
                FindObjectOfType<turret25>().explode();
                GameObject explosion = Instantiate(exploder, hit.transform.position + new Vector3(0.0f, 2f, 0.0f), hit.transform.rotation);
                explosion.GetComponent<ParticleSystem>().Play();
                Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
                hit.transform.gameObject.AddComponent<Rigidbody>();
                rb = hit.transform.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(fpsCam.transform.forward * 1000f);
            }
            else if (hit.transform.tag == "enemy26")
            {
                StartCoroutine(cameraShake.Shake(1f, .10f));
                FindObjectOfType<turret26>().explode();
                GameObject explosion = Instantiate(exploder, hit.transform.position + new Vector3(0.0f, 2f, 0.0f), hit.transform.rotation);
                explosion.GetComponent<ParticleSystem>().Play();
                Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
                hit.transform.gameObject.AddComponent<Rigidbody>();
                rb = hit.transform.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(fpsCam.transform.forward * 1000f);
            }
            else if (hit.transform.tag == "enemy27")
            {
                StartCoroutine(cameraShake.Shake(1f, .10f));
                FindObjectOfType<turret27>().explode();
                GameObject explosion = Instantiate(exploder, hit.transform.position + new Vector3(0.0f, 2f, 0.0f), hit.transform.rotation);
                explosion.GetComponent<ParticleSystem>().Play();
                Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
                hit.transform.gameObject.AddComponent<Rigidbody>();
                rb = hit.transform.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(fpsCam.transform.forward * 1000f);
            }
            else if (hit.transform.tag == "enemy28")
            {
                StartCoroutine(cameraShake.Shake(1f, .10f));
                FindObjectOfType<turret28>().explode();
                GameObject explosion = Instantiate(exploder, hit.transform.position + new Vector3(0.0f, 2f, 0.0f), hit.transform.rotation);
                explosion.GetComponent<ParticleSystem>().Play();
                Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
                hit.transform.gameObject.AddComponent<Rigidbody>();
                rb = hit.transform.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(fpsCam.transform.forward * 1000f);
            }
            else if (hit.transform.tag == "enemy29")
            {
                StartCoroutine(cameraShake.Shake(1f, .10f));
                FindObjectOfType<turret29>().explode();
                GameObject explosion = Instantiate(exploder, hit.transform.position + new Vector3(0.0f, 2f, 0.0f), hit.transform.rotation);
                explosion.GetComponent<ParticleSystem>().Play();
                Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
                hit.transform.gameObject.AddComponent<Rigidbody>();
                rb = hit.transform.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(fpsCam.transform.forward * 1000f);
            }
            else if (hit.transform.tag == "enemy30")
            {
                StartCoroutine(cameraShake.Shake(1f, .10f));
                FindObjectOfType<turret30>().explode();
                GameObject explosion = Instantiate(exploder, hit.transform.position + new Vector3(0.0f, 2f, 0.0f), hit.transform.rotation);
                explosion.GetComponent<ParticleSystem>().Play();
                Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
                hit.transform.gameObject.AddComponent<Rigidbody>();
                rb = hit.transform.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(fpsCam.transform.forward * 1000f);
            }
            else if (hit.transform.tag == "enemy31")
            {
                StartCoroutine(cameraShake.Shake(1f, .10f));
                FindObjectOfType<turret31>().explode();
                GameObject explosion = Instantiate(exploder, hit.transform.position + new Vector3(0.0f, 2f, 0.0f), hit.transform.rotation);
                explosion.GetComponent<ParticleSystem>().Play();
                Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
                hit.transform.gameObject.AddComponent<Rigidbody>();
                rb = hit.transform.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(fpsCam.transform.forward * 1000f);
            }
            else if (hit.transform.tag == "enemy32")
            {
                StartCoroutine(cameraShake.Shake(1f, .10f));
                FindObjectOfType<turret32>().explode();
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
