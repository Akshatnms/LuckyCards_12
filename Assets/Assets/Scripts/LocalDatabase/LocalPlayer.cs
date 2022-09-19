using UnityEngine;

public static class LocalPlayer
{
    //private static string _playerId;
    //private static string _balance;
    
    //public static string playerId { get { return _playerId; } set { _playerId = value; } }
    //public static string balance { get { return _balance; } set { _balance = value; } }
    //public static string profilePic { get { return _profilePic; } set { _profilePic = value; } }
    //public static string userName { get { return _userName; } set { _userName = value; } }
    //public static bool isFbLogedIn { get { return _isLogedIn; } set { _isLogedIn = value; } }
    //public static int defautprofileIndex { get { return _defautprofileIndex; } set { _defautprofileIndex = value; } }


    //public static void LoadGame()
    //{
    //    PlayerPrefs.DeleteKey(LocalPlayerEnum.playerId.ToString());
    //    _playerId = PlayerPrefs.GetString(LocalPlayerEnum.playerId.ToString(), LocalPlayerDefaultValues.@null.ToString());
    //    _balance = PlayerPrefs.GetString(LocalPlayerEnum.balance.ToString(), ((int)LocalPlayerDefaultValues.tenThousand).ToString());
    //    _profilePic = PlayerPrefs.GetString(LocalPlayerEnum.profilePic.ToString(), LocalPlayerDefaultValues.noProfile.ToString());
    //    _isLogedIn = bool.Parse(PlayerPrefs.GetString(LocalPlayerEnum.isLogedIn.ToString(), LocalPlayerDefaultValues.@false.ToString()));
    //    _userName = PlayerPrefs.GetString(LocalPlayerEnum.userName.ToString(), "Guest: " + SystemInfo.deviceUniqueIdentifier);
    //    _defautprofileIndex = PlayerPrefs.GetInt(LocalPlayerEnum.defautprofileIndex.ToString(), 2);
        
    //}
    //public static void SaveGame()
    //{
    //    PlayerPrefs.SetString(LocalPlayerEnum.playerId.ToString(), _playerId);
    //    PlayerPrefs.SetString(LocalPlayerEnum.balance.ToString(), _balance);
    //    PlayerPrefs.SetString(LocalPlayerEnum.profilePic.ToString(), _profilePic);
    //    PlayerPrefs.SetString(LocalPlayerEnum.isLogedIn.ToString(), _isLogedIn.ToString());
    //    PlayerPrefs.SetString(LocalPlayerEnum.userName.ToString(), _userName.ToString());
    //    PlayerPrefs.SetInt(LocalPlayerEnum.defautprofileIndex.ToString(), _defautprofileIndex);
    //    PlayerPrefs.Save();
    //}

}

public class PlayerSavedData
{
    public string userName;
    public string balance;
    public string playerId;
    public bool isFbLogedIn;
}