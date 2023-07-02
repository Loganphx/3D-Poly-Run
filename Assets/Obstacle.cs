using System;
using UnityEngine;

public class Obstacle : MonoBehaviour, IMoveLeft
{
    [SerializeField] private float speed;

    private void Update()
    {
        transform.Translate(Vector3.left * (Time.deltaTime * speed));
    }

    public Action<Obstacle> OnDeath { get; set; }

    public float Speed => speed;
}