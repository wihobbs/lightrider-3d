using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTrailCollider : MonoBehaviour
{
    public float destroyTime = 15f;
    public LightBike parent;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}
