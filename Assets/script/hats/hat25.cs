using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hat25 : MonoBehaviour
{
    private AudioSource audi;
    // Start is called before the first frame update
    void Start()
    {
        audi = GetComponent<AudioSource>();
        FindObjectOfType<turret25>().detec = audi;
    }

    public void enable()
    {
        audi.mute = false;
    }

    public IEnumerator disable()
    {
        if (FindObjectOfType<turret25>().seePlayer == false)
        {
            yield return new WaitForSeconds(2);
            if (FindObjectOfType<turret25>().seePlayer == false)
            {
                audi.mute = true;
            }
        }
    }

}
