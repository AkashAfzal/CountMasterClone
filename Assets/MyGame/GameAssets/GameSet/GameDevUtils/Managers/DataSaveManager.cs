using UnityEngine;


namespace GameAssets.GameSet.GameDevUtils.Managers
{


	public class DataSaveManager : Singleton<DataSaveManager>
	{

		public bool HasDataAgainstKey(string key) => PlayerPrefs.HasKey(key);

		public void SaveData(string key, string value)
		{
			PlayerPrefs.SetString(key, value);
			PlayerPrefs.Save();
		}

		public void SaveData(string key, float value)
		{
			PlayerPrefs.SetFloat(key, value);
			PlayerPrefs.Save();
		}

		public void SaveData(string key, int value)
		{
			PlayerPrefs.SetInt(key, value);
			PlayerPrefs.Save();
		}

		public int GetIntData(string key)
		{
			if (PlayerPrefs.HasKey(key))
			{
				return PlayerPrefs.GetInt(key);
			}
			else
			{
				print($"No saved value available against {key}");
			}

			return -1;
		}

		public float GetFloatData(string key)
		{
			if (PlayerPrefs.HasKey(key))
			{
				return PlayerPrefs.GetFloat(key);
			}
			else
			{
				print($"No saved value available against {key}");
			}

			return -1;
		}

		public string GetStringData(string key)
		{
			if (PlayerPrefs.HasKey(key))
			{
				return PlayerPrefs.GetString(key);
			}
			else
			{
				print($"No saved value available against {key}");
			}

			return null;
		}

	}


}