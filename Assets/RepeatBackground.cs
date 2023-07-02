using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    private Vector3 startPosition;
    private float   repeatWidth;
    void Start()
    {
        startPosition = transform.position;
        repeatWidth   = transform.localScale.x * (GetComponent<BoxCollider>().size.x / 2f);
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x < startPosition.x - repeatWidth) transform.position = startPosition;
        
    }
}
