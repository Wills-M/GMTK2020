using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoopsDripDirector : MonoBehaviour
{
    public bool autoStart = false;
    private void OnEnable()
    {
        if (autoStart) BeginScoopsDripping();
    }
    // Start is called before the first frame update
    void BeginScoopsDripping()
    {
        StartCoroutine(DripRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public UnityEvent OnGameEnd;

    public DropSpawner dps1, dps2, dps3;
    IEnumerator DripRoutine() {
        dps1.BeginDripping();
        while (dps1.mass > 0) {
            yield return new WaitForEndOfFrame();
        }
        dps1.Shrink();
        dps2.BeginDripping();
        while (dps2.mass > 0) {
            yield return new WaitForEndOfFrame();
        }
        dps2.Shrink();
        dps3.BeginDripping();
        while (dps3.mass > 0) {
            yield return new WaitForEndOfFrame();
        }
        dps3.Shrink();
        OnGameEnd.Invoke();
    }
}
