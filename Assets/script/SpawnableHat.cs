using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableHat : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Die", 10);
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }
}
