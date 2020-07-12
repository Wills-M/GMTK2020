using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    public Transform projectionPoint;
    public Transform dropBall;

    public float gravity = -.5f;
    
    public LayerMask mask; 
    public LayerMask coneMask; 

    Vector3 projectPosition;
    Vector3 projectDirection;
    private void OnEnable()
    {
        // initialize the projectionposition to match projectionpoint transform. never use that transform again
        projectPosition = Vector3.ProjectOnPlane((projectionPoint.position-transform.position), Vector3.up) + transform.position;
        // Debug.DrawLine(transform.position, projectPosition, Color.red, 5f);
        projectDirection = (transform.position - projectPosition).normalized;
        StartCoroutine(Drip());
    }

    public AnimationCurve EmergeCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    public float EmergeTime = 1f;
    // Update is called once per frame
    private  IEnumerator Drip()
    {
        float currTime = 0f;
        float lerpVal;
        Vector3 startPos = dropBall.localPosition;
        WaitForEndOfFrame wfeof = new WaitForEndOfFrame();
        while (currTime < EmergeTime) {
            currTime += Time.deltaTime;
            lerpVal = EmergeCurve.Evaluate(Mathf.InverseLerp(0f, EmergeTime, currTime));
            dropBall.localPosition = Vector3.Lerp(startPos, Vector3.zero, lerpVal);
            yield return wfeof;
        }
        bool dripping = true;
        while (dripping) {// move the projection point dowward so it can drip
            projectPosition += gravity * Vector3.up * Time.deltaTime;

            RaycastHit hit;
            Debug.DrawRay(projectPosition, (projectDirection) * 1.5f, Color.red);
            if (Physics.Raycast(projectPosition, (projectDirection), out hit, 1.5f, coneMask)) {
                dripping = false;
                Fall();
            } else if (Physics.Raycast(projectPosition, (projectDirection), out hit, 1.5f, mask)) {
                transform.position = Vector3.Lerp(transform.position, hit.point, .9f); 
            } else {
                // wait for a new hit
            }
            yield return null;
        }
    }

    private void Fall() {
        gameObject.AddComponent<Rigidbody>();
    }
}
