using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

public class DataStorage
{
	public static void SaveToFile<T>(string fileName, T dataToStore)
	{
		fileName = Application.persistentDataPath + "/" + fileName + ".json";

		try
		{
			string serializedData = JsonUtility.ToJson(dataToStore, true);
			File.WriteAllText(fileName, serializedData);
		}
		catch(System.Exception ex)
		{
			Debug.LogWarning("File writing error: " + ex.Message);
		}
	}

	public static T LoadFromFile<T>(string fileName)
	{
		T storedData = default(T);
		fileName = Application.persistentDataPath + "/" + fileName + ".json";
		try
		{
			string serializedData = File.ReadAllText(fileName);
			storedData = JsonUtility.FromJson<T>(serializedData);
		}
		catch (System.Exception ex)
		{
			Debug.LogWarning("File reading error: " + ex.Message);
		}

		return storedData;
	}

	public static void RemoveData<T>(string fileName)
	{
		fileName = Application.persistentDataPath + "/" + fileName + ".json";

		try
		{
			File.Delete(fileName);
		}
		catch (System.Exception ex)
		{
			Debug.LogWarning(fileName + " deleting error: " + ex.Message);
		}
	}
}
