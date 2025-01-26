using System.Collections.Generic;
using UnityEngine;

public class ExtrasSpawner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> objectsToSpawn;

    [SerializeField]
    private List<Transform> spawnPositions;

    [SerializeField]
    private float minSpawnTime = 1.0f;

    [SerializeField]
    private float maxSpawnTime = 5.0f;

    private float nextSpawnTime;
    private bool objectCollected = true;

    void Start()
    {
        ScheduleNextSpawn();
    }

    void Update()
    {
        if (objectCollected && Time.time >= nextSpawnTime)
        {
            SpawnObject();
        }
    }

    private void ScheduleNextSpawn()
    {
        nextSpawnTime = Time.time + Random.Range(minSpawnTime, maxSpawnTime);
    }

    private void SpawnObject()
    {
        if (objectsToSpawn.Count == 0 || spawnPositions.Count == 0)
        {
            Debug.LogWarning("No objects to spawn or no spawn positions available.");
            return;
        }

        GameObject objectToSpawn = objectsToSpawn[Random.Range(0, objectsToSpawn.Count)];
        Transform spawnPosition = spawnPositions[Random.Range(0, spawnPositions.Count)];

        GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition);
        spawnedObject.transform.position = spawnPosition.position;
        spawnedObject.GetComponent<Collectible>().OnCollected.AddListener(OnObjectCollected);

        objectCollected = false;
    }

    private void OnObjectCollected()
    {
        objectCollected = true;
        ScheduleNextSpawn();
    }
}