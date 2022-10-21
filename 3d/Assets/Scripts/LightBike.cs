using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBike : MonoBehaviour
{
    [System.Serializable]
    public struct MeshMaterialPair
    {
        public MeshRenderer mr;
        public int mat;
    }

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
    public float cameraOffsetMultiplier = 1f;
    public float cameraOffsetMultiplier2 = 1f;
    public float cameraOffsetBehavior = 1f;
    public float cameraLeanMultiplier = 0.5f;
    public float speedCompensation = 0.2f;
    public float gravityScale = 1.0f;
    public Vector2 cameraMinMax;

    [Space(15)]
    public Vector2 MouseSensitivity;
    public string horizontalAxisName = "Horizontal1";
    public string throttleAxis = "Vertical1";
    public string brakeAxis = "Vertical1";

    [Space(15)]
    public MeshMaterialPair[] lights;
    [ColorUsage(true, true)]
    public Color lightColor;

    public float throttleInput;
    public float brakeInput;
    
    private GameObject cameraAxis;
    private Camera camera;
    private float currentSteer;
    private float currentSteer2;
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
        camera = cameraAxis.transform.Find("Camera").GetComponent<Camera>();
        forward = true;

        foreach(MeshMaterialPair m in lights){
            //m.mr.materials[m.mat].color = lightColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentSteer = Mathf.Lerp(currentSteer, Input.GetAxis(horizontalAxisName) * (1 - speedCompensation * Mathf.Pow(Mathf.Clamp(rb.velocity.magnitude * 0.025f, 0f, 1f), 0.4f)), steerLerpRate * Time.fixedDeltaTime);
        currentSteer2 = Mathf.Lerp(currentSteer2, currentSteer, 7f * Time.fixedDeltaTime);

        currentSpeed = rb.velocity.magnitude;
        float veloComp = rb.velocity.magnitude > 35f ? Mathf.Clamp((40f - rb.velocity.magnitude) * 0.2f, 0f, 1f) : 1;
        
        int gear = forward ? 1 : -1;
        throttleInput = forward ? Input.GetAxis(throttleAxis) : Input.GetAxis(brakeAxis);
        brakeInput = forward ? Input.GetAxis(brakeAxis) : Input.GetAxis(throttleAxis);

        foreach (LightBikeWheel F in F)
        {
            F.motorTorque = throttleInput * motorTorque.x * veloComp * gear;
            F.brakeTorque = brakeInput * brakeTorque.x;
            F.steerAngle = currentSteer2 * steerAngle;
        }
        foreach (LightBikeWheel R in R) {
            R.motorTorque = throttleInput * motorTorque.y * veloComp * gear;
            R.brakeTorque = brakeInput * brakeTorque.y;
        }

        offset += new Vector3(Input.GetAxis("Mouse Y") * MouseSensitivity.y * Input.GetAxis("Fire2"), Input.GetAxis("Mouse X") * MouseSensitivity.x * Input.GetAxis("Fire2"), 0f);
        offset = offset * Input.GetAxis("Fire2");

        dot = Vector3.Dot(rb.velocity, transform.forward);

        camera.fieldOfView = Mathf.Pow((rb.velocity.magnitude * 0.025f), 2) * (cameraMinMax.y - cameraMinMax.x) + cameraMinMax.x;

        if (brakeInput > 0 && dot * gear < 0.5f)
            forward = !forward;
        
        foreach(MeshMaterialPair m in lights){
            m.mr.materials[m.mat].SetColor("_EmissiveColor", lightColor);
        }
    }

    void FixedUpdate(){
        cameraAxis.transform.position = Vector3.Lerp(cameraAxis.transform.position + transform.right * Mathf.Pow(Mathf.Abs(currentSteer2), cameraOffsetBehavior) * Mathf.Sign(currentSteer2) * cameraOffsetMultiplier * Mathf.Clamp(rb.velocity.magnitude * 0.05f, 0f, 1f) + transform.up * Mathf.Abs(currentSteer) * cameraOffsetMultiplier2 * Mathf.Clamp(rb.velocity.magnitude * 0.05f, 0f, 1f),
            transform.position,
            cameraPositionLerpRate * Time.fixedDeltaTime);
        float temp = cameraAxis.transform.eulerAngles.z > 180f ? cameraAxis.transform.eulerAngles.z - 360f : cameraAxis.transform.eulerAngles.z;
        cameraAxis.transform.rotation = Quaternion.Lerp(cameraAxis.transform.rotation, transform.rotation * Quaternion.Euler(offset) * Quaternion.Euler(new Vector3(0f, 0f, -temp * cameraLeanMultiplier)), cameraRotationLerpRate * Time.fixedDeltaTime);
        Vector3 gravity = globalGravity * gravityScale * Vector3.up;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, currentSteer2 * leanCoef * Mathf.Clamp(rb.velocity.magnitude * 0.05f, 0f, 1f));
        rb.AddForce(gravity, ForceMode.Acceleration);
    }
}
