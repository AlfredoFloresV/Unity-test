using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    // Function to save a boolean variable to PlayerPrefs
    public static void SaveBool(string key, bool value)
    {
        int intValue = value ? 1 : 0;
        PlayerPrefs.SetInt(key, intValue);
        PlayerPrefs.Save();
    }

    // Function to retrieve a boolean variable from PlayerPrefs
    public static bool LoadBool(string key)
    {
        int intValue = PlayerPrefs.GetInt(key, 0);
        return intValue == 1;
    }
}
