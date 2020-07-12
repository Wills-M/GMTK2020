using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cone : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private bool invertHorizontal;
    
    private Transform transform;

    // Start is called before the first frame update
    private void Start()
    {
        transform = GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        //// Get vertical and horizontal input (WASD) and invert if necessary
        //float vertical = Input.GetAxis("Vertical");
        //vertical = invertVertical ? -vertical : vertical;
        //float horizontal = Input.GetAxis("Horizontal");
        //horizontal = invertHorizontal ? -horizontal : horizontal;

        //// Add force to cone based on input
        //Vector3 movement = new Vector3(vertical, 0, horizontal);
        //rigidbody.AddForce(movement * speed);



        // Get horizontal input (using A and D) and invert if necessary
        float horizontal = Input.GetAxis("Horizontal");
        horizontal = invertHorizontal ? -horizontal : horizontal;

        transform.Rotate(Vector3.up, speed * horizontal);
    }

    //private void Move(Vector3 offset)
    //{
    //    Vector3 newPosition = transform.position;
    //    newPosition += offset;
    //    transform.position = newPosition;
    //}
}
