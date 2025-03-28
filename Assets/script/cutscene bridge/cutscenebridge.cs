using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cutscenebridge : MonoBehaviour
{
    //camera's
     private Camera fpsCam;
    
     [SerializeField] private Camera cutsceneCam;

    //explosions
    [SerializeField] public ParticleSystem explode1;
    [SerializeField] public ParticleSystem explode2;
    [SerializeField] public ParticleSystem explode3;
    [SerializeField] public ParticleSystem explode4;

    //sounds
    [SerializeField] public AudioSource boom1;
    [SerializeField] public AudioSource boom2;
    [SerializeField] public AudioSource boom3;

    //camerashake
    [SerializeField] public screenShake Shake;

    //animator
    [SerializeField] public Animator animator;
    [SerializeField] public Animator worldState;

    //trigger
    [SerializeField] public GameObject trigger;

    //UI
    [SerializeField] public GameObject crosshair;
    [SerializeField] public GameObject helth1;
    [SerializeField] public GameObject helth2;
    [SerializeField] public GameObject helth3;
    [SerializeField] private GameObject Shield;

    [SerializeField] public GameObject player;

    //turrets
    public GameObject[] turretArray; 

    public void Awake()
    {
        fpsCam = Camera.main;
        
        cutsceneCam.gameObject.SetActive(false);
        foreach (GameObject Turret in turretArray)
        {
            Turret.gameObject.SetActive(false);
        }
    }

    //start cutscene
    public void cutscene()
    {
        FindObjectOfType<fpscontroller>().canMove = false;
        FindObjectOfType<gunscript>().Shooting = true;
        fpsCam.gameObject.SetActive(false);
        cutsceneCam.gameObject.SetActive(true);
        StartCoroutine(cutsceneButNumerator());

    }

    private IEnumerator cutsceneButNumerator()
    {
        hideUI();
        animator.SetTrigger("open");
        StartCoroutine(Shake.Shake(1f, 1.5f));
        explode1.Play();
        boom1.Play();
        yield return new WaitForSeconds(1f);
        StartCoroutine(Shake.Shake(0.5f, 2.5f));
        explode2.Play();
        boom2.Play();
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Shake.Shake(1f, 3.5f));
        explode3.Play();
        boom3.Play();
        yield return new WaitForSeconds(0.5f);
        explode4.Play();
        boom1.Play();
        yield return new WaitForSeconds(0.4f);
        animator.SetTrigger("exit");
        trigger.gameObject.SetActive(false);
        worldState.SetTrigger("open");
        showUI();
        player.transform.position = new Vector3(-94.319f, 15.10499f, -42.929f);
        fpsCam.gameObject.SetActive(true);
        cutsceneCam.gameObject.SetActive(false);
        FindObjectOfType<fpscontroller>().canMove = true;
        FindObjectOfType<gunscript>().Shooting = false;
    }

    public void hideUI()
    {
        crosshair.gameObject.SetActive(false);
        helth1.gameObject.SetActive(false);
        helth2.gameObject.SetActive(false);
        helth3.gameObject.SetActive(false);
        Shield.gameObject.SetActive(false);
    }

    private void showUI()
    {
        crosshair.gameObject.SetActive(true);
        helth1.gameObject.SetActive(true);
        helth2.gameObject.SetActive(true);
        helth3.gameObject.SetActive(true);
    }
}
