using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTrailCollider : MonoBehaviour
{
    public float destroyTime = 15f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
