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

    [Header("Player Variables")] 
    [SerializeField] private float maxWalkingSpeed = 11.5f;
    [SerializeField] private float currentWalkingSpeed = 11.5f;
    [SerializeField] private float minWalkingSpeed = 3.5f;
    [SerializeField] private float walkingSpeedIncrement;
    
    [SerializeField] private float jumpSpeed = 8.0f;
    [SerializeField] private float gravity = 15.0f;
    
    [SerializeField] private float lookSpeed = 2.0f;
    [SerializeField] private float lookXLimit = 90.0f;
    
    [SerializeField] private float doubleJumpSpeed = 120.0f;
    [SerializeField] private float delay = 0.5f;
    
    [SerializeField] private float coolDownPeriodInSeconds = 0.15f;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashSpeed;
    
    public float health = 100;

    [Header("references")]
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

    //die rigidbody
    [SerializeField] private GameObject pl1;
    [SerializeField] private GameObject pl2;
    [SerializeField] private GameObject pl5;

    //dash
    [SerializeField] private AudioSource dashsfx;

    //hat wearing
    [SerializeField] private GameObject doubleJumpHat;
    
    //shield script
    [SerializeField] private HeavyShield shield;

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;

    //misselanious
    private cutscenebridge turretholder;
    private Rigidbody rb;
    private Camera PlayerCamera;
    
    private float thehealth;
    
    private int canDash = 1;
    private int canFloat = 1;
    [HideInInspector] public bool canMove = true;


    private void Start()
    {
        turretholder = FindObjectOfType<cutscenebridge>();
        rb = pl2.GetComponent<Rigidbody>();
        PlayerCamera = Camera.main;
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //start turtorial
        StartCoroutine(guntut());
        
        //ui
        deahtText.gameObject.SetActive(false);
        deathButton.gameObject.SetActive(false);

        //get charachter controller
        characterController = GetComponent<CharacterController>();
        
        //enable normal hat
        doubleJumpHat.gameObject.SetActive(true);
    }

    private void Update()
    {
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        // Press Left Shift to run
        float curSpeedX = canMove ? (currentWalkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (currentWalkingSpeed) * Input.GetAxis("Horizontal") : 0;
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
                characterController.Move(moveDirection * (dashSpeed * Time.deltaTime));
                yield return null;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            shield.Defending.Invoke();
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            shield.TryShieldBreaking.Invoke();
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
        if (shield.isDefending)
        {
            shield.ShieldTakeDamage.Invoke();
        }
        else
        {
            CancelInvoke("RestoreWalkingSpeed");
            
            thehealth = health - 2f;
            health = thehealth;
            
            float thisWalkingSpeed = currentWalkingSpeed - walkingSpeedIncrement;

            if (thisWalkingSpeed >= minWalkingSpeed)
            {
                currentWalkingSpeed = thisWalkingSpeed;
            }
            else if (thisWalkingSpeed < minWalkingSpeed)
            {
                currentWalkingSpeed = minWalkingSpeed;
            }
            
            InvokeRepeating("RestoreWalkingSpeed", 1f, 0.5f);
        }
    }

    private void RestoreWalkingSpeed()
    {
        currentWalkingSpeed += walkingSpeedIncrement;
        
        if (currentWalkingSpeed >= maxWalkingSpeed)
        {
            currentWalkingSpeed = maxWalkingSpeed;
            CancelInvoke("RestoreWalkingSpeed");
        }
    }

    //restart on death
    private void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public float returnHealth()
    {
        return health;
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
        
        //turret en and disabling
        else if (other.tag == "hitbox1")
        {
            // Enable new turrets
            for (int i = 0; i < 3; i++)
                turretholder.turretArray[i].gameObject.SetActive(true);
        }
        else if (other.tag == "hitbox2")
        {
            // Disable previous turrets
            for (int i = 0; i < 3; i++)
                turretholder.turretArray[i].gameObject.SetActive(false);

            // Enable new turrets
            for (int i = 3; i <= 10; i++)
                turretholder.turretArray[i].gameObject.SetActive(true);
        }
        else if (other.tag == "hitbox3")
        {
            // Disable previous turrets
            for (int i = 3; i <= 10; i++)
                turretholder.turretArray[i].gameObject.SetActive(false);

            // Enable new turrets
            for (int i = 11; i <= 12; i++)
                turretholder.turretArray[i].gameObject.SetActive(true);
        }
        else if (other.tag == "hitbox4")
        {
            // Disable previous turrets
            for (int i = 11; i <= 12; i++)
                turretholder.turretArray[i].gameObject.SetActive(false);

            // Enable new turrets
            int[] indices = { 13, 14, 15, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 };
            foreach (int index in indices)
                turretholder.turretArray[index].gameObject.SetActive(true);
        }
        else if (other.tag == "hitbox5")
        {
            // Disable previous turrets
            int[] indices = { 13, 14, 15, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 };
            foreach (int index in indices)
                turretholder.turretArray[index].gameObject.SetActive(false);

            // Enable new turrets
            for (int i = 27; i <= 31; i++)
                turretholder.turretArray[i].gameObject.SetActive(true);
        }
        else if (other.tag == "endbox")
        {
            // Disable previous turrets
            for (int i = 27; i <= 31; i++)
                turretholder.turretArray[i].gameObject.SetActive(false);

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
