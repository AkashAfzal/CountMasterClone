using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : Singleton<PlayerController>
{

	public  List<Character>                      allCharacters           = new List<Character>();
	[SerializeField]private  int                                  startCharacterSize   = 2;
	public  int                                  currentCharacterSize = 0;
	public  TMP_Text                             characterSizeText;
	public  TMP_Text                             addedSizeText;
	public  List<Transform>                      finishObjects     = new List<Transform>();
	public  float                                distBTWCharacters = 0.4f;
	public  Cinemachine.CinemachineVirtualCamera gamePlayCam;
	private IEnumerator                          finishCo;
	private bool                                 isFighting = false;
	private PlayerMovement                       movement;
	private EnemyCollection                      targetEnemyGroup;
	


	private void Start()
	{
		movement = GetComponent<PlayerMovement>();
		AddCharacter(startCharacterSize, true);
	}

	public void AddCharacter(int size, bool isStart)
	{
		if (!isStart)
			StartCoroutine(AddedSizeAnimation(size));
		for (int i = 0; i < size; i++)
		{
			Vector3    pos            = new Vector3(Random.Range(transform.position.x - 1, transform.position.x + 1), transform.position.y, Random.Range(transform.position.z - 1, transform.position.z + 1));
			GameObject addedCharacter = CharacterPool.instance.GetCharacter(); //Instantiate(characterPrefab, pos, Quaternion.identity);
			addedCharacter.SetActive(true);
			addedCharacter.transform.position              = pos;
			addedCharacter.transform.parent                = transform;
			addedCharacter.GetComponent<Character>().id = allCharacters.Count;
			currentCharacterSize++;
			allCharacters.Add(addedCharacter.GetComponent<Character>());
			UpdateCharacterSizeText();
		}
	}

	IEnumerator AddedSizeAnimation(int size)
	{
		Vector3 startPos  = addedSizeText.transform.localPosition;
		Vector3 targetPos = addedSizeText.transform.localPosition + new Vector3(0f, 1f, 0f);
		addedSizeText.text = "+" + size.ToString();
		addedSizeText.gameObject.SetActive(true);
		while (addedSizeText.transform.localPosition != targetPos)
		{
			addedSizeText.transform.localPosition = Vector3.MoveTowards(addedSizeText.transform.localPosition, targetPos, 2f * Time.deltaTime);
			yield return null;
		}

		addedSizeText.gameObject.SetActive(false);
		addedSizeText.transform.localPosition = startPos;
	}

	public void RemoveCharacter(int size)
	{
		for (int i = 0; i < size; i++)
		{
			if (allCharacters.Count > 0)
			{
				allCharacters[0].gameObject.SetActive(false);
				CharacterDeath(allCharacters[0].id);
			}
		}
	}

	public void CharacterDeath(int index)
	{
		GameObject removedCharacter = allCharacters[index].gameObject;
		allCharacters.RemoveAt(index);
		for (int i = allCharacters.Count - 1; i > index - 1; i--)
		{
			if (i > 0)
			{
				allCharacters[i].id = i;
			}
			else
			{
				allCharacters[i].id = 0;
			}
		}

		currentCharacterSize--;
		UpdateCharacterSizeText();
		if (allCharacters.Count == 0)
		{
			movement.StopFightMove();
			characterSizeText.transform.parent.gameObject.SetActive(false);
			if (isFighting)
				targetEnemyGroup.Victory();
			movement.userCanControl = false;
			LevelController.instance.Fail();
		}
	}

	public void FightWithEnemyGroup(EnemyCollection enemyGroup)
	{
		targetEnemyGroup        = enemyGroup;
		isFighting              = true;
		movement.userCanControl = false;
		movement.FightMove(enemyGroup.transform);
	}

	public void FightEnded()
	{
		if (allCharacters.Count > 0)
		{
			isFighting              = true;
			movement.userCanControl = true;
		}
	}

	public void DecreaseCharacterSize()
	{
		currentCharacterSize--;
		UpdateCharacterSizeText();
	}

	private void UpdateCharacterSizeText()
	{
		characterSizeText.text = currentCharacterSize.ToString();
	}

	public void Finish()
	{
		if (finishCo == null)
		{
			finishCo = FinishShape();
			StartCoroutine(finishCo);
		}
	}

	public void Attack(Transform enemy)
	{
		movement.userCanControl = false;
		for (int i = 0; i < allCharacters.Count; i++)
		{
			allCharacters[i].targetPos = enemy;
		}
	}

	private IEnumerator FinishShape()
	{
		characterSizeText.transform.parent.gameObject.SetActive(false);
		yield return StartCoroutine(movement.MoveCenter());
		int        numberOfCharacter = 1;
		int        count             = 0;
		GameObject finishParent      = new GameObject();
		finishParent.transform.parent        = transform;
		finishParent.transform.localPosition = Vector3.zero;
		finishParent.transform.eulerAngles   = Vector3.zero;
		while (allCharacters.Count > 0)
		{
			GameObject finishGroup = new GameObject();
			finishGroup.transform.parent      = finishParent.transform;
			finishGroup.transform.position    = new Vector3(finishParent.transform.position.x, 0f, finishParent.transform.position.z);
			finishGroup.transform.eulerAngles = Vector3.zero;
			finishObjects.Add(finishGroup.transform);
			gamePlayCam.Follow = finishObjects[0];
			for (int i = 0; i < numberOfCharacter; i++)
			{
				if (allCharacters.Count > 0)
				{
					int middleCharacter = FindMiddleCharacter();
					allCharacters[middleCharacter].transform.parent           = finishGroup.transform;
					allCharacters[middleCharacter].isMoveAble                    = false;
					allCharacters[middleCharacter].rigidBody.interpolation          = RigidbodyInterpolation.None;
					allCharacters[middleCharacter].rigidBody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
					allCharacters[middleCharacter].rigidBody.isKinematic            = true;
					allCharacters[middleCharacter].FinishPlacement(new Vector3(-(numberOfCharacter - 1) * distBTWCharacters / 2 + i * distBTWCharacters, 0f, 0f));
					allCharacters.RemoveAt(middleCharacter);
				}
				else
					break;
			}

			count++;
			if (count == 2)
			{
				numberOfCharacter++;
				if (numberOfCharacter > allCharacters.Count)
					numberOfCharacter = allCharacters.Count;
				count = 0;
			}

			if (allCharacters.Count > 0)
			{
				Vector3 target = finishParent.transform.localPosition + new Vector3(0f, 1f, 0f);
				while (finishParent.transform.localPosition != target)
				{
					finishParent.transform.localPosition = Vector3.MoveTowards(finishParent.transform.localPosition, target, 30f * Time.deltaTime);
					yield return null;
				}
			}

			yield return null;
		}

		movement.finishCam.gameObject.SetActive(true);
	}

	private int FindMiddleCharacter()
	{
		float distance = Vector3.Distance(allCharacters[0].transform.position, transform.position);
		int   index    = 0;
		for (int i = 1; i < allCharacters.Count; i++)
		{
			if (Vector3.Distance(allCharacters[i].transform.position, transform.position) < distance)
			{
				distance = Vector3.Distance(allCharacters[i].transform.position, transform.position);
				index    = i;
				if (distance < 0.5)
					return index;
			}
		}

		return index;
	}

}