using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cutscenebridge : MonoBehaviour
{
    //camera's
    public Camera fpsCam;
    public Camera cutsceneCam;

    //screenshake
    public screenShake cameraShake;

    //explosions
    public ParticleSystem explode1;
    public ParticleSystem explode2;
    public ParticleSystem explode3;
    public ParticleSystem explode4;

    //sounds
    public AudioSource boom1;
    public AudioSource boom2;
    public AudioSource boom3;

    //camerashake
    public screenShake Shake;

    //animator
    public Animator animator;
    public Animator worldState;

    //trigger
    public GameObject trigger;

    //UI
    public GameObject crosshair;
    public GameObject helth1;
    public GameObject helth2;
    public GameObject helth3;

    public GameObject player;

    //turrets
    public GameObject turret1;
    public GameObject turret2;
    public GameObject turret3;
    public GameObject turret4;
    public GameObject turret5;
    public GameObject turret6;
    public GameObject turret7;
    public GameObject turret8;
    public GameObject turret9;
    public GameObject turret10;
    public GameObject turret11;
    public GameObject turret12;
    public GameObject turret13;
    public GameObject turret14;
    public GameObject turret15;
    public GameObject turret16;
    public GameObject turret17;
    public GameObject turret18;
    public GameObject turret19;
    public GameObject turret20;
    public GameObject turret21;
    public GameObject turret22;
    public GameObject turret23;
    public GameObject turret24;
    public GameObject turret25;
    public GameObject turret26;
    public GameObject turret27;
    public GameObject turret28;
    public GameObject turret29;
    public GameObject turret30;
    public GameObject turret31;
    public GameObject turret32;

    public void Awake()
    {
        cutsceneCam.gameObject.SetActive(false);
        turret1.gameObject.SetActive(false);
        turret2.gameObject.SetActive(false);
        turret3.gameObject.SetActive(false);
        turret4.gameObject.SetActive(false);
        turret5.gameObject.SetActive(false);
        turret6.gameObject.SetActive(false);
        turret7.gameObject.SetActive(false);
        turret8.gameObject.SetActive(false);
        turret9.gameObject.SetActive(false);
        turret10.gameObject.SetActive(false);
        turret11.gameObject.SetActive(false);
        turret12.gameObject.SetActive(false);
        turret13.gameObject.SetActive(false);
        turret14.gameObject.SetActive(false);
        turret15.gameObject.SetActive(false);
        turret16.gameObject.SetActive(false);
        turret18.gameObject.SetActive(false);
        turret19.gameObject.SetActive(false);
        turret20.gameObject.SetActive(false);
        turret21.gameObject.SetActive(false);
        turret22.gameObject.SetActive(false);
        turret23.gameObject.SetActive(false);
        turret24.gameObject.SetActive(false);
        turret25.gameObject.SetActive(false);
        turret26.gameObject.SetActive(false);
        turret27.gameObject.SetActive(false);
        turret28.gameObject.SetActive(false);
        turret29.gameObject.SetActive(false);
        turret30.gameObject.SetActive(false);
        turret31.gameObject.SetActive(false);
        turret32.gameObject.SetActive(false);
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

    IEnumerator cutsceneButNumerator()
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
    }

    private void showUI()
    {
        crosshair.gameObject.SetActive(true);
        helth1.gameObject.SetActive(true);
        helth2.gameObject.SetActive(true);
        helth3.gameObject.SetActive(true);
    }
}
