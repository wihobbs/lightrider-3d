using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBike : MonoBehaviour
{
    public WheelCollider[] F;
    public WheelCollider[] R;
    public Vector2 motorTorque;
    public Vector2 brakeTorque;
    public float steerAngle = 10f;
    public GameObject cameraAxis;
    public float cameraPositionLerpRate = 10f;
    public float cameraRotationLerpRate = 7f;
    public float steerLerpRate = 1f;
    public float leanCoef = -10f;
    [SerializeField]
    private float currentSteer;
    public float speedCompensation = 0.2f;
    [SerializeField]
    private float currentSpeed;
    private Rigidbody rb;
    public float gravityScale = 1.0f;
    static float globalGravity = -9.8f;
    public Vector2 MouseSensitivity;
    Vector3 offset;
    
    // Start is called before the first frame update\
    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        currentSteer = Mathf.Lerp(currentSteer, Input.GetAxis("Horizontal"), steerLerpRate * Time.fixedDeltaTime);
        
        currentSpeed = rb.velocity.magnitude;
        float veloComp = rb.velocity.magnitude > 35f ? Mathf.Clamp((40f - rb.velocity.magnitude) * 0.2f, 0f, 1f) : 1;
        foreach (WheelCollider F in F)
        {
            F.motorTorque = Mathf.Clamp(Input.GetAxis("Vertical"), 0f, 1f) * motorTorque.x * veloComp;
            F.brakeTorque = Mathf.Clamp(-Input.GetAxis("Vertical"), 0f, 1f) * brakeTorque.x;
            F.steerAngle = currentSteer * steerAngle;
        }
        foreach (WheelCollider R in R) {
            R.motorTorque = Mathf.Clamp(Input.GetAxis("Vertical"), 0f, 1f) * motorTorque.y * veloComp;
            R.brakeTorque = Mathf.Clamp(-Input.GetAxis("Vertical"), 0f, 1f) * brakeTorque.y;
        }

        //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, currentSteer * leanCoef);

        offset += new Vector3(Input.GetAxis("Mouse Y") * MouseSensitivity.y, Input.GetAxis("Mouse X") * MouseSensitivity.x, 0f);
    }

    void FixedUpdate(){
        cameraAxis.transform.position = Vector3.Lerp(cameraAxis.transform.position, transform.position, cameraPositionLerpRate * Time.fixedDeltaTime);
        cameraAxis.transform.rotation = Quaternion.Lerp(cameraAxis.transform.rotation, transform.rotation * Quaternion.Euler(offset), cameraRotationLerpRate * Time.fixedDeltaTime);
        Vector3 gravity = globalGravity * gravityScale * Vector3.up;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, currentSteer * leanCoef);

        rb.AddForce(gravity, ForceMode.Acceleration);
    }
}
