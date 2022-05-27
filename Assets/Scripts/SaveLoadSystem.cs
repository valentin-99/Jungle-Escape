using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveLoadSystem
{
    public static void SaveSoloData (PlayerController player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/solo.out";
        
        // TRB SA CREEZ VARIABILA SI PENTRU NIVELUL CURENT, SAU NU, VERIFIC DACA
        // VARIABILA AIA ARRAY ESTE GOALA PE POZITIA N.
    }
}
