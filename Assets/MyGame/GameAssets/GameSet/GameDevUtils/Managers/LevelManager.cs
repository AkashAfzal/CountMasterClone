using UnityEngine;


namespace GameAssets.GameSet.GameDevUtils.Managers
{


	public class LevelManager : MonoBehaviour
	{

		[SerializeField] GameObject[] allLevels;
		[SerializeField] int          currentLevelNumber;
		[SerializeField] bool         isTesting;
		readonly         string       Level_Pref     = "LevelNumber";
		readonly         string       PlayLevel_Pref = "PlayLevelNumber";

		public void LoadLevelAtStart()
		{
			if (allLevels.Length > 0)
			{
				Instantiate(allLevels[CurrentPlayLevelNumber() - 1], Vector3.zero, Quaternion.identity);
			}
		}

		public int InfinityCurrentLevelNumber()
		{
			if (!isTesting)
			{
				if (!DataSaveManager.Instance.HasDataAgainstKey(Level_Pref))
				{
					DataSaveManager.Instance.SaveData(Level_Pref, 1);
				}

				return DataSaveManager.Instance.GetIntData(Level_Pref);
			}
			else
				return currentLevelNumber;
		}

		public int CurrentPlayLevelNumber()
		{
			if (!isTesting)
			{
				if (!DataSaveManager.Instance.HasDataAgainstKey(PlayLevel_Pref))
				{
					DataSaveManager.Instance.SaveData(PlayLevel_Pref, 1);
				}

				return DataSaveManager.Instance.GetIntData(PlayLevel_Pref);
			}
			else
				return currentLevelNumber;
		}

		public void NextUnlockLevel()
		{
			int level     = DataSaveManager.Instance.GetIntData(Level_Pref)     + 1;
			int playLevel = DataSaveManager.Instance.GetIntData(PlayLevel_Pref) + 1;
			if (playLevel > allLevels.Length)
			{
				playLevel = 1;
			}

			DataSaveManager.Instance.SaveData(Level_Pref,     level);
			DataSaveManager.Instance.SaveData(PlayLevel_Pref, playLevel);
		}

	}


}