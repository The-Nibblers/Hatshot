using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GunTurret : MonoBehaviour
{
   
    [Header("Values")]
    
    [SerializeField ]private float turnSpeed;
    [SerializeField] private float maxTurn;

    [SerializeField] private float lineOfSight;
    [SerializeField] private float maxSightLine;
    
    
    [Header("References")]
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


    //events
    private UnityEvent onSeePlayer;
    private UnityEvent onLosePlayer;

    
    //misscelanious
    private bool seePlayer = false;
    private bool isDestroyed = false;
    private bool isLeftBarrel = false;
    private bool isShooting = false;
    private fpscontroller fpscontroller;
    private GameObject player;
    private bool hasPlayedDetectionAudio = false; // New variable to track audio playback


    private void Start()
    {
        fpscontroller = FindObjectOfType<fpscontroller>();
        player = GameObject.FindGameObjectWithTag("Player");
        
        //initialize events
        onSeePlayer = new UnityEvent();
        onLosePlayer = new UnityEvent();
        
        onSeePlayer.AddListener(PlayerDetected);
        onLosePlayer.AddListener(PlayerLost);
    }
    
    private void Update() 
    {
        DetectPlayer();
        TurretIdleRotation();
    }

    private void TurretIdleRotation()
    {
        if (seePlayer && isDestroyed)
            return;
            
        transform.Rotate(0, turnSpeed, 0);
        
        if ((transform.rotation.eulerAngles.y > maxTurn && transform.rotation.eulerAngles.y < 180) ||
            (transform.rotation.eulerAngles.y < 360 - maxTurn && transform.rotation.eulerAngles.y > 180))
        {
            turnSpeed *= -1;
        }
    }
    
    private void DetectPlayer()
    {
        Vector3 sight = player.transform.position - transform.position;
        float dot = Vector3.Dot(sight, transform.right);

        //detect if the turret sees the player
        if (dot < lineOfSight && dot > -lineOfSight)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, (player.transform.position - transform.position).normalized,
                    out hit, maxSightLine) && hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                    onSeePlayer.Invoke();
            }
            else if (seePlayer)
            {
                onLosePlayer.Invoke();
            }
        }
    }
    
    public void PlayerDetected()
    {
        if (isDestroyed) return;
        
        if (!hasPlayedDetectionAudio)
        { 
            detectionAudio.Play();
            hasPlayedDetectionAudio = true;
        }
            
        seePlayer = true;
        transform.LookAt(player.transform);

        if (!isShooting)
        {
            StartCoroutine(Shoot());    
        }
    }
    
    public void PlayerLost()
    {
        if (isDestroyed) return;
        
        seePlayer = false;
        hasPlayedDetectionAudio = false;
        idleAudio.Play();   
    } 
    
    //shoot at the player, alternating gun barrels
     IEnumerator Shoot()
     {
         isShooting = true;
         
         yield return new WaitForSeconds(2f);
         
         if (!seePlayer || isDestroyed) 
         {
             isShooting = false;
             yield break;
         }
             while (seePlayer)
             {
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
             }
             isShooting = false;
     }

     //referenced in other script, death logic
   public void Explode()
    {
        isDestroyed = true;
        seePlayer = false;
        
        explosionAudio.Play();
        
        hitBoxes.tag = "Untagged";
        
        lightLeft.gameObject.SetActive(false);
        lightRight.gameObject.SetActive(false);
        
        this.enabled = false;
    } 

    private void OnDestroy()
    {
        onSeePlayer.RemoveAllListeners();
        onLosePlayer.RemoveAllListeners();
    }
}
