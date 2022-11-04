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
    public float stability = 0.3f;
    public float speed = 2.0f;
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

    public GameObject boxCollider;
    public GameObject boxSpawnPosition;
    private Quaternion prevRotation;
    public int frameInterval = 4;
    public float boxPrevLerp = -0.2f;
    private int tracker = 0;
    private float accumulatedSize = 0;
    public GameObject explosion;
    public GameObject trail;
    public Transform respawn;
    public GameObject thisPlayer;
    
    private GameObject cameraAxis;
    private Camera camera;
    public float currentSteer;
    private float currentSteer2;
    private float currentSpeed;
    private Rigidbody rb;
    private static float globalGravity = -9.8f;
    private Vector3 offset;
    private bool forward;
    private float dot;

    // just something to recognize player 1 from player 2
    public int PLAYER_NUMBER;

    
    // Start is called before the first frame update\
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraAxis = transform.Find("cameraAxis").gameObject;
        cameraAxis.transform.parent = null;
        camera = cameraAxis.transform.Find("Camera").GetComponent<Camera>();
        forward = true;

        tracker = 0;
        accumulatedSize = 0;
        prevRotation = Quaternion.identity;

        foreach(MeshMaterialPair m in lights){
            //m.mr.materials[m.mat].color = lightColor;
        }
        // if load from save, load now
        if(SaveSystem.LOAD_FROM_SAVE){
            Debug.Log("LOADING FROM SAVE");
            Vector3 savedPosition;
            PlayerData savedData;
            if(this.PLAYER_NUMBER == 1){
                // load player 1
                savedData = SaveSystem.LoadPlayer(1);
            }else{
                // load player 2
                savedData = SaveSystem.LoadPlayer(2);
            }
            if(savedData == null){
                return;
            }
            savedPosition.x = savedData.position[0];
            savedPosition.y = savedData.position[1];
            savedPosition.z = savedData.position[2];

            this.transform.position = savedPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float tempSteerBackToCenter = Mathf.Abs(Input.GetAxis(horizontalAxisName)) < Mathf.Abs(currentSteer) ? 1f : 0f;
        currentSteer = Mathf.Lerp(currentSteer, Input.GetAxis(horizontalAxisName) * (1 - speedCompensation * Mathf.Pow(Mathf.Clamp(rb.velocity.magnitude * 0.025f, 0f, 1f), 0.4f)),
            (steerLerpRate + Mathf.Lerp(0, 2, tempSteerBackToCenter)) * Time.deltaTime);
        currentSteer2 = Mathf.Lerp(currentSteer2, currentSteer, 5f * Time.deltaTime);

        currentSpeed = rb.velocity.magnitude;
        float veloComp = rb.velocity.magnitude > 35f ? Mathf.Clamp((40f - rb.velocity.magnitude) * 0.2f, 0f, 1f) : 1;
        
        int gear = forward ? 1 : -1;
        throttleInput = forward ? Mathf.Pow(Input.GetAxis(throttleAxis), 3f) : Mathf.Pow(Input.GetAxis(brakeAxis), 3f);
        brakeInput = forward ? Mathf.Pow(Input.GetAxis(brakeAxis), 3f) : Mathf.Pow(Input.GetAxis(throttleAxis), 3f);

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

        RaycastHit hit;
        //if (Physics.Raycast(transform.position, rb.velocity.normalized - transform.up, out hit, 5f){
        //transform.up = Vector3.up;
        //}
        //transform.rotation = Quaternion.FromToRotation(transform.up, Vector3.up);

        offset += new Vector3(Input.GetAxis("Mouse Y") * MouseSensitivity.y * Input.GetAxis("Fire2"), Input.GetAxis("Mouse X") * MouseSensitivity.x * Input.GetAxis("Fire2"), 0f);
        offset = offset * Input.GetAxis("Fire2");

        dot = Vector3.Dot(rb.velocity, transform.forward);

        camera.fieldOfView = Mathf.Pow((rb.velocity.magnitude * 0.025f), 2) * (cameraMinMax.y - cameraMinMax.x) + cameraMinMax.x;

        if (brakeInput > 0 && dot * gear < 0.5f)
            forward = !forward;
        
        foreach(MeshMaterialPair m in lights){
            m.mr.materials[m.mat].SetColor("_EmissiveColor", lightColor);
        }
        
        tracker++;
        accumulatedSize += Time.deltaTime;
        if (tracker >= frameInterval && dot > 0){
            GameObject tempCollider = Instantiate(boxCollider, boxSpawnPosition.transform.position, Quaternion.LerpUnclamped(prevRotation, boxSpawnPosition.transform.rotation, boxPrevLerp));
            tempCollider.transform.localScale = new Vector3(tempCollider.transform.localScale.x, tempCollider.transform.localScale.y, dot * accumulatedSize);
            tempCollider.GetComponent<LightTrailCollider>().parent = this;
            accumulatedSize = 0;
            tracker = 0;
            prevRotation = boxSpawnPosition.transform.rotation;
        }
    }

    void FixedUpdate(){
        cameraAxis.transform.position = Vector3.Lerp(cameraAxis.transform.position + transform.right * Mathf.Pow(Mathf.Abs(currentSteer2), cameraOffsetBehavior) * Mathf.Sign(currentSteer2) * cameraOffsetMultiplier * Mathf.Clamp(rb.velocity.magnitude * 0.05f, 0f, 1f) + transform.up * Mathf.Abs(currentSteer) * cameraOffsetMultiplier2 * Mathf.Clamp(rb.velocity.magnitude * 0.05f, 0f, 1f),
            transform.position,
            cameraPositionLerpRate * Time.fixedDeltaTime);
        float temp = cameraAxis.transform.eulerAngles.z > 180f ? cameraAxis.transform.eulerAngles.z - 360f : cameraAxis.transform.eulerAngles.z;
        Quaternion targetRotation = Quaternion.LerpUnclamped(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0f), cameraLeanMultiplier);
        cameraAxis.transform.rotation = Quaternion.Lerp(cameraAxis.transform.rotation, targetRotation * Quaternion.Euler(offset), cameraRotationLerpRate * Time.fixedDeltaTime);
        Vector3 gravity = globalGravity * gravityScale * Vector3.up;
        RaycastHit hit;
        //if (Physics.Raycast(transform.position, rb.velocity.normalized - transform.up, out hit, 5f){
            //transform.up = 
        //}
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, currentSteer2 * leanCoef * Mathf.Clamp(rb.velocity.magnitude * 0.05f, 0f, 1f));
        rb.AddForce(gravity, ForceMode.Acceleration);

        Vector3 predictedUp = Quaternion.AngleAxis(
         rb.angularVelocity.magnitude * Mathf.Rad2Deg * stability / speed,
         rb.angularVelocity
     ) * transform.up;
        Vector3 torqueVector = Vector3.Cross(predictedUp, Vector3.up);
        rb.AddTorque(torqueVector * speed * speed);
    }

    void OnTriggerEnter(Collider collision){
        if (collision.gameObject.tag == "LightTrail")
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            //trail.transform.parent = null;
            //Destroy(trail, 15f);
            //Destroy(cameraAxis, 2f);
            Invoke("Respawn", 2f);
            gameObject.SetActive(false);

        }
    }

    void Respawn()
    {
        gameObject.SetActive(true);
        gameObject.transform.position = respawn.position;
        gameObject.transform.rotation = respawn.rotation;

        Debug.Log("respawned!");
    }

    public void Save(){
        SaveSystem.SavePlayer(this,this.PLAYER_NUMBER);
    }
}
