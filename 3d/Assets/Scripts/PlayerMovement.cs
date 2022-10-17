using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float moveSpeed = 5.0f;
    [SerializeField] float jumpForce = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        this.rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float horiInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");

        UpdateVelocity(horiInput * this.moveSpeed,rb.velocity.y,vertInput * this.moveSpeed);

        if(Input.GetButtonDown("Jump")){
            UpdateVelocity(rb.velocity.x,this.jumpForce,rb.velocity.z);
        }
    }

    void UpdateVelocity(float floatX,float floatY,float floatZ){
        // helper func
        this.rb.velocity = new Vector3(floatX,floatY,floatZ);
    }
}
