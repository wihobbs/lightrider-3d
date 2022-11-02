using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    /*
    TO DO: Format this to be nicer. Only care about functionality rn.
    */

    // player number either 1 or 2
    public static void SaveTime(){
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/time.bindata";
        FileStream stream = new FileStream(path,FileMode.Create);
        TimerData data = new TimerData();
        // write data
        formatter.Serialize(stream,data);
        Debug.Log("Saved to: " + path);

        stream.Close();
    }

    public static TimerData LoadTime(){
        string path = Application.persistentDataPath + "/time.bindata";
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


}
