using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    private float leftBound = -15f;

    [SerializeField] 
    private float speed = 30.0f;

    private void Update()
    {
        transform.Translate(Vector3.left * (Time.deltaTime * speed));
    }

    public float LeftBound => leftBound;
}
