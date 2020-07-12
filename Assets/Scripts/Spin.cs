using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField]
    private float spinSpeed;

    private void FixedUpdate()
    {
        transform.Rotate(Vector3.up, spinSpeed);
    }
}
