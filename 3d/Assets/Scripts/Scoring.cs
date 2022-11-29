using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoring : MonoBehaviour
{
    // Setting to public for now but should probably be private.
    public int TopBikeScore;
    public int BottomBikeScore;

    // Start is called before the first frame update
    void Start()
    {
        // initialize to 0
        // TopBikeScore = 0;
        // BottomBikeScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // TopBikeScore =
        print("Top Bike " + TopBikeScore + " Bottom Bike " + BottomBikeScore);
    }
}
