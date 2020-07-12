using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoopsDripDirector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public DropSpawner dps1, dps2, dps3;
    IEnumerator DripRoutine() {
        dps1.BeginDripping();
        while (dps1.mass > 0) {
            yield return new WaitForEndOfFrame();
        }
        dps2.BeginDripping();
        while (dps2.mass > 0) {
            yield return new WaitForEndOfFrame();
        }
        dps3.BeginDripping();
        while (dps3.mass > 0) {
            yield return new WaitForEndOfFrame();
        }
    }
}
