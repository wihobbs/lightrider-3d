using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float[] position;
    public int otherPlayerElimCount;

    public PlayerData(LightBike player){
        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
        otherPlayerElimCount= int.Parse(player.otherPlayerElimText.text);
    }
}