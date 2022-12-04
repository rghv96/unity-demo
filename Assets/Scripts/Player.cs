using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private LayerMask playerMask;
    private bool jumpKeyPressed;
    private float horizontalInput;
    private Rigidbody rigidBodyComponent;

    // var csv;
    StringBuilder csv = new StringBuilder("Time,X,Y,Z\n");

    string filePath = "/Users/rghv/rough/Assets/Scripts/data.csv";


    StreamWriter motionData;
    // private bool isGrounded;


    // Start is called before the first frame update
    void Start()
    {
        rigidBodyComponent = GetComponent<Rigidbody>();
        InvokeRepeating("TrackData", 1.0f, 1.0f);
        File.AppendAllText(filePath, csv.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) {
            jumpKeyPressed = true;
        }
        horizontalInput = Input.GetAxis("Horizontal");
    }

    // called once every physics update
    private void FixedUpdate()
    {
        rigidBodyComponent.velocity = new Vector3(horizontalInput, rigidBodyComponent.velocity.y, 0);
          
        if(Physics.OverlapSphere(groundCheckTransform.position, 0.1f, playerMask).Length == 0) {
            return;
        }
        // if (!isGrounded) {
        //     return;
        // }
        if (jumpKeyPressed) {
            rigidBodyComponent.AddForce(Vector3.up * 7, ForceMode.VelocityChange);
            jumpKeyPressed = false;
        }
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == 7) {
            Destroy(other.gameObject); 
        }
    }

    void TrackData() {
        Vector3 position = transform.position;
        var newLine = string.Format("{0},{1},{2},{3}\n", 
        System.DateTime.Now, position.x, position.y, position.z);
        Debug.Log("Writing coordinates " + newLine);
        //csv.AppendLine(newLine);
        File.AppendAllText(filePath, newLine);
    }

    // private void OnCollisionEnter(Collision collision) {
    //     isGrounded = true;  
    // }

    // private void OnCollisionExit(Collision collision) {
    //     isGrounded = false;  
    // }
}
