using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenTrigger : MonoBehaviour
{
    [SerializeField]
    private Transform headReference;
    [SerializeField]
    private Transform head;

    [SerializeField]
    private AnimationCurve animationCurve;
    [SerializeField]
    private float animateTime;

    // On activate, transition game to end screen.
    private void Start()
    {
        StartCoroutine(MoveHead());
    }

    private IEnumerator MoveHead()
    {
        Vector3 startPos = head.position;
        Quaternion startRot = head.rotation;
        Vector3 endPos = headReference.position;
        Quaternion endRot = headReference.rotation;

        for (float timer = 0f;  timer < animateTime; timer += Time.deltaTime)
        {
            float x = timer / animateTime;
            float y = animationCurve.Evaluate(x);
            Vector3 newPos = Vector3.Lerp(startPos, endPos, y);
            Quaternion newRot = Quaternion.Lerp(startRot, endRot, y);

            head.position = newPos;
            head.rotation = newRot;
            yield return null;
        }

        head.position = headReference.position;
        head.rotation = headReference.rotation;
    }
}
