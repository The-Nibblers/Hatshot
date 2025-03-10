using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[RequireComponent(typeof(CharacterController))]

public class fpscontroller : MonoBehaviour
{

    private Rigidbody rb;
    private Camera PlayerCamera;
    
    [Header("Player Variables")]
    [SerializeField] private float walkingSpeed = 11.5f;
    [SerializeField] private float jumpSpeed = 8.0f;
    [SerializeField] private float gravity = 15.0f;
    [SerializeField] private float lookSpeed = 2.0f;
    [SerializeField] private float lookXLimit = 90.0f;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashSpeed;

    
    // turtorial text
    [SerializeField] private Animator tuto;

    //metal screaching noise
    [SerializeField] private AudioSource screech;
    [SerializeField] private AudioSource mucis_loud;
    [SerializeField] private AudioSource heartbeat;

    //screenshake
    [SerializeField] private screenShake cameraShake;

    // ui on death
    [SerializeField] private GameObject crosshair;
    [SerializeField] private GameObject deahtText;
    [SerializeField] private GameObject deathButton;
 
    //turret damage and health
    public float health = 100;
    private float thehealth;

    //die rigidbody
    [SerializeField] private GameObject pl1;
    [SerializeField] private GameObject pl2;
    [SerializeField] private GameObject pl5;

    //dash
  
    [SerializeField] private AudioSource dashsfx;
    private int canDash = 1;
    [SerializeField] private float coolDownPeriodInSeconds = 0.15f;

    // float
    private int canFloat = 1;
    [SerializeField] private float doubleJumpSpeed = 120.0f;

    //hat wearing
    [SerializeField] private GameObject doubleJumpHat;

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    
    [HideInInspector] public bool canMove = true;

    [SerializeField] private float delay = 0.5f;


    private void Start()
    {
        rb = pl2.GetComponent<Rigidbody>();
        PlayerCamera = Camera.main;
        
        
        //hide cursor
        Cursor.visible = false;

        //start turtorial
        StartCoroutine(guntut());


        //ui
        deahtText.gameObject.SetActive(false);
        deathButton.gameObject.SetActive(false);

        //get charachter controller
        characterController = GetComponent<CharacterController>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //enable normal hat
        doubleJumpHat.gameObject.SetActive(true);
    }

    private void Update()
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
        if (Input.GetButton("Jump") && canMove && canFloat == 0)
        {
            StartCoroutine(jumpcd());
        }
        else
        {
            moveDirection.y = movementDirectionY;

        }
        IEnumerator jumpcd()
        {
            moveDirection.y = jumpSpeed;
            yield return new WaitForSeconds(1);
            canFloat++;
            yield return null;
        }

        // dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time > coolDownPeriodInSeconds)
        {
                dashsfx.Play();
                StartCoroutine(dash());
                coolDownPeriodInSeconds = Time.time + delay;
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
            canFloat = 0;
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

        if (health <= 0)
        {
            canMove = false;
            FindObjectOfType<gunscript>().isdead = true;
            pl1.GetComponent<Animator>().enabled = false;
            pl1.AddComponent<Rigidbody>();
            pl2.AddComponent<Rigidbody>();
            pl5.AddComponent<Rigidbody>();
            rb.AddForce(PlayerCamera.transform.forward * 10f);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            crosshair.gameObject.SetActive(false);
            deahtText.gameObject.SetActive(true);
            deathButton.gameObject.SetActive(true);

        }


    }
    //health and damage system
    public void damaging()
    {
        thehealth = health - 2f;
        health = thehealth;
    }

    //restart on death
    private void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //collision system
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "cutscene")
        {
            FindObjectOfType<cutscenebridge>().cutscene();
        }
        else if (other.tag == "metalnoise")
        {
            screech.Play();
            StartCoroutine(cameraShake.Shake(3f, 0.3f));
        }
        else if (other.tag == "heal")
        {
            health = 100;
            Destroy(other.gameObject);
        }
        else if (other.tag == "hitbox1")
        {
            //enable new turrets
            FindObjectOfType<cutscenebridge>().turret1.gameObject.SetActive(true);
            FindObjectOfType<cutscenebridge>().turret2.gameObject.SetActive(true);
            FindObjectOfType<cutscenebridge>().turret3.gameObject.SetActive(true);
        }
        else if (other.tag == "hitbox2")
        {
            //disable the previous turrets
            // FindObjectOfType<cutscenebridge>().turret1.gameObject.SetActive(false);
            // FindObjectOfType<cutscenebridge>().turret2.gameObject.SetActive(false);
            // FindObjectOfType<cutscenebridge>().turret3.gameObject.SetActive(false);
            //
            // //enable new ones
            // FindObjectOfType<cutscenebridge>().turret4.gameObject.SetActive(true);
            // FindObjectOfType<cutscenebridge>().turret5.gameObject.SetActive(true);
            // FindObjectOfType<cutscenebridge>().turret6.gameObject.SetActive(true);
            // FindObjectOfType<cutscenebridge>().turret7.gameObject.SetActive(true);
            // FindObjectOfType<cutscenebridge>().turret8.gameObject.SetActive(true);
            // FindObjectOfType<cutscenebridge>().turret9.gameObject.SetActive(true);
            // FindObjectOfType<cutscenebridge>().turret10.gameObject.SetActive(true);
            // FindObjectOfType<cutscenebridge>().turret11.gameObject.SetActive(true);
        }
        else if (other.tag == "hitbox3")
        {
            //disable the previous
            FindObjectOfType<cutscenebridge>().turret4.gameObject.SetActive(false);
            FindObjectOfType<cutscenebridge>().turret5.gameObject.SetActive(false);
            FindObjectOfType<cutscenebridge>().turret6.gameObject.SetActive(false);
            FindObjectOfType<cutscenebridge>().turret7.gameObject.SetActive(false);
            FindObjectOfType<cutscenebridge>().turret8.gameObject.SetActive(false);
            FindObjectOfType<cutscenebridge>().turret9.gameObject.SetActive(false);
            FindObjectOfType<cutscenebridge>().turret10.gameObject.SetActive(false);
            FindObjectOfType<cutscenebridge>().turret11.gameObject.SetActive(false);

            //enable the next
            FindObjectOfType<cutscenebridge>().turret12.gameObject.SetActive(true);
            FindObjectOfType<cutscenebridge>().turret13.gameObject.SetActive(true);
        }
        else if (other.tag == "hitbox4")
        {
            //disable the previous
            FindObjectOfType<cutscenebridge>().turret12.gameObject.SetActive(false);
            FindObjectOfType<cutscenebridge>().turret13.gameObject.SetActive(false);

            //enable the next
            FindObjectOfType<cutscenebridge>().turret14.gameObject.SetActive(true);
            FindObjectOfType<cutscenebridge>().turret15.gameObject.SetActive(true);
            FindObjectOfType<cutscenebridge>().turret16.gameObject.SetActive(true);
            FindObjectOfType<cutscenebridge>().turret18.gameObject.SetActive(true);
            FindObjectOfType<cutscenebridge>().turret19.gameObject.SetActive(true);
            FindObjectOfType<cutscenebridge>().turret20.gameObject.SetActive(true);

            FindObjectOfType<cutscenebridge>().turret21.gameObject.SetActive(true);
            FindObjectOfType<cutscenebridge>().turret22.gameObject.SetActive(true);
            FindObjectOfType<cutscenebridge>().turret23.gameObject.SetActive(true);
            FindObjectOfType<cutscenebridge>().turret24.gameObject.SetActive(true);
            FindObjectOfType<cutscenebridge>().turret25.gameObject.SetActive(true);
            FindObjectOfType<cutscenebridge>().turret26.gameObject.SetActive(true);
        }
        else if (other.tag == "hitbox5")
        {
            //disable the previous
            FindObjectOfType<cutscenebridge>().turret14.gameObject.SetActive(false);
            FindObjectOfType<cutscenebridge>().turret15.gameObject.SetActive(false);
            FindObjectOfType<cutscenebridge>().turret16.gameObject.SetActive(false);
            FindObjectOfType<cutscenebridge>().turret18.gameObject.SetActive(false);
            FindObjectOfType<cutscenebridge>().turret19.gameObject.SetActive(false);
            FindObjectOfType<cutscenebridge>().turret20.gameObject.SetActive(false);

            FindObjectOfType<cutscenebridge>().turret21.gameObject.SetActive(false);
            FindObjectOfType<cutscenebridge>().turret22.gameObject.SetActive(false);
            FindObjectOfType<cutscenebridge>().turret23.gameObject.SetActive(false);
            FindObjectOfType<cutscenebridge>().turret24.gameObject.SetActive(false);
            FindObjectOfType<cutscenebridge>().turret25.gameObject.SetActive(false);
            FindObjectOfType<cutscenebridge>().turret26.gameObject.SetActive(false);

            //enable the next
            FindObjectOfType<cutscenebridge>().turret27.gameObject.SetActive(true);
            FindObjectOfType<cutscenebridge>().turret28.gameObject.SetActive(true);
            FindObjectOfType<cutscenebridge>().turret29.gameObject.SetActive(true);
            FindObjectOfType<cutscenebridge>().turret30.gameObject.SetActive(true);
            FindObjectOfType<cutscenebridge>().turret31.gameObject.SetActive(true);
            FindObjectOfType<cutscenebridge>().turret32.gameObject.SetActive(true);
        }
        else if (other.tag == "endbox")
        {
            //enable the next
            FindObjectOfType<cutscenebridge>().turret27.gameObject.SetActive(false);
            FindObjectOfType<cutscenebridge>().turret28.gameObject.SetActive(false);
            FindObjectOfType<cutscenebridge>().turret29.gameObject.SetActive(false);
            FindObjectOfType<cutscenebridge>().turret30.gameObject.SetActive(false);
            FindObjectOfType<cutscenebridge>().turret31.gameObject.SetActive(false);
            FindObjectOfType<cutscenebridge>().turret32.gameObject.SetActive(false);

            StartCoroutine(FadeAudio.FadeOut(mucis_loud, 2f));
            heartbeat.Play();
        }
    }

    //start turtorial
    private IEnumerator guntut()
    {
        tuto.SetTrigger("start");
        yield return new WaitForSeconds(6.9f);
        tuto.SetTrigger("idle");
    }
}
