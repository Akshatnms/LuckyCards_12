using System;
using UnityEngine;
using Newtonsoft.Json;
public static class LocalDatabase
{

    static string KEY = "PlayerData";
    public static LocalData data;
    public static void LoadGame()
    {
        bool hasValidData = PlayerPrefs.HasKey(KEY) && !string.IsNullOrEmpty(PlayerPrefs.GetString(KEY))
            && PlayerPrefs.GetString(KEY) != "null";
        if (hasValidData)
        {
            string saveData = PlayerPrefs.GetString(KEY);
            Debug.Log(saveData);
            data = JsonConvert.DeserializeObject<LocalData>(saveData);
            return;
        }
        LocalData defautValue = new LocalData()
        {
            phone = string.Empty,
            email = string.Empty,
            password = string.Empty,
            username = SystemInfo.deviceModel,
            playerId = SystemInfo.deviceUniqueIdentifier,
            music=true,
            sound=true,
            isLogedIn=false,
            balance=10000f,
        };
        data = defautValue;
    }
    public static void SaveGame()
    {
        Debug.Log("Saving...");
        PlayerPrefs.SetString(KEY, JsonConvert.SerializeObject(data));
    }  
    public static void DeleteData()
    {
        LocalData defautValue = new LocalData()
        {
            phone = string.Empty,
            email = string.Empty,
            password = string.Empty,
            username = SystemInfo.deviceModel,
            playerId = SystemInfo.deviceUniqueIdentifier,
            music = true,
            sound = true,
            isLogedIn = false,
            balance = 10000f,
        };
        PlayerPrefs.SetString(KEY, JsonConvert.SerializeObject(defautValue));
    }

    public class LocalData
    {
        public string phone;
        public string username;
        public string password;
        public string email;
        public string playerId;
        public string user_id;
        public bool music;
        public bool sound;
        public bool isLogedIn;
        public float balance;

    }
}
