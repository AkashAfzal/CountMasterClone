using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameAssets.GameSet.GameDevUtils.Managers;
using UnityEngine;
using TMPro;

public class PlayerController : Singleton<PlayerController>
{

	//private Inspector Fields
	[SerializeField] int                                  startCharacterSize = 2;
	[SerializeField] float                                distBtwCharacters  = 0.4f;
	[SerializeField] TMP_Text                             characterSizeText;
	[SerializeField] TMP_Text                             addedSizeText;
	[SerializeField] Cinemachine.CinemachineVirtualCamera gamePlayCam;

	//public Hidden Fields
	[HideInInspector] public int             currentCharacterSize = 0;
	[HideInInspector] public List<Character> allCharacters        = new List<Character>();
	//[HideInInspector] 
	public List<Transform> finishObjects = new List<Transform>();


	//Private Fields 
	private bool            IsFighting = false;
	private IEnumerator     FinishCo;
	private PlayerMovement  Movement;
	private EnemyCollection TargetEnemyGroup;


	private void Start()
	{
		Movement = GetComponent<PlayerMovement>();
		AddNewCharacter(startCharacterSize, true);
	}

	public void AddNewCharacter(int size, bool isStart)
	{
		if (!isStart)
			AddedSizeAnimation(size);
		for (int i = 0; i < size; i++)
		{
			Vector3    pos            = new Vector3(Random.Range(transform.position.x - 1, transform.position.x + 1), transform.position.y, Random.Range(transform.position.z - 1, transform.position.z + 1));
			GameObject addedCharacter = CharacterPool.instance.GetCharacter();
			addedCharacter.SetActive(true);
			addedCharacter.transform.position           = pos;
			addedCharacter.transform.parent             = transform;
			addedCharacter.GetComponent<Character>().id = allCharacters.Count;
			currentCharacterSize++;
			allCharacters.Add(addedCharacter.GetComponent<Character>());
			UpdateCharacterSizeText();
		}
	}

	void AddedSizeAnimation(int size)
	{
		Vector3 startPos = addedSizeText.transform.localPosition;
		addedSizeText.text = "+" + size.ToString();
		addedSizeText.gameObject.SetActive(true);
		addedSizeText.transform.DOLocalMove(addedSizeText.transform.localPosition.SetY(1), 0.1f).SetEase(Ease.OutBounce).OnComplete(() =>
		{
			addedSizeText.gameObject.SetActive(false);
			addedSizeText.transform.localPosition = startPos;
		});
	}

	public void CharacterDeath(int id)
	{
		allCharacters.RemoveAt(id);
		for (int i = allCharacters.Count - 1; i > id - 1; i--)
		{
			allCharacters[i].id = i > 0 ? i : 0;
		}

		currentCharacterSize--;
		UpdateCharacterSizeText();
		if (allCharacters.Count == 0)
		{
			Movement.StopFightMove();
			characterSizeText.transform.parent.gameObject.SetActive(false);
			if (IsFighting)
				TargetEnemyGroup.Victory();
			Movement.userCanControl = false;
			GameManager.Instance.ChangeGameState(GameState.Fail);
		}
	}

	public void FightWithEnemyGroup(EnemyCollection enemyGroup)
	{
		TargetEnemyGroup        = enemyGroup;
		IsFighting              = true;
		Movement.userCanControl = false;
		Movement.FightMove(enemyGroup.transform);
	}

	public void FightEnded()
	{
		if (allCharacters.Count > 0)
		{
			IsFighting              = true;
			Movement.userCanControl = true;
			Movement.StopFightMove();
		}
	}

	private void UpdateCharacterSizeText()
	{
		characterSizeText.text = currentCharacterSize.ToString();
	}

	public void Finish()
	{
		if (FinishCo == null)
		{
			FinishCo = FinishShape();
			StartCoroutine(FinishCo);
		}
	}

	private IEnumerator FinishShape()
	{
		characterSizeText.transform.parent.gameObject.SetActive(false);
		yield return StartCoroutine(Movement.MoveCenter());
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
					int chracterIndex = 0;
					allCharacters[chracterIndex].transform.parent                 = finishGroup.transform;
					allCharacters[chracterIndex].isMoveAble                       = false;
					allCharacters[chracterIndex].rigidBody.interpolation          = RigidbodyInterpolation.None;
					allCharacters[chracterIndex].rigidBody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
					allCharacters[chracterIndex].rigidBody.isKinematic            = true;
					allCharacters[chracterIndex].FinishPlacement(new Vector3(-(numberOfCharacter - 1) * distBtwCharacters / 2 + i * distBtwCharacters, 0f, 0f));
					allCharacters.RemoveAt(chracterIndex);
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

		Movement.finishCam.gameObject.SetActive(true);
	}

}