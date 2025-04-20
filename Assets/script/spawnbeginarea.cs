using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnbeginarea : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private float radius = 10;
    [SerializeField] private Vector3 minPosition;
    [SerializeField] private Vector3 maxPosition;
    
    private void Start()
    {
        StartCoroutine(time());
    }
    
    private IEnumerator time()
    {
        Vector3 randomPosition = new Vector3(
            Random.Range(minPosition.x, maxPosition.x),
            Random.Range(minPosition.y, maxPosition.y),
            Random.Range(minPosition.z, maxPosition.z)
        );
        
        int i = Random.Range(2, 5);

        yield return new WaitForSeconds(i);

        GameObject newHat = Instantiate(objectToSpawn, randomPosition, Quaternion.identity);
        newHat.tag = "Detect";
        StartCoroutine(time());
    }

}
