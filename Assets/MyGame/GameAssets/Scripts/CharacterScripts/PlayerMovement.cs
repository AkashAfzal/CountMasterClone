using System.Collections;
using GameAssets.GameSet.GameDevUtils.Controller.Scripts;
using GameAssets.GameSet.GameDevUtils.Managers;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

	public InputData                            inputs;
	public Finish                               finish;
	public Cinemachine.CinemachineVirtualCamera finishCam;

	Vector3          CharStartPos = Vector3.zero;
	Vector3          StartPoint   = Vector3.zero;
	Vector3          EndPoint     = Vector3.zero;
	PlayerController playerController;
	IEnumerator      fightMoveCo;
	Camera           MainCamera;
	Vector3          InputPoint   = Vector3.zero;
	float            MovableLeft  = 0f;
	float            MovableRight = 0f;

	bool    FightStart;
	Vector3 TargetPos;

	public bool userCanControl;
	public bool moveForward;
	public bool moveHorizontal;

	void OnEnable() => GameManager.onGamePlayEvent += StartGame;

	void OnDisable() => GameManager.onGamePlayEvent -= StartGame;

	private void StartGame()
	{
		playerController = GetComponent<PlayerController>();
		MainCamera       = Camera.main;
		StartCoroutine(UpdateLeftRightBoundary());
		SoundManager.Instance.PlayRunSounds(true);
	}

	private void Update()
	{
		if (GameManager.Instance.GameCurrentState == GameState.Gameplay || GameManager.Instance.GameCurrentState == GameState.FinalMomentum)
		{
			if (userCanControl)
			{
				ForwardMovement();
				HorizontalMovement();
			}

			if (FightStart && !userCanControl)
			{
				var position = transform.position;
				position           = Vector3.MoveTowards(position, position + TargetPos, 1.5f * Time.deltaTime);
				transform.position = position;
			}
		}
	}

	private void ForwardMovement()
	{
		if (!moveForward) return;
		var position = transform.position;
		position           = Vector3.MoveTowards(position, position + Vector3.forward, inputs.MoveSpeed * Time.deltaTime);
		transform.position = position;
	}

	void HorizontalMovement()
	{
		if (moveHorizontal)
		{
			InputPoint   = Input.mousePosition;
			InputPoint.z = inputs.CameraOffset;
			if (Input.GetMouseButtonDown(0))
			{
				StartPoint   =  MainCamera.ScreenToWorldPoint(InputPoint);
				StartPoint.x -= MainCamera.transform.position.x;
				CharStartPos =  transform.localPosition;
			}

			if (Input.GetMouseButton(0))
			{
				EndPoint   =  MainCamera.ScreenToWorldPoint(InputPoint);
				EndPoint.x -= MainCamera.transform.position.x;
				float   distance  = EndPoint.x - StartPoint.x;
				Vector3 targetPos = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x + distance, transform.localPosition.y, transform.localPosition.z), inputs.MaxHorizontalSpeed * Time.deltaTime);
				if ((targetPos.x > MovableLeft || transform.position.x < targetPos.x) && (targetPos.x < MovableRight || transform.position.x > targetPos.x))
				{
					transform.localPosition =  targetPos;
					StartPoint.x            += transform.localPosition.x - CharStartPos.x;
					CharStartPos            =  transform.localPosition;
				}
				else
				{
					StartPoint   =  MainCamera.ScreenToWorldPoint(InputPoint);
					StartPoint.x -= MainCamera.transform.position.x;
					CharStartPos =  transform.localPosition;
				}
			}
		}
	}

	private IEnumerator UpdateLeftRightBoundary()
	{
		while (GameManager.Instance.GameCurrentState == GameState.Gameplay)
		{
			float leftBorder  = 0f;
			float rightBorder = 0f;
			foreach (Character character in playerController.allCharacters)
			{
				if (character.transform.localPosition.x < leftBorder)
				{
					leftBorder = character.transform.localPosition.x;
				}

				if (character.transform.localPosition.x > rightBorder)
				{
					rightBorder = character.transform.localPosition.x;
				}
			}

			MovableLeft  = inputs.MinXLimit - leftBorder;
			MovableRight = inputs.MaxXLimit - rightBorder;
			yield return new WaitForSeconds(1f);
		}
	}

	public void FightMove(Transform target)
	{
		SoundManager.Instance.PlayRunSounds(false);
		TargetPos  = (target.position - transform.position).normalized;
		FightStart = true;
	}

	public void StopFightMove()
	{
		FightStart = false;
		SoundManager.Instance.PlayRunSounds(true);
		
		//Reset Input;
		InputPoint   =  Input.mousePosition;
		StartPoint   =  MainCamera.ScreenToWorldPoint(InputPoint);
		StartPoint.x -= MainCamera.transform.position.x;
		CharStartPos =  transform.localPosition;
	}

	public IEnumerator MoveCenter()
	{
		moveHorizontal = false;
		while (transform.position != new Vector3(0f, transform.position.y, transform.position.z))
		{
			transform.position = Vector3.MoveTowards(transform.position, new Vector3(0f, transform.position.y, transform.position.z), inputs.MaxHorizontalSpeed * Time.deltaTime);
			yield return null;
		}

		SoundManager.Instance.PlayRunSounds(false);
		StartCoroutine(FinishMove());
	}

	private IEnumerator FinishMove()
	{
		userCanControl = false;
		int     count = 0;
		Vector3 targetPos;
		while (transform.position != finish.transform.position)
		{
			transform.position = Vector3.MoveTowards(transform.position, finish.transform.position, inputs.MoveSpeed * Time.deltaTime);
			yield return null;
		}

		while (playerController.finishObjects.Count > 0)
		{
			targetPos = finish.transform.position + new Vector3(0f, 0f, count);
			while (transform.position != targetPos)
			{
				transform.position = Vector3.MoveTowards(transform.position, targetPos, inputs.MoveSpeed * Time.deltaTime);
				yield return null;
			}

			count++;
			finish.finishSteps[count].stepCamera.SetActive(true);
			playerController.finishObjects[playerController.finishObjects.Count - 1].parent = null;
			for (int i = 0; i < playerController.finishObjects[playerController.finishObjects.Count - 1].childCount; i++)
			{
				playerController.finishObjects[playerController.finishObjects.Count - 1].GetChild(i).GetComponent<Animator>().SetBool("Run", false);
			}

			SoundManager.Instance.PlayStairsUpSound();
			playerController.finishObjects.RemoveAt(playerController.finishObjects.Count - 1);
		}

		if (count - 2 > 0)
		{
			yield return new WaitForSeconds(0.5f);
			SoundManager.Instance.PlayOneShot(SoundManager.Instance.confettiClip, 0.7f);
			finish.finishSteps[count - 2].particles.SetActive(true);
			yield return new WaitForSeconds(1.5f);
			GameManager.Instance.ChangeGameState(GameState.Win);
		}
		else
		{
			yield return new WaitForSeconds(1f);
			GameManager.Instance.ChangeGameState(GameState.Win);
		}
	}

}