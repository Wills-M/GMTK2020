using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerletChain : MonoBehaviour
{

    public float radius;
    public float gravity = -1f;
    public Transform[] _joints;
    public List<Joint> joints;
    public Transform topPoint;
    public Transform endPoint;
    public int kickIndex = 11;
    public float kickStrength = 1f;
    public DropSpawner[] iceCreamOrbs;
    public float fullLength {
        get {
            return radius * (float)(_joints.Length-1);
        }
    }
    private void Awake()
    {
        joints = new List<Joint>();
        joints.Add(new Joint(_joints[0], true));
        for (int i = 1; i < _joints.Length-1; i++) {
            joints.Add(new Joint(_joints[i]));
        }
        joints.Add(new Joint(_joints[_joints.Length-1], true));

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            joints[kickIndex].SetPendingKick(Vector3.up * kickStrength);
            StartCoroutine(Lick());
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Simulate();
        ConstrainForward();
        ConstrainBackward();
    }

    public float lickInterval = .04f;
    public float lickDecay = 2f;
    bool licking;
    IEnumerator Lick() {
        // for (int i = 0; i < (int)(joints.Count*.8f); i++) {
        //     yield return new WaitForSeconds(lickInterval);
        //     joints[i].SetPendingKick(Vector3.up * kickStrength);
        // }
        licking = true;
        joints[joints.Count-1].kinematic = false;
        // for (int i = (int)(joints.Count*.8f); i < joints.Count; i++) {
        //     yield return new WaitForSeconds(lickInterval);
        //     joints[i].SetPendingKick(Vector3.up * kickStrength);
        // }

            joints[kickIndex].SetPendingKick(Vector3.up * kickStrength);

        yield return new WaitForSeconds(lickDecay);
        joints[joints.Count-1].kinematic = true;
        licking = false;
    }

    private void ConstrainForward() {
        // constraint, skips the root joint
        joints[0].position = topPoint.position;
        if (!licking) joints[joints.Count-1].position = endPoint.position;
        else endPoint.position = joints[joints.Count-1].position;
        for (int i = 0; i < joints.Count-1; i++) {
            Joint first = joints[i];
            Joint second = joints[i+1];
            // Vector3 delta = first.position-second.position;
            // first.position = second.position + delta.normalized * Mathf.Min(delta.magnitude, radius);
            float dist = (first.position-second.position).magnitude;
            float error = dist-radius;
            Vector3 changeDir = (first.position-second.position).normalized;
            Vector3 changeAmount = changeDir * error;
            if (i != 0) {
                first.position -= changeAmount * .5f;
                second.position += changeAmount * .5f;
            } else {
                second.position += changeAmount;
            }
        }
    }

    private void ConstrainBackward() {
        // constraint, skips the root joint
        joints[0].position = topPoint.position;
        if (!licking) joints[joints.Count-1].position = endPoint.position;
        else endPoint.position = joints[joints.Count-1].position;
        for (int i = joints.Count-1; i > 0; i--) {
            Joint first = joints[i];
            Joint second = joints[i-1];
            // Vector3 delta = first.position-second.position;
            // first.position = second.position + delta.normalized * Mathf.Min(delta.magnitude, radius);
            float dist = (first.position-second.position).magnitude;
            float error = dist-radius;
            Vector3 changeDir = (first.position-second.position).normalized;
            Vector3 changeAmount = changeDir * error;
            if (licking || i != joints.Count-1) {
                first.position -= changeAmount * .5f;
                second.position += changeAmount * .5f;
            } else {
                second.position += changeAmount;
            }
        }
    }

    private void ConstrainOrbs () {
        for (int i = 0; i < joints.Count-1; i++) {
            for (int j = 0; j < iceCreamOrbs.Length; j++) {
                Vector3 delta = (joints[i].position - iceCreamOrbs[j].transform.position);
                joints[i].position = delta.normalized * Mathf.Max(iceCreamOrbs[j].radius, delta.magnitude);
            }
        }
    }
    // private void ConstrainBackward() {
    //     // constraint, skips the root joint
    //     for (int i = joints.Count-2; i >= 0; i--) {
    //         Vector3 delta = joints[i].position-joints[i+1].position;
    //         joints[i].position = joints[i+1].position + delta.normalized * Mathf.Min(delta.magnitude, radius);
    //     }
    // }

    Joint j;
    private void Simulate() {
        Vector3 forceGravity = new Vector3(0, gravity, 0f);
        for (int i = 0; i < joints.Count; i++) {
            j = joints[i];
            j.currentVelocity = (j.position-j.lastPosition)/Time.deltaTime + j.pendingKick;
            j.pendingKick = Vector3.zero;
            j.lastPosition = j.position;
            if (!j.kinematic) {
                j.position += j.currentVelocity*Time.deltaTime + forceGravity*Time.deltaTime;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        for (int i = 0; i < _joints.Length; i++) {
            Gizmos.DrawWireSphere(_joints[i].position, radius*.5f);
        }
    }

    public class Joint{
        public Vector3 lastPosition;
        public Vector3 lastVelocity;
        private Vector3 _currentVelocity;
        public bool kinematic = false;
        public float retracted = 0f;
        public Vector3 currentVelocity {
            get { return _currentVelocity;}
            set {
                lastVelocity = _currentVelocity;
                _currentVelocity = retracted > 0f ? Vector3.zero : value;
            }
        }
        public Vector3 retractedPosition;
        public Vector3 position {
            get {return _transform.position;}
            set {_transform.position = Vector3.Lerp(value, retractedPosition, retracted);}
        }
        public Transform _transform;
        public Vector3 pendingKick = Vector3.zero;
        public void SetPendingKick(Vector3 vel) {
            pendingKick = vel;
        }


        public Joint(Transform trsf, bool _kinematic = false) {
            _transform = trsf;
            currentVelocity = Vector3.zero;
            lastVelocity = Vector3.zero;
            lastPosition = trsf.position;
            kinematic = _kinematic;
        }
    }
}
