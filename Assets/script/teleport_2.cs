using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleport_2 : MonoBehaviour
{
    public Transform teleporTtarget;
    public GameObject thePlayer;

    private void OnTriggerEnter(Collider other)
    {
        thePlayer.transform.position = teleporTtarget.transform.position;
    }
}
