using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

	private void Awake()
	{
		Application.targetFrameRate = 60;
		#if UNITY_EDITOR
		Debug.unityLogger.logEnabled = true;
		#else
    Debug.unityLogger.logEnabled = false;
		#endif
		if (!PlayerPrefs.HasKey("level"))
		{
			PlayerPrefs.SetInt("level", 0);
		}

		if (PlayerPrefs.GetInt("level") + 1 >= SceneManager.sceneCountInBuildSettings - 1)
			PlayerPrefs.SetInt("level", 0);
		SceneManager.LoadScene(PlayerPrefs.GetInt("level") + 1);
	}

}