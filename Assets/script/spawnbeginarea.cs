using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnbeginarea : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float radius = 10;
    public Vector3 minPosition;
    public Vector3 maxPosition;
    public GameObject Thing;

    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(time());

        IEnumerator time()
        {
            Vector3 randomPosition = new Vector3(
        Random.Range(minPosition.x, maxPosition.x),
        Random.Range(minPosition.y, maxPosition.y),
        Random.Range(minPosition.z, maxPosition.z)
        );
            int i = Random.Range(5, 10);

            yield return new WaitForSeconds(i);
            Thing = Instantiate(objectToSpawn, randomPosition, Quaternion.identity);
            Thing.tag = "Detect";
            StartCoroutine(time());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
