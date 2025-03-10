using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]

public class SC_FPSController : MonoBehaviour
{
    public float walkingSpeed = 11.5f;

    public float jumpSpeed = 8.0f;
    public float gravity = 15.0f;
    public Camera PlayerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 90.0f;

    //turtorial things
    public Animator tut;
    public Text tuttext;

    // music
    public AudioSource music_calm;
    public AudioSource rain_normal;
    public AudioSource rain_metal;
    public AudioSource music_loud;
    public AudioSource shotgun_pickupSfx;
    public AudioSource doorsfx;

    //shotgun pickup animation
    public GameObject shotgun_pickup;
    public GameObject shotgun_pickup_inperson;
    public Animator shotguner;

    //fall
    public GameObject addrb1;
    public GameObject addrb2;
    public GameObject addrb3;
    public GameObject addrb4;
    public GameObject addrb5;
    public AudioSource clonk;

    // animationstart
    public Animator startanim;
    public GameObject backdrop;
    public bool yes = false;

    //dash
    public float dashTime;
    public float dashSpeed;
    public AudioSource dashsfx;
    public int canDash = 0;
    private float coolDownPeriodInSeconds = 0.15f;

    // doublejump
    public int fuckyou = 0;
    public bool doJump = false;
    public float doubleJumpSpeed = 120.0f;

    //hat wearing
    public GameObject normalHat;
    public GameObject rocketHat;
    public GameObject doubleJumpHat;

    //hat UI elements
    public GameObject UIHat1;
    public GameObject UIHat2;
    public GameObject UIHat3;

    //detection
    public int hat;
    public Text hattext;
    private string s;

    //keys
    public int keys = 0;
    public GameObject keyUI1;
    public GameObject keyUI2;
    public GameObject keyUI3;

    //door 1 animation
    public Animator door1anim;
    public GameObject door1obj;

    //door 2 animation
    public Animator door2anim;
    public GameObject door2obj;

    //door 3 animation
    public Animator door3anim;
    public GameObject door3obj;

    //glass door animation
    public Animator doorglassanim;
    public GameObject doorglassobj;

    //area1 flare
    public ParticleSystem rockethatflare;

    // area 2 flare
    public ParticleSystem area2Flare;

CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    [HideInInspector]
    public bool canMove = true;

    [SerializeField]
    private float delay = 0.5f;


    void Start()
    {
        Cursor.visible = false;
        //startanimation play
        StartCoroutine(startani());
        StartCoroutine(startturtorial());

        IEnumerator startani()
        {
            startanim.SetTrigger("open");
            canMove = false;
            yield return new WaitForSeconds((float)0.5);
            canMove = true;
            Destroy(backdrop);

        }

    //get charachter controller
    characterController = GetComponent<CharacterController>();

        //disable shotgun
        shotgun_pickup_inperson.gameObject.SetActive(false);

        //show hatcount in beggining
        s = Convert.ToString(hat);
        hattext.text = s;

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //enable normal hat
        normalHat.gameObject.SetActive(true);
        rocketHat.gameObject.SetActive(false);
        doubleJumpHat.gameObject.SetActive(false);

        //start flare rockethat
        rockethatflare.Play();
        area2Flare.Play();

        //disable UIhats
        UIHat2.SetActive(false);
        UIHat3.SetActive(false);

        //disable keyUI
        keyUI1.SetActive(false);
        keyUI2.SetActive(false);
        keyUI3.SetActive(false);
    }

    void Update()
    {
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        // Press Left Shift to run
        float curSpeedX = canMove ? (walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (walkingSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        //jump
        if (Input.GetButton("Jump") && canMove && fuckyou == 0 && doJump == false)
        {
            StartCoroutine(normaljump());
        }
        else if (Input.GetButton("Jump") && canMove && fuckyou == 0 && doJump == true)
        {
            StartCoroutine(jumpcd());
        }
        else
        {
            moveDirection.y = movementDirectionY;

        }
        IEnumerator normaljump()
        {
            moveDirection.y = jumpSpeed;
            yield return new WaitForSeconds((float)0.10);
            fuckyou++;
            yield return null;
        }
        IEnumerator jumpcd()
        {
            moveDirection.y = jumpSpeed;
            yield return new WaitForSeconds(1);
            fuckyou++;
            yield return null;
        }

        // dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time > coolDownPeriodInSeconds)
        {
            if (canDash == 1)
            {
                dashsfx.Play();
                StartCoroutine(dash());
                coolDownPeriodInSeconds = Time.time + delay;
            }
        }

        IEnumerator dash()
        {
            float startime = Time.time;

            while (Time.time < startime + dashTime)
            {
                characterController.Move(moveDirection * dashSpeed * Time.deltaTime);
                yield return null;
            }

        }
        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        else
        {
            fuckyou = 0;
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            PlayerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
        

    }
    //collision system
    private void OnTriggerEnter(Collider other)
    {
        //normal hat collision
        if (other.tag == "Detect")
        {
            hat++;
            s = Convert.ToString(hat);
            hattext.text = s;
            Destroy(other.gameObject);

        }
        //key collision
        if (other.tag == "key1")
        {
            keys++;
            keyUI1.SetActive(true);
            Destroy(other.gameObject);
        }
        if (other.tag == "key2")
        {
            keys++;
            keyUI2.SetActive(true);
            Destroy(other.gameObject);
        }
        if (other.tag == "key3")
        {
            keys++;
            keyUI3.SetActive(true);
            Destroy(other.gameObject);
        }
        //door 1 collision
        else if (other.tag == "door1" && hat >= 5)
        {
            StartCoroutine(jumpMessage());
            int calc = hat - 5;
            hat = calc;
            s = Convert.ToString(hat);
            hattext.text = s;
            door1anim.SetTrigger("open");
            StartCoroutine(door1waittime());

        }
        //door 2 collision
        else if (other.tag == "door2" && hat >= 10)
        {
            int calc = hat - 10;
            hat = calc;
            s = Convert.ToString(hat);
            hattext.text = s;
            door2anim.SetTrigger("open");
            StartCoroutine(door2waittime());
        }
        //door 3 collision
        else if (other.tag == "door3" && hat >= 15)
        {
            int calc = hat - 15;
            hat = calc;
            s = Convert.ToString(hat);
            hattext.text = s;
            door3anim.SetTrigger("open");
            StartCoroutine(door3waittime());
        }

        // rockethat pickup collision
        else if (other.tag == "rockethat")
        {
            StartCoroutine(dashMessage());
            rocketHat.gameObject.SetActive(true);
            normalHat.gameObject.SetActive(false);
            rockethatflare.Stop();
            Destroy(other.gameObject);
            canDash = 1;
            UIHat1.SetActive(false);
            UIHat2.SetActive(true);
        }

        //doublejumphat collision
        else if (other.tag == "doublejumphat")
        {
            StartCoroutine(weirdMessage());
            rocketHat.gameObject.SetActive(false);
            doubleJumpHat.gameObject.SetActive(true);
            area2Flare.Stop();
            Destroy(other.gameObject);
            UIHat2.SetActive(false);
            UIHat3.SetActive(true);
            jumpSpeed = 6.0f;
            doJump = true;
            doorglassanim.SetTrigger("open");
            StartCoroutine(doorglasswaittime());
        }

        //area 3 fall
        else if (other.tag == "fall")
        {
            addrb1.AddComponent<Rigidbody>();
            addrb2.AddComponent<Rigidbody>();
            addrb3.AddComponent<Rigidbody>();
            addrb4.AddComponent<Rigidbody>();
            addrb5.AddComponent<Rigidbody>();
            clonk.Play();
            StartCoroutine(destroyclonks());
        }

        //shotgunpickup anim
        else if (other.tag == "shotgun")
        {
            Destroy(other.gameObject);
            StartCoroutine(shotgun_pickups());
        }

        //stop music
        else if (other.tag == "music stop")
        {
            StartCoroutine(FadeAudio.FadeOut(music_calm, 1f));
        }

        //change rain to metal
        else if (other.tag == "start_metal")
        {
            StartCoroutine(FadeAudio.FadeOut(rain_normal, 1f));
            rain_metal.Play();
        }
        //change metal back to normal
        else if (other.tag == "start_normal")
        {
            StartCoroutine(FadeAudio.FadeOut(rain_metal,1f));
            rain_normal.Play();
        }

        //start loud music
        else if (other.tag == "start_loud")
        {
            StartCoroutine(loudsting());

        }
    }
    // end audio destroy
    IEnumerator loudsting()
    {
        music_loud.Play();
        yield return new WaitForSeconds(16);
        Destroy(music_loud.gameObject);
    }
    // shotgunpickup anim
    IEnumerator shotgun_pickups()
    {
        canMove = false;
        shotgun_pickup_inperson.gameObject.SetActive(true);
        shotguner.SetTrigger("open");
        shotgun_pickupSfx.Play();
        yield return new WaitForSeconds(3.2f);
        canMove = true;
        shotguner.SetTrigger("idle");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //destroy audiosource
    IEnumerator destroyclonks()
    {
        yield return new WaitForSeconds(3);
        Destroy(clonk);
    }
        //door1 wait till destroy
        IEnumerator door1waittime()
    {
        doorsfx.Play();
        yield return new WaitForSeconds(1);
        Destroy(door1obj);
    }
        
        //door2 wait till destroy
        IEnumerator door2waittime()
    {
        doorsfx.Play();
        yield return new WaitForSeconds(1);
        Destroy(door2obj);
    }

        //door 3 wait till destroy
        IEnumerator door3waittime()
    {
        doorsfx.Play();
        yield return new WaitForSeconds(1);
        Destroy(door3obj);
    }

    //door glass wait till destroy
    IEnumerator doorglasswaittime()
    {
        doorsfx.Play();
        yield return new WaitForSeconds(1);
        Destroy(doorglassobj);
    }

    //turtorial messages
    IEnumerator startturtorial()
    {
        tuttext.text = "W A S D to move";
        yield return new WaitForSeconds(3);
        tut.SetTrigger("thing");
        yield return new WaitForSeconds(6.8f);
        tut.SetTrigger("idle");
    }

    IEnumerator jumpMessage()
    {
        tuttext.text = "press SPACE to jump";
        tut.SetTrigger("thing");
        yield return new WaitForSeconds(6.8f);
        tut.SetTrigger("idle");
    }

    IEnumerator dashMessage()
    {
        tuttext.text = "press SHIFT to dash in the direction you're moving";
        tut.SetTrigger("thing");
        yield return new WaitForSeconds(6.8f);
        tut.SetTrigger("idle");
    }

    IEnumerator weirdMessage()
    {
        tuttext.text = "hold SPACE to float, you can dash during the float";
        tut.SetTrigger("thing");
        yield return new WaitForSeconds(6.8f);
        tut.SetTrigger("idle");
    }

}