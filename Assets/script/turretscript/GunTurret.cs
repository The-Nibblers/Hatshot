using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class GunTurret : MonoBehaviour
{
   
    [Header("Values")]
    
    [SerializeField ]private float turnSpeed;
    [SerializeField] private float maxTurn;

    [SerializeField] private float lineOfSight;
    [SerializeField] private float maxSightLine;
    
    
    [Header("References")]
    private GameObject player;
    [SerializeField] private GameObject hitBoxes;
    
    [SerializeField] private AudioSource idleAudio;
    [SerializeField] private AudioSource detectionAudio;
    [SerializeField] private AudioSource explosionAudio;
    
    
    [Header("References left")] 
    
    [SerializeField] private ParticleSystem particlesLeft;
    [SerializeField] private Light lightLeft;
    
    [SerializeField] private AudioSource gunLeft;
    
    
    
    [Header("References right")]

    [SerializeField] private ParticleSystem particlesRight;
    [SerializeField] private Light lightRight;
    
    [SerializeField] private AudioSource gunRight;


    [Header("Events")]
    [SerializeField] private UnityEvent onSeePlayer;
    [SerializeField] private UnityEvent onLosePlayer;

    private bool seePlayer = false;
    private bool isDestroyed = false;
    private bool isLeftBarrel = false;
    private bool isShooting = false;
    private fpscontroller fpscontroller;


    private void Awake()
    {
        fpscontroller = FindObjectOfType<fpscontroller>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void SeePlayer()
    {
        
        Vector3 sight = player.transform.position - transform.position;
        float dot = Vector3.Dot(sight, transform.right);

        //detect if the turret sees the player
        if (dot < lineOfSight && dot > -lineOfSight)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, (player.transform.position - transform.position).normalized,
                    out hit, maxSightLine) && hit.collider.CompareTag("Player"))
            {
                onSeePlayer.Invoke();
            }
            else if (seePlayer)
            {
                onLosePlayer.Invoke();
            }
        }
    }

    //assigned in unity editor for event
    public void PlayerSeen()
    {
        if (!isDestroyed)
        {
            detectionAudio.Play();
            seePlayer = true;
            transform.LookAt(player.transform);
            StartCoroutine(shoot());
        }
    }

    //assigned in unity editor for event
    public void PlayerLost()
    {
        if (!isDestroyed)
        {
            seePlayer = false;
            idleAudio.Play();   
        }
    } 
    
    
    void Update() 
    {
        SeePlayer();
        
        //idle rotation of turret
        if (!seePlayer && !isDestroyed)
        {
            transform.Rotate(0, turnSpeed, 0);
            if ((transform.rotation.eulerAngles.y > maxTurn && transform.rotation.eulerAngles.y < 180) ||
                (transform.rotation.eulerAngles.y < 360 - maxTurn && transform.rotation.eulerAngles.y > 180))
            {
                turnSpeed *= -1;
            }
        }
    }

    
    
    //shoot at the player, alternating gun barrels
     IEnumerator shoot()
     {
         yield return new WaitForSeconds(2f);
         
         if (seePlayer && !isShooting && !isDestroyed)
         {
             isShooting = true;
             fpscontroller.damaging();
             
             if (isLeftBarrel)
             {
                 
                 gunLeft.Play();
                 particlesLeft.Play();
                 lightLeft.gameObject.SetActive(true);
                 yield return new WaitForSeconds(0.06f);
                 particlesLeft.Stop();
                 lightLeft.gameObject.SetActive(false);
                 isLeftBarrel = false;
             }
             if (!isLeftBarrel)
             {
                 gunRight.Play();
                 particlesRight.Play();
                 lightRight.gameObject.SetActive(true);
                 yield return new WaitForSeconds(0.06f);
                 particlesRight.Stop();
                 lightRight.gameObject.SetActive(false);
                 isLeftBarrel = true;
             }

             isShooting = false;
             
         }
    }

     //referenced in other script, death logic
   public void explode()
    {
        isDestroyed = true;
        seePlayer = false;
        explosionAudio.Play();
        hitBoxes.tag = "Untagged";
        lightLeft.gameObject.SetActive(false);
        lightRight.gameObject.SetActive(false);
        this.enabled = false;
    }
}
