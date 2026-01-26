using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public struct GameData
{
    public KeyCode left;
    public KeyCode right;
    public KeyCode jump;
    public KeyCode quit;
    public KeyCode restart;
    public int level;

    public GameData(KeyCode leftValue, KeyCode rightValue, KeyCode jumpValue, KeyCode quitValue, KeyCode restartValue, int levelValue)
    {
        left = leftValue;
        right = rightValue;
        jump = jumpValue;
        quit = quitValue;
        restart = restartValue;
        level = levelValue;
    }
}

public class SaveAndLoad
{
    public static GameData gameData = new GameData(KeyCode.A, KeyCode.D, KeyCode.W, KeyCode.Escape, KeyCode.R, 0);
    public static void Save(int location)
    {
        GameData savedGameData = gameData;
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        string filePath = Application.persistentDataPath + "/mygamedata" + location + ".db";
        FileStream fileStream = new FileStream(filePath, FileMode.Create);
        binaryFormatter.Serialize(fileStream, savedGameData);
        fileStream.Close();
    }

    public static void Load(int location)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        string filePath = Application.persistentDataPath + "/mygamedata" + location + ".db";
        if (File.Exists(filePath))
        {
            FileStream fileStream = new FileStream(filePath, FileMode.Open);
            gameData = (GameData)binaryFormatter.Deserialize(fileStream);
            fileStream.Close();
        }
        else
        {
            gameData = new GameData(KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.Escape, KeyCode.R, 0);
            Save(location);
        }
    }

    public static bool HaveGameData(int location)
    {
        string filePath = Application.persistentDataPath + "/mygamedata" + location + ".db";
        return File.Exists(filePath);
    }

    public static void DeleteGameData(int location)
    {
        string filePath = Application.persistentDataPath + "/mygamedata" + location + ".db";
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}