using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{

	public                     bool       isLevelFinished = false;
	[Header("Prefabs")] public GameObject winPanel;
	public                     GameObject failPanel;


	#region StandAlone

	public static LevelController instance;

	private void Awake()
	{
		instance = this;
	}

	#endregion


	public void Win()
	{
		winPanel.SetActive(true);
	}

	public void Fail()
	{
		failPanel.SetActive(true);
	}

}