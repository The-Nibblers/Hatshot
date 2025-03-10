using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class endscript : MonoBehaviour
{
    //animators
    public Animator UI;
    public Animator cuts;

    //audio
    public AudioSource endstinger;

    //screenshake
    public screenShake cameraShake;

    //cameras
    public Camera cutscenecam;
    public Camera fpscam;

    //explosions
    public GameObject exploder;
    public GameObject exploder2;
    public GameObject exploder3;
    public GameObject exploder4;
    public GameObject exploder5;


    private void Start()
    {
        cutscenecam.gameObject.SetActive(false);

        exploder.gameObject.SetActive(false);
        exploder2.gameObject.SetActive(false);
        exploder3.gameObject.SetActive(false);
        exploder4.gameObject.SetActive(false);
        exploder5.gameObject.SetActive(false);

    }

    public void theEnd()
    {
        //hideUI
        FindObjectOfType<cutscenebridge>().hideUI();

        //cameraenablers
        cutscenecam.gameObject.SetActive(true);
        fpscam.gameObject.SetActive(false);

        StartCoroutine(cutscene());
    }

    IEnumerator cutscene()
    {
        //cutscene
        cuts.SetTrigger("scene");
        //funny boom
        StartCoroutine(explode());
        yield return new WaitForSeconds(3);

        //initiate thanks
        StartCoroutine(thankPlayer());
    }
    IEnumerator thankPlayer()
    {
        UI.SetTrigger("open");
        yield return new WaitForSeconds(10);
        AppHelper.Quit();
    }

    IEnumerator explode()
    {
        exploder.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        exploder2.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        exploder3.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        exploder4.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        exploder5.gameObject.SetActive(true);
        yield return new WaitForSeconds(4);
        endstinger.Play();
    }
}
