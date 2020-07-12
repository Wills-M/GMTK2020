using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cone : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private bool invertHorizontal;
    [SerializeField]
    private bool invertVertical;

    private Transform transform;
    private Rigidbody rigidbody;

    // Start is called before the first frame update
    private void Start()
    {
        transform = GetComponent<Transform>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    //private void Update()
    //{
    //    if (Input.GetKey(KeyCode.W))
    //    {
    //        //Move(new Vector3(-moveIncrement, 0, 0));
    //    }
    //    if (Input.GetKey(KeyCode.S))
    //    {
    //        //Move(new Vector3(moveIncrement, 0, 0));
    //    }
    //    if (Input.GetKey(KeyCode.A))
    //    {
    //        //Move(new Vector3(0, 0, -moveIncrement));
    //    }
    //    if (Input.GetKey(KeyCode.D))
    //    {
    //        //Move(new Vector3(0, 0, moveIncrement));
    //    }
    //}

    private void FixedUpdate()
    {
        // Get vertical and horizontal input (WASD) and invert if necessary
        float vertical = Input.GetAxis("Vertical");
        vertical = invertVertical ? -vertical : vertical;
        float horizontal = Input.GetAxis("Horizontal");
        horizontal = invertHorizontal ? -horizontal : horizontal;

        // Add force to cone based on input
        Vector3 movement = new Vector3(vertical, 0, horizontal);
        rigidbody.AddForce(movement * speed);
    }

    private void Move(Vector3 offset)
    {
        Vector3 newPosition = transform.position;
        newPosition += offset;
        transform.position = newPosition;
    }
}
