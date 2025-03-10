using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class gunscript : MonoBehaviour
{
    [Header("shotgun stats")]
    [SerializeField] private float range = 300f;
    [SerializeField] private int totalAmmo = 2;

    [Header("references")]
    
    [SerializeField] private GameObject exploder;
    
    [SerializeField] private GameObject muzzleLight;
    [SerializeField] private ParticleSystem muzzleParticle;
    
    [SerializeField] private Animator shortgun;
    
    [SerializeField] private AudioSource openclick;
    [SerializeField] private AudioSource insert;
    [SerializeField] private AudioSource shot;
    [SerializeField] private AudioSource dry;
    
    [SerializeField] private screenShake cameraShake;

    
    //bools
    [HideInInspector] public bool isdead = false;
    [HideInInspector] public bool Shooting;
    private bool reloading;

    //misselanious
    private string AmmoUIText;
    private Rigidbody rb;
    private Camera fpsCam;
    private TextMesh textObject;
    
    //events
    private UnityEvent OnShoot;
    private UnityEvent OnReload;
    

    private void Awake()
    {
        muzzleLight.gameObject.SetActive(false);
        fpsCam = Camera.main;
        textObject = GameObject.Find("ammo").GetComponent<TextMesh>();
        
        OnShoot = new UnityEvent();
        OnReload = new UnityEvent();
        
        OnShoot.AddListener(BeforeShoot);
        OnReload.AddListener(BeforeReload);

        UpdateAmmoCounter();
    }
    
    void Update()
    {

            //shoot
            if (Input.GetButtonDown("Fire1"))
            {
                OnShoot.Invoke();
            }
            //reload input
            if (Input.GetKeyDown("r"))
            {
                OnReload.Invoke();
            }
    }

    private void BeforeShoot()
    {
        if (isdead)
            return;
        
        if (totalAmmo > 0 && !reloading && !Shooting)
        {
            StartCoroutine(Shoot());
        }
        else if (totalAmmo == 0 && !reloading && !Shooting) 
        {
            dry.Play();
        }
    }

    private void BeforeReload()
    {
        if (isdead || reloading || Shooting)
            return;
        
        if (totalAmmo == 1)
        {
            StartCoroutine(reload1shell());
        }
        else if (totalAmmo == 0)
        {
            StartCoroutine(reload2shell());
        }
    }
    private void UpdateAmmoCounter()
    {
        AmmoUIText = Convert.ToString(totalAmmo);
        textObject.text = AmmoUIText;
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
            if(hit.transform.CompareTag("enemy"))
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
            else if (hit.transform.CompareTag("end"))
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
        UpdateAmmoCounter();
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
        UpdateAmmoCounter();

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
        UpdateAmmoCounter();

        shortgun.SetTrigger("idler");
        reloading = false;
    }

    private void OnDestroy()
    {
        OnShoot.RemoveAllListeners();
        OnReload.RemoveAllListeners();
    }
}
