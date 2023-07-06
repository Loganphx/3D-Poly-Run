using UnityEngine;

public class MoveLeft : MonoBehaviour, IMoveLeft
{
    [SerializeField] 
    private float speed = 30.0f;

    public Transform Transform => transform;
    public float Speed => speed;
}
