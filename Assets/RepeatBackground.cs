using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    private Transform _transform;
    private Vector3 _startPosition;
    private float   _repeatWidth;

    private void Awake()
    {
        _transform = transform;
    }

    private void Start()
    {
        _startPosition = _transform!.position;
        _repeatWidth   = _transform!.localScale.x * (GetComponent<BoxCollider>().size.x / 2f);
    }

    // Update is called once per frame
    private void Update()
    {
        if (transform.position.x < _startPosition.x - _repeatWidth)
        {
            transform.position = _startPosition;
        }
    }
}