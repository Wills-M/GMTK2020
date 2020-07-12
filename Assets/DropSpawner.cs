using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropSpawner : MonoBehaviour
{
    Vector3 startScale;
    Vector3 startPosition;
    Vector3 finalPosition;
    float startradius;
    public bool autoStart = false;
    private void OnEnable()
    {
        if (autoStart) {
            BeginDripping();
        }
    }
    public Transform scoopPparent;
    // Start is called before the first frame update
    public void BeginDripping()
    {
        drippin = true;
        startradius= radius;
        startPosition = scoopPparent.position;
        finalPosition = (radius * (.8f)) * Vector3.down + startPosition;
        startScale = scoopPparent.localScale;
        StartCoroutine(SpawnDropsLoop());
    }

    public float mass = 100f;
    public float lossPerDrop = 3f;

    public float lerp;
    bool drippin = false;
    private void Update()
    {
        lerp = Mathf.InverseLerp(0f, 100f, mass);
        scoopPparent.localScale = Vector3.Lerp(startScale * .2f, startScale, lerp);
        scoopPparent.localPosition = Vector3.Lerp(finalPosition, startPosition, lerp);
        radius = Mathf.Lerp(startradius*.2f, startradius, lerp);
        if (lerp <= 0f) {
            drippin = false;
            StopAllCoroutines();
            StartCoroutine(ShrinkAndDisappear());
        }
    }
    
    public float radius;
    public float interval = 2f;
    public GameObject dropPrefab;
    IEnumerator SpawnDropsLoop() {
        while (true) {
            Vector3 newPositionOffset = (new Vector3(Random.Range(-1f, 1f), Random.Range(.3f, .9f), Random.Range(-1f, 1f))).normalized * radius;
            GameObject go = Instantiate(dropPrefab, transform.position + newPositionOffset, Quaternion.LookRotation(newPositionOffset));
            mass -= lossPerDrop;
            go.SetActive(true);
            yield return new WaitForSeconds(interval);
        }
    }

    public AnimationCurve ShrinkCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    public float ShrinkTime = 1f;
    private IEnumerator ShrinkAndDisappear() {
        float currTime = 0f;
        float lerpVal;
        Vector3 startScal = scoopPparent.localScale;
        WaitForEndOfFrame wfeof = new WaitForEndOfFrame();
        while (currTime < ShrinkTime) {
            currTime += Time.deltaTime;
            lerpVal = ShrinkCurve.Evaluate(Mathf.InverseLerp(0f, ShrinkTime, currTime));
            scoopPparent.localScale *= 1f - lerpVal;
            yield return wfeof;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
