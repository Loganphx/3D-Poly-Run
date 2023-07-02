using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs;

    [SerializeField] private Vector3 spawnPosition = new Vector3(25, 0, 0);
    // Start is called before the first frame update

    private PlayerController _playerController;
    private float            startDelay = 2f;
    private float            repeatRate = 2f;
    void Start()
    {
        InvokeRepeating(nameof(SpawnObstacle), startDelay, repeatRate);
        _playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void SpawnObstacle()
    {
        if (!_playerController.gameOver)
        { 
            var prefab = prefabs[Random.Range(0,prefabs.Length)];
            Instantiate(prefab, spawnPosition, prefab.transform.rotation);
        }
    }
}
