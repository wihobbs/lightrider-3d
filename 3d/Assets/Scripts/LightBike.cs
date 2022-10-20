using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBike : MonoBehaviour
{
    public LightBikeWheel[] F;
    public LightBikeWheel[] R;

    public Vector2 motorTorque;
    public Vector2 brakeTorque;
    public float steerAngle = 10f;

    [Space(15)]
    public float cameraPositionLerpRate = 10f;
    public float cameraRotationLerpRate = 7f;
    public float steerLerpRate = 1f;
    public float leanCoef = -10f;
    public float cameraLeanMultiplier = 0.5f;
    public float speedCompensation = 0.2f;
    public float gravityScale = 1.0f;

    [Space(15)]
    public Vector2 MouseSensitivity;
    public string horizontalAxisName = "Horizontal1";
    public string verticalAxisName = "Vertical1";
    
    
    private GameObject cameraAxis;
    private float currentSteer;
    private float currentSpeed;
    private Rigidbody rb;
    private static float globalGravity = -9.8f;
    private Vector3 offset;
    private bool forward;
    private float dot;


    
    // Start is called before the first frame update\
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraAxis = transform.Find("cameraAxis").gameObject;
        cameraAxis.transform.parent = null;
        forward = true;
    }

    // Update is called once per frame
    void Update()
    {
        currentSteer = Mathf.Lerp(currentSteer, Input.GetAxis(horizontalAxisName), steerLerpRate * Time.fixedDeltaTime);
        
        currentSpeed = rb.velocity.magnitude;
        float veloComp = rb.velocity.magnitude > 35f ? Mathf.Clamp((40f - rb.velocity.magnitude) * 0.2f, 0f, 1f) : 1;
        
        int gear = forward ? 1 : -1;
        float modifiedVerticalInput = Input.GetAxis(verticalAxisName) * gear;
        
        foreach (LightBikeWheel F in F)
        {
            F.motorTorque = Mathf.Clamp(modifiedVerticalInput, 0f, 1f) * motorTorque.x * veloComp * gear;
            F.brakeTorque = Mathf.Clamp(-modifiedVerticalInput, 0f, 1f) * brakeTorque.x;
            F.steerAngle = currentSteer * steerAngle;
        }
        foreach (LightBikeWheel R in R) {
            R.motorTorque = Mathf.Clamp(modifiedVerticalInput, 0f, 1f) * motorTorque.y * veloComp * gear;
            R.brakeTorque = Mathf.Clamp(-modifiedVerticalInput, 0f, 1f) * brakeTorque.y;
        }

        offset += new Vector3(Input.GetAxis("Mouse Y") * MouseSensitivity.y * Input.GetAxis("Fire2"), Input.GetAxis("Mouse X") * MouseSensitivity.x * Input.GetAxis("Fire2"), 0f);
        offset = offset * Input.GetAxis("Fire2");

        dot = Vector3.Dot(rb.velocity, transform.forward);

        if (modifiedVerticalInput < 0 && dot * gear < 0.5f)
            forward = !forward;
    }

    void FixedUpdate(){
        cameraAxis.transform.position = Vector3.Lerp(cameraAxis.transform.position, transform.position, cameraPositionLerpRate * Time.fixedDeltaTime);
        float temp = cameraAxis.transform.eulerAngles.z > 180f ? cameraAxis.transform.eulerAngles.z - 360f : cameraAxis.transform.eulerAngles.z;
        cameraAxis.transform.rotation = Quaternion.Lerp(cameraAxis.transform.rotation, transform.rotation * Quaternion.Euler(offset) * Quaternion.Euler(new Vector3(0f, 0f, -temp * cameraLeanMultiplier)), cameraRotationLerpRate * Time.fixedDeltaTime);
        Vector3 gravity = globalGravity * gravityScale * Vector3.up;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, currentSteer * leanCoef * Mathf.Clamp(rb.velocity.magnitude * 0.05f, 0f, 1f));
        rb.AddForce(gravity, ForceMode.Acceleration);
    }
}
