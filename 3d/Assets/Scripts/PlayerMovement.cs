using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float moveSpeed = 5.0f;
    [SerializeField] float jumpForce = 5.0f;
    [SerializeField] KeyCode leftKey = KeyCode.A;
    [SerializeField] KeyCode rightKey = KeyCode.D;
    [SerializeField] KeyCode upKey = KeyCode.W;
    [SerializeField] KeyCode downKey = KeyCode.S;

    // Start is called before the first frame update
    void Start()
    {
        this.rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(this.leftKey)){
            UpdateVelocity(-1*this.moveSpeed,this.rb.velocity.y,this.rb.velocity.z);
        }
        if(Input.GetKey(this.rightKey)){
            UpdateVelocity(this.moveSpeed,this.rb.velocity.y,this.rb.velocity.z);
        }
        if(Input.GetKey(this.upKey)){
            UpdateVelocity(this.rb.velocity.x,this.rb.velocity.y,this.moveSpeed);
        }
        if(Input.GetKey(this.downKey)){
            UpdateVelocity(this.rb.velocity.x,this.rb.velocity.y,-1*this.moveSpeed);
        }
    }

    void UpdateVelocity(float floatX,float floatY,float floatZ){
        // helper func
        this.rb.velocity = new Vector3(floatX,floatY,floatZ);
    }
}
