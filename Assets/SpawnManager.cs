using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Obstacle[] prefabs;
    [SerializeField] private List<Obstacle> spawnedObstacles;
    [SerializeField] private List<IMoveLeft> moveLefts;

    [SerializeField] private Vector3 spawnPosition = new Vector3(25, 0, 0);

    [SerializeField] private float startDelay = 2f;
    [SerializeField] private float repeatRate = 2f;
    [SerializeField] private float leftBound = -15f;
    [SerializeField] public bool accelerate;
    
    public void StartGame(DifficultyTypes difficulty)
    {
        switch (difficulty)
        {
            case DifficultyTypes.Easy:
                repeatRate = 2f;
                break;
            case DifficultyTypes.Medium:
                repeatRate = 1.5f;
                break;
            case DifficultyTypes.Hard:
                repeatRate = 1f;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
        }
        spawnedObstacles = new List<Obstacle>();
        moveLefts = new List<IMoveLeft>();
        InvokeRepeating(nameof(SpawnObstacle), startDelay, repeatRate);

        foreach (var rootGameObject in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            var moveLeft = rootGameObject.GetComponentInChildren<IMoveLeft>();
            if (moveLeft != null) moveLefts.Add(moveLeft);
        }

        enabled = true;
    }

    private void FixedUpdate()
    {
        foreach (var obstacle in spawnedObstacles)
        {
            if (transform.position.x < leftBound)
            {
                obstacle.OnDeath.Invoke(obstacle);
                obstacle.enabled = false;
                Destroy(obstacle.gameObject);
            }
        }

    }

    private void Update()
    {
        foreach (var moveLeft in moveLefts)
        {
            if (accelerate) moveLeft.Transform.Translate(Vector3.left * (Time.deltaTime * moveLeft.Speed * 1.5f));
            else moveLeft.Transform.Translate(Vector3.left * (Time.deltaTime * moveLeft.Speed));
        }
    }

    public void GameOver()
    {
        CancelInvoke(nameof(SpawnObstacle));
        foreach (var obstacle in spawnedObstacles)
        {
            obstacle.enabled = false;
        }

        enabled = false;
    }
    private void SpawnObstacle()
    {
        var prefab = prefabs[Random.Range(0,prefabs.Length)];
        var obstacle = Instantiate(prefab, spawnPosition, prefab.transform.rotation);
        obstacle.OnDeath += DespawnObstacle;
        spawnedObstacles.Add(obstacle);
        moveLefts.Add(obstacle);
    }
    
    private void DespawnObstacle(Obstacle obstacle)
    {
        spawnedObstacles.Remove(obstacle);
    }
}
