using UnityEngine;

namespace Observer
{
    public class Enemy : Obstacle
    {
        [SerializeField, Range(0.1f, 10f)] private float rotationSpeed = 2f;
        [SerializeField] private int scoreValue = 2;

        private void Start()
        {
            type = ObstacleType.Enemy;
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
}
