using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardTargetPoint : MonoBehaviour
{
    public float driveForce = 1f;
    public Transform target;
    public VerletChain chain;
    Rigidbody rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigidbody.AddForce(driveForce * (target.position-transform.position).normalized, ForceMode.Acceleration);
    }

    private void LateUpdate()
    {
        Vector3 delta = transform.position - chain.topPoint.position;
        transform.position = chain.topPoint.position + Mathf.Min(delta.magnitude, chain.fullLength) * delta.normalized;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(target.position, .02f);
    }
}
