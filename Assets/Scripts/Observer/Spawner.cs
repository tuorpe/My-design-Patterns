using UnityEngine;
using System;
using Observer;
using Random = UnityEngine.Random;

[RequireComponent(typeof(BoxCollider))]
public class Spawner : MonoBehaviour
{
    [SerializeField] private Obstacle[] objectsToSpawn;
    [SerializeField, Range(1, 100)] private int numberOfObjectsToSpawn = 10;
    [SerializeField, Range(0.1f, 10f)] private float spawnInterval = 1f;
    private BoxCollider _spawnArea;
    private float _spawnTimer;
    public event Action<Obstacle> ItemSpawned;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _spawnArea = GetComponent<BoxCollider>();
    }
    
    // Update is called once per frame
    private void Update()
    {
        _spawnTimer += Time.deltaTime;
        if (!(_spawnTimer >= spawnInterval))
            return;
        InstantiateObject();
        _spawnTimer = 0f;
    }
    
    private void InstantiateObject()
    {
        if (objectsToSpawn.Length == 0)
        {
            // Debug.LogWarning("No objects assigned to spawn.");
            return;
        }
        var spawnPosition = _spawnArea.center + new Vector3(
            Random.Range(_spawnArea.bounds.min.x, _spawnArea.bounds.max.x),
            Random.Range(_spawnArea.bounds.min.y, _spawnArea.bounds.max.y),
            Random.Range(_spawnArea.bounds.min.z, _spawnArea.bounds.max.z)
        );
        // Get a random object from the array
        var randomObject = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];
        var newItem = Instantiate(randomObject, spawnPosition, Quaternion.identity, transform);
        // Notify subscribers that an item has been spawned
        ItemSpawned?.Invoke(newItem);
    }
}
