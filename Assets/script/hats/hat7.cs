using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hat7 : MonoBehaviour
{
    private AudioSource audi;
    // Start is called before the first frame update
    void Start()
    {
        audi = GetComponent<AudioSource>();
        FindObjectOfType<turret7>().detec = audi;
    }

    public void enable()
    {
        audi.mute = false;
    }

    public IEnumerator disable()
    {
        if (FindObjectOfType<turret7>().seePlayer == false)
        {
            yield return new WaitForSeconds(2);
            if (FindObjectOfType<turret7>().seePlayer == false)
            {
                audi.mute = true;
            }
        }
    }

}
