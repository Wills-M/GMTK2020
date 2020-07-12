using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainPositionOverride : MonoBehaviour
{
    public VerletChain chain;
    [Range(0f, 1f)]
    public float retract = 0f;
    public Vector3 retractAxis = Vector3.forward;
    public Transform EndPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    float retractVelocity = 0f, retractSmoothTime = .6f, retractMaxSpeed = .5f;
    float retractTarget = 1f;
    void Update()
    {
        retractTarget = 1f-Vector3.Distance(transform.position, EndPoint.position)*1.11f / chain.fullLength;
        retract = Mathf.SmoothDamp(retract, retractTarget, ref retractVelocity, retractSmoothTime, retractMaxSpeed);
        for (int i = 0; i < chain._joints.Length; i++) {
            float retracted = Mathf.Clamp01(((retract * chain._joints.Length) - (i) + 3f)/3f);
            chain.joints[i].retracted = retracted;
            if (retracted > 0f) {
                chain.joints[i].retractedPosition = RetractedPosition(i);
            }
        }
    }

    Vector3 RetractedPosition(int index) {
        Vector3 tipPos = retract * chain._joints.Length * chain.radius * retractAxis + transform.position;
        Vector3 finalPos = index * chain.radius * -retractAxis + tipPos;
        return finalPos;
    }
}
