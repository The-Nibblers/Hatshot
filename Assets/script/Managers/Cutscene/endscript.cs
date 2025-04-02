using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class endscript : MonoBehaviour
{
    //animators
    [SerializeField] private Animator UI;
    [SerializeField] private Animator cuts;

    //audio
    [SerializeField] private AudioSource endstinger;

    //screenshake
    [SerializeField] private screenShake cameraShake;

    //cameras
    [SerializeField] private Camera cutscenecam;
    private Camera fpscam;
    
    [Header("explosions")]
    [SerializeField] private GameObject exploder;
    [SerializeField] private GameObject exploder2;
    [SerializeField] private GameObject exploder3;
    [SerializeField] private GameObject exploder4;
    [SerializeField] private GameObject exploder5;
    
    private cutscenebridge cutscenebridge1;


    private void Start()
    {
        cutscenebridge1 = FindObjectOfType<cutscenebridge>();

        fpscam = Camera.main;
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
        cutscenebridge1.hideUI();

        //cameraenablers
        cutscenecam.gameObject.SetActive(true);
        fpscam.gameObject.SetActive(false);

        StartCoroutine(cutscene());
    }

    private IEnumerator cutscene()
    {
        //cutscene
        cuts.SetTrigger("scene");
        //funny boom
        StartCoroutine(explode());
        yield return new WaitForSeconds(3);

        //initiate thanks
        StartCoroutine(thankPlayer());
    }
    private IEnumerator thankPlayer()
    {
        UI.SetTrigger("open");
        yield return new WaitForSeconds(10);
        AppHelper.Quit();
    }

    private IEnumerator explode()
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
