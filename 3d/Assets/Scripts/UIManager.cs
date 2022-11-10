using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text timeText;
    float time;

    // Start is called before the first frame update
    void Start()
    {
        time = 90f;
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        timeText.text = timeFormat(time);
    }

    string timeFormat(float temp)
    {
        int m = (int)temp / 60;
        int s = (int)temp - m * 60;
        int ms = (int)(1000 * (temp - m * 60 - s));
        return string.Format("{0:00}:{1:00}:{2:000}", m, s, ms);
    }
}
