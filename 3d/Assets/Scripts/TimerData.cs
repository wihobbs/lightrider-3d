using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimerData
{
    // note that this gets saved twice
    // to do: do better!
    public float timeValue;

    public TimerData(){
        this.timeValue = Timer.timeValue;
    }
}
