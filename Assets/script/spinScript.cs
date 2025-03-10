using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spinScript : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 1f, 0f, Space.Self);
        transform.Rotate(1f, 0f, 0f, Space.Self);
        transform.Rotate(0f, 0f, 1f, Space.Self);
    }
}
