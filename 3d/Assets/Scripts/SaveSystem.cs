using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    /*
    TO DO: Format this to be nicer. Only care about functionality rn.
    */
    public static bool LOAD_FROM_SAVE = false;

    // player number either 1 or 2
    public static void SaveTime(){
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/time.bindata";
        FileStream stream = new FileStream(path,FileMode.Create);
        TimerData data = new TimerData(PauseMenu.timeValue);
        // write data
        formatter.Serialize(stream,data);
        Debug.Log("Saved to: " + path);

        stream.Close();
    }

    public static void DeleteSave(){
        string timePath = Application.persistentDataPath + "/time.bindata";
        string player1Path = Application.persistentDataPath + "/player1.bindata";
        string player2Path = Application.persistentDataPath + "/player2.bindata";

        if(File.Exists(timePath)){
            File.Delete(timePath);
        }
        if(File.Exists(player1Path)){
            File.Delete(player1Path);
        }
        if(File.Exists(player2Path)){
            File.Delete(player2Path);
        }
    }

    public static TimerData LoadTime(){
        string path = Application.persistentDataPath + "/time.bindata";
        Debug.Log(path);
        if(File.Exists(path)){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path,FileMode.Open);
            // read from stream
            TimerData data = formatter.Deserialize(stream) as TimerData;
            // Close the file stream
            stream.Close();

            return data;
        }else{
            Debug.LogError("Save file not found in " +path);
            return null;
        }
    }

    public static bool TimeSaved(){
        // return the path of the string for time
        string path = Application.persistentDataPath + "/time.bindata";
        return File.Exists(path);
    }
    public static void SavePlayer(LightBike player,int playerNum){
        BinaryFormatter formatter = new BinaryFormatter();
        string path;
        if(playerNum == 1){
            path = Application.persistentDataPath + "/player1.bindata";
        }else{
            path = Application.persistentDataPath + "/player2.bindata";
        }
        FileStream stream = new FileStream(path,FileMode.Create);
        PlayerData data = new PlayerData(player);
        formatter.Serialize(stream,data);
        stream.Close();
    }
    public static PlayerData LoadPlayer(int playerNum){
        string path;
        if(playerNum == 1){
            path = Application.persistentDataPath + "/player1.bindata";
        }else{
            path = Application.persistentDataPath + "/player2.bindata";
        }
        if(File.Exists(path)){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path,FileMode.Open);
            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return data;
        }else{
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

}
