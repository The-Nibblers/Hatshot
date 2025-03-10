using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turret15 : MonoBehaviour
{
    public bool seePlayer = false;
    public GameObject player;

    public GameObject hitbox;

    private float turnspeed = 3f;
    private float maxTurn = 180f;

    private float lineOfSight = 3.5f;
    private float maxSight = 100;

    public ParticleSystem left;
    public Light leftl;

    public ParticleSystem right;
    public Light rightl;

    public AudioSource gunLeft;
    public AudioSource gunRight;

    private bool lft = false;
    private bool rht = false;

    public AudioSource gay;
    public AudioSource detec;
    private int firstDetec = 0;
    private bool firstlook = false;

    private bool isDestroyed = false;
    public AudioSource boom;

    public GameObject ob3;

    // Update is called once per frame
    private void Awake()
    {
        leftl.gameObject.SetActive(false);
        rightl.gameObject.SetActive(false);

    }
    void Update()
    {
        if (!seePlayer && !isDestroyed)
        {
            transform.Rotate(0, turnspeed, 0);
            if ((transform.rotation.eulerAngles.y > maxTurn && transform.rotation.eulerAngles.y < 180) ||
                (transform.rotation.eulerAngles.y < 360 - maxTurn && transform.rotation.eulerAngles.y > 180))
            {
                turnspeed *= -1;
            }
        }
        else if (!isDestroyed)
        {
            transform.LookAt(player.transform);
            //check for the first detect
            if (firstDetec == 1)
            {
                gay.Play();
                firstDetec++;
            }
            StartCoroutine(shoot());
        }
        if (seePlayer == false && firstlook == false && !isDestroyed)
        {
            FindObjectOfType<hat15>().enable();
            detec.Play();
            StartCoroutine(FindObjectOfType<hat15>().disable());
        }

        Vector3 sight = player.transform.position - transform.position;
        float dot = Vector3.Dot(sight, transform.right);

        if (dot < lineOfSight && dot > -lineOfSight)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, (player.transform.position - transform.position).normalized, out hit, maxSight))
            {
                if (hit.collider.name == "Capsule")
                {
                    firstlook = true;
                    seePlayer = true;
                    firstDetec = 1;
                }
                else
                {
                    firstlook = false;
                    seePlayer = false;
                    firstDetec = 0;
                }
            }
            else
            {
                firstlook = false;
                seePlayer = false;
                firstDetec = 0;
            }
        }
    }

    IEnumerator shoot()
    {
        //wait befor shooting
        yield return new WaitForSeconds(2);
        if (seePlayer && rht == false && lft == false && !isDestroyed)
        {
            lft = true;
            //shot left
            gunLeft.Play();
            left.Play();
            leftl.gameObject.SetActive(true);
            FindObjectOfType<fpscontroller>().damaging();
            yield return new WaitForSeconds(0.06f);
            left.Stop();
            leftl.gameObject.SetActive(false);
            lft = false;
            if (seePlayer && lft == false && rht == false && !isDestroyed)
            {
                //shot right
                rht = true;
                gunRight.Play();
                right.Play();
                rightl.gameObject.SetActive(true);
                FindObjectOfType<fpscontroller>().damaging();
                yield return new WaitForSeconds(0.06f);
                right.Stop();
                rightl.gameObject.SetActive(false);
                rht = false;
            }
        }
    }
    IEnumerator goFuckYourself()
    {
        yield return new WaitForSeconds(2);
        detec.gameObject.SetActive(true);
    }

   public void explode()
    {
        isDestroyed = true;
        seePlayer = false;
        boom.Play();
        hitbox.tag = "Untagged";
        leftl.gameObject.SetActive(false);
        rightl.gameObject.SetActive(false);
        Destroy(GetComponent<turret15>());
    }
}
