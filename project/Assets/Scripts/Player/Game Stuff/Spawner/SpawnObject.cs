using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    private float spawnTimer = 0f;

    public List<GameObject> pool = new List<GameObject>();

    [SerializeField] private GameObject spawn;
    [SerializeField] private float spawnDelay;
    [SerializeField] private float destroyDelay;

    public void Spawn()
    {
        GameObject obj = Instantiate(spawn, transform.position, transform.rotation);

        pool.Add(obj);

        spawnTimer = Time.time + spawnDelay;
        Destroy(obj, destroyDelay);
    }

    void FixedUpdate()
    {
        if (spawnTimer < Time.time)
        {
            Spawn();
        }
        
    }
}
