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

    public static void SaveRunnerData(PlayerControllerRunner player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/runner.out";
        FileStream stream = new FileStream(path, FileMode.Create);
        RunnerData data = new RunnerData(player);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static RunnerData LoadRunnerData()
    {
        string path = Application.persistentDataPath + "/runner.out";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            RunnerData data = formatter.Deserialize(stream) as RunnerData;
            stream.Close();

            return data;
        }
        else
        {
            return null;
        }
    }

    public static void SaveVolumeData(MenuInteraction settings)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/settings.out";
        FileStream stream = new FileStream(path, FileMode.Create);
        VolumeData data = new VolumeData(settings);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static VolumeData LoadVolumeData()
    {
        string path = Application.persistentDataPath + "/settings.out";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            VolumeData data = formatter.Deserialize(stream) as VolumeData;
            stream.Close();

            return data;
        }
        else
        {
            return null;
        }
    }
}
