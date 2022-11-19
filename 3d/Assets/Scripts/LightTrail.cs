using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTrail : MonoBehaviour
{

    [ColorUsage(true, true)]
    public Color lightColor;
    public TrailRenderer[] trArray;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(TrailRenderer tr in trArray){
            tr.materials[0].color = lightColor;
        }
    }
}