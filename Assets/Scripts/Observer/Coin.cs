using Observer;
using UnityEngine;
public class Coin : Obstacle
{
    [SerializeField, Range(0.1f, 10f)] private float rotationSpeed = 2f;
    [SerializeField] private int scoreValue = 1;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        type = ObstacleType.Coin;
        value = scoreValue;
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime * 360f, Space.World);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) 
            return;
        Hit();
        Destroy(gameObject);
    }
}
