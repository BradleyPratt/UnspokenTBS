using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static class SaveLoad
    {
        public static List<Unit> savedGames = new List<Unit>(); 
    }

    public static void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/SaveGame.dat");
        GameObject[] tanks = GameObject.FindGameObjectsWithTag("Unit");
        Money money = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Money>();
        bf.Serialize(file, SaveLoad.savedGames);
        file.Close();         
    }

    public static void Load()
    {
        if(File.Exists(Application.persistentDataPath + "/SaveGame.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + ".SaveGame.dat", FileMode.Open);
            SaveLoad.savedGames = (List < Unit >) bf.Deserialize(file);
            file.Close();
        }
    }
}
