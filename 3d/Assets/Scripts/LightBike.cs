using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBike : MonoBehaviour
{
    public WheelCollider[] F;
    public WheelCollider[] R;
    public GameObject steerTransform;
    public GameObject susFTransform;
    public GameObject susRTransform;
    public GameObject wheelFTransform;
    public GameObject wheelRTransform;

    public Vector2 motorTorque;
    public Vector2 brakeTorque;
    public float steerAngle = 10f;
    public GameObject cameraAxis;
    public float cameraPositionLerpRate = 10f;
    public float cameraRotationLerpRate = 7f;
    public float steerLerpRate = 1f;
    public float leanCoef = -10f;
    public float cameraLeanMultiplier = 0.5f;

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
    public string horizontalAxisName = "Horizontal1";
    public string verticalAxisName = "Vertical1";
    
    // Start is called before the first frame update\
    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        currentSteer = Mathf.Lerp(currentSteer, Input.GetAxis(horizontalAxisName), steerLerpRate * Time.fixedDeltaTime);
        
        currentSpeed = rb.velocity.magnitude;
        float veloComp = rb.velocity.magnitude > 35f ? Mathf.Clamp((40f - rb.velocity.magnitude) * 0.2f, 0f, 1f) : 1;
        foreach (WheelCollider F in F)
        {
            F.motorTorque = Mathf.Clamp(Input.GetAxis(verticalAxisName), 0f, 1f) * motorTorque.x * veloComp;
            F.brakeTorque = Mathf.Clamp(-Input.GetAxis(verticalAxisName), 0f, 1f) * brakeTorque.x;
            F.steerAngle = currentSteer * steerAngle;
            wheelFTransform.gameObject.transform.Rotate(-F.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        }
        foreach (WheelCollider R in R) {
            R.motorTorque = Mathf.Clamp(Input.GetAxis(verticalAxisName), 0f, 1f) * motorTorque.y * veloComp;
            R.brakeTorque = Mathf.Clamp(-Input.GetAxis(verticalAxisName), 0f, 1f) * brakeTorque.y;
            wheelRTransform.gameObject.transform.Rotate(-R.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        }

        steerTransform.transform.localEulerAngles = new Vector3(currentSteer * steerAngle, 55f, 0f);

        offset += new Vector3(Input.GetAxis("Mouse Y") * MouseSensitivity.y * Input.GetAxis("Fire2"), Input.GetAxis("Mouse X") * MouseSensitivity.x * Input.GetAxis("Fire2"), 0f);
        offset = offset * Input.GetAxis("Fire2");

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
