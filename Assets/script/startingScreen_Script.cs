using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startingScreen_Script : MonoBehaviour
{
    //transition to game
    public AudioSource Stinger;
    public Animator transitioner;

    //animations
    public Animator animator1;
    public Animator animator2;
    public Animator animator3;
    public Animator animator4;
    public Animator animator5;

    //cameras
    public GameObject cam1;
    public GameObject cam2;
    public GameObject cam3;
    public GameObject cam4;
    public GameObject cam5;

    //random int
    public int random;
    public int last_random;

    // Update is called once per frame
    private void Start()
    {
        randomize();
    }
    void randomize()
    {
        //random camera
        int random = Random.Range(1, 6);
        if (random != last_random)
        {
            if (random == 1)
            {
                StartCoroutine(Cam1());
            }
            else if (random == 2)
            {
                StartCoroutine(Cam2());
            }
            else if (random == 3)
            {
                StartCoroutine(Cam3());
            }
            else if (random == 4)
            {
                StartCoroutine(Cam4());
            }
            else if (random == 5)
            {
                StartCoroutine(Cam5());
            }
        }
    }

    //camera animations and switches
    IEnumerator Cam1()
    {
        cam1.SetActive(true);
        cam2.SetActive(false);
        cam3.SetActive(false);
        cam4.SetActive(false);
        cam5.SetActive(false);

        animator1.SetTrigger("open");
        yield return new WaitForSeconds(6);
        last_random = random;
        randomize();
    }

    IEnumerator Cam2()
    {
        cam1.SetActive(false);
        cam2.SetActive(true);
        cam3.SetActive(false);
        cam4.SetActive(false);
        cam5.SetActive(false);

        animator2.SetTrigger("open");
        yield return new WaitForSeconds(6);
        last_random = random;
        randomize();
    }
    IEnumerator Cam3()
    {
        cam1.SetActive(false);
        cam2.SetActive(false);
        cam3.SetActive(true);
        cam4.SetActive(false);
        cam5.SetActive(false);

        animator3.SetTrigger("open");
        yield return new WaitForSeconds(6);
        last_random = random;
        randomize();
    }
    IEnumerator Cam4()
    {
        cam1.SetActive(false);
        cam2.SetActive(false);
        cam3.SetActive(false);
        cam4.SetActive(true);
        cam5.SetActive(false);

        animator4.SetTrigger("open");
        yield return new WaitForSeconds(6);
        last_random = random;
        randomize();
    }
    IEnumerator Cam5()
    {
        cam1.SetActive(false);
        cam2.SetActive(false);
        cam3.SetActive(false);
        cam4.SetActive(false);
        cam5.SetActive(true);

        animator5.SetTrigger("open");
        yield return new WaitForSeconds(6);
        last_random = random;
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
