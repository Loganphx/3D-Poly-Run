using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Obstacle[] prefabs;
    [SerializeField] private List<Obstacle> spawnedObstacles;

    [SerializeField] private Vector3 spawnPosition = new Vector3(25, 0, 0);
    // Start is called before the first frame update

    private float            startDelay = 2f;
    private float            repeatRate = 2f;
    private float leftBound = -15f;
    
    public void StartGame()
    {
        spawnedObstacles = new List<Obstacle>();
        InvokeRepeating(nameof(SpawnObstacle), startDelay, repeatRate);
    }

    private void FixedUpdate()
    {
        foreach (var obstacle in spawnedObstacles)
        {
            if (transform.position.x < leftBound)
            {
                obstacle.OnDeath.Invoke(obstacle);
                obstacle.enabled = false;
                Destroy(gameObject);
            }
        }
    }

    public void GameOver()
    {
        CancelInvoke(nameof(SpawnObstacle));
        foreach (var obstacle in spawnedObstacles)
        {
            obstacle.enabled = false;
        }
    }
    private void SpawnObstacle()
    {
        var prefab = prefabs[Random.Range(0,prefabs.Length)];
        var obstacle = Instantiate(prefab, spawnPosition, prefab.transform.rotation);
        obstacle.OnDeath += DespawnObstacle;
        spawnedObstacles.Add(obstacle);
    }
    
    private void DespawnObstacle(Obstacle obstacle)
    {
        spawnedObstacles.Remove(obstacle);
    }
}
