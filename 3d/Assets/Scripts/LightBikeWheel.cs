using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBikeWheel : MonoBehaviour
{
    private WheelCollider wheel;

    public Transform steerTransform;
    public Transform susTransform;
    public Transform wheelTransform;
    
    [HideInInspector]
    public bool grounded;

    public float AngleConst;

    [HideInInspector]
    public float motorTorque;
    [HideInInspector]
    public float brakeTorque;
    [HideInInspector]
    public float steerAngle;

    [SerializeField]
    private float currentSuspensionCompression;

    // Start is called before the first frame update
    void Start()
    {
        wheel = GetComponent<WheelCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        wheelTransform.gameObject.transform.Rotate(wheel.rpm / 60 * 360 * Time.deltaTime, 0, 0);

        wheel.motorTorque = motorTorque;
        wheel.brakeTorque = brakeTorque;
        wheel.steerAngle = steerAngle;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, wheel.suspensionDistance + wheel.radius) ) {
            currentSuspensionCompression = wheel.suspensionDistance - (hit.point - transform.position).magnitude + wheel.radius;
            grounded = true;
        } else {
            currentSuspensionCompression = 0f;
            grounded = false;
        }

        susTransform.position = transform.position - (transform.up * (wheel.suspensionDistance - currentSuspensionCompression)) + (wheel.suspensionDistance - currentSuspensionCompression) * AngleConst * transform.forward;


        steerTransform.transform.localEulerAngles = new Vector3(steerTransform.transform.localEulerAngles.x, steerAngle, steerTransform.transform.localEulerAngles.z);
    }
}
