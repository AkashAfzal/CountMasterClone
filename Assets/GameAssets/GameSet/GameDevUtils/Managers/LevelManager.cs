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
				if (!DataManager.Instance.HasDataAgainstKey(Level_Pref))
				{
					DataManager.Instance.SaveData(Level_Pref, 1);
				}

				return DataManager.Instance.GetIntData(Level_Pref);
			}
			else
				return currentLevelNumber;
		}

		public int CurrentPlayLevelNumber()
		{
			if (!isTesting)
			{
				if (!DataManager.Instance.HasDataAgainstKey(PlayLevel_Pref))
				{
					DataManager.Instance.SaveData(PlayLevel_Pref, 1);
				}

				return DataManager.Instance.GetIntData(PlayLevel_Pref);
			}
			else
				return currentLevelNumber;
		}

		public void NextUnlockLevel()
		{
			int level     = DataManager.Instance.GetIntData(Level_Pref)     + 1;
			int playLevel = DataManager.Instance.GetIntData(PlayLevel_Pref) + 1;
			if (playLevel > allLevels.Length)
			{
				playLevel = 1;
			}

			DataManager.Instance.SaveData(Level_Pref,     level);
			DataManager.Instance.SaveData(PlayLevel_Pref, playLevel);
		}

	}


}