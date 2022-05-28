using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveLoadSystem
{
    public static void SaveSoloData (PlayerController player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/solo.out";
        FileStream stream = new FileStream(path, FileMode.Create);
        SoloData data = new SoloData(player);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SoloData LoadSoloData()
    {
        string path = Application.persistentDataPath + "/solo.out";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SoloData data = formatter.Deserialize(stream) as SoloData;
            stream.Close();

            return data;
        }
        else
        {
            return null;
        }
    }
}
