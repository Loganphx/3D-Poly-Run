using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeScale : MonoBehaviour
{
    [SerializeField] private Vector3 minScale;
    [SerializeField] private Vector3 maxScale;

    private void Start()
    {
        transform.localScale = new Vector3(
            UnityEngine.Random.Range(minScale.x, maxScale.x),
            UnityEngine.Random.Range(minScale.y, maxScale.y),
            UnityEngine.Random.Range(minScale.z, maxScale.z));
    }
}
