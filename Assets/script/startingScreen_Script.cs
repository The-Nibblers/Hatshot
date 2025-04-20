using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startingScreen_Script : MonoBehaviour
{
    //transition to game
    [SerializeField] private AudioSource Stinger;
    [SerializeField] private Animator transitioner;

    //animations
    [SerializeField] private Animator[] cameraAnimators;
    [SerializeField] private GameObject[] cameraObjects;

    //random int
    private int random;
    private int last_random;

    //camera references
    private Camera PrevCam;
    private Camera CurrentCam;
    private Animator CurrentAnimator;

    // Update is called once per frame
    private void Start()
    {
        randomize();
    }
    void randomize()
    {
        int random = Random.Range(0, cameraObjects.Length); // Now using 0-based index

        if (random != last_random)
        {
            CurrentCam = cameraObjects[random].GetComponent<Camera>();
            CurrentAnimator = cameraAnimators[random];

            StartCoroutine(SetCam(CurrentCam, CurrentAnimator));
            last_random = random;
        }
    }

    IEnumerator SetCam(Camera nextCam, Animator camAnimator)
    {
        if (PrevCam != null)
        {
            PrevCam.gameObject.SetActive(false);   
        }
        nextCam.gameObject.SetActive(true);
        
        camAnimator.SetTrigger("open");
        yield return new WaitForSeconds(6);
        
        PrevCam = nextCam;
        randomize();
    }
    
    public void Playgame()
    {
        StartCoroutine(stinger());
    }

    IEnumerator stinger()
    {
        Stinger.Play();
        transitioner.SetTrigger("open");
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Quitgame()
    {
        AppHelper.Quit();
    }
}
