using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    private PlayerController _playerController;
    private float leftBound = -15f;
        
    [SerializeField] 
    private float speed = 30.0f;
    
    // Start is called before the first frame update
    private void Awake()
    {
        _playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_playerController.gameOver) transform.Translate(Vector3.left * (Time.deltaTime * speed));

        if (transform.position.x < leftBound && gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
