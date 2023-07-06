using System;
using UnityEngine;

public class Obstacle : MonoBehaviour, IMoveLeft
{
    [SerializeField] private float speed;
    
    public Action<Obstacle> OnDeath { get; set; }

    public Transform Transform => transform;
    public float Speed => speed;
}