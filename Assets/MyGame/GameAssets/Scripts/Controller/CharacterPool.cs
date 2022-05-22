using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPool : MonoBehaviour
{

	public  GameObject        characterPrefab;
	public  int               size;
	private Queue<GameObject> characterPool = new Queue<GameObject>();


	#region StandAlone

	public static CharacterPool instance;

	private void Awake()
	{
		if (CharacterPool.instance == null)
			instance = this;
		else
			gameObject.SetActive(false);
	}

	#endregion


	void Start()
	{
		for (int i = 0; i < size; i++)
		{
			GameObject addedCharacter = Instantiate(characterPrefab);
			addedCharacter.SetActive(false);
			characterPool.Enqueue(addedCharacter);
		}
	}

	public GameObject GetCharacter()
	{
		if (characterPool.Count > 0)
		{
			return characterPool.Dequeue();
		}
		else
			return Instantiate(characterPrefab);
	}

}