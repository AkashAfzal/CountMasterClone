using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameAssets.GameSet.GameDevUtils.Controller.Scripts;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

	public             InputData                            inputs;
	[Space(10)] public Finish                               finish;
	public             Cinemachine.CinemachineVirtualCamera finishCam;

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

	private void Start()
	{
		playerController = GetComponent<PlayerController>();
		MainCamera       = Camera.main;
		StartCoroutine(UpdateBorders());
		inputs.isBlockAllInput        = false;
		inputs.isBlockHorizontalInput = false;
	}

	private void Update()
	{
		if (!inputs.isBlockAllInput)
		{
			Move();
		}

		if (FightStart && inputs.isBlockAllInput)
		{
			var position = transform.position;
			position           = Vector3.MoveTowards(position, position + TargetPos, 1.5f * Time.deltaTime);
			transform.position = position;
		}
	}

	protected virtual void Move()
	{
		if (!inputs.isBlockVerticalInput)
		{
			var position = transform.position;
			position           = Vector3.MoveTowards(position, position + Vector3.forward, inputs.MoveSpeed * Time.deltaTime);
			transform.position = position;
		}

		if (!inputs.isBlockHorizontalInput)
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
				EndPoint   =  Camera.main.ScreenToWorldPoint(InputPoint);
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

	private IEnumerator UpdateBorders()
	{
		while (!LevelController.instance.isLevelFinished)
		{
			float leftBorder  = 0f;
			float rightBorder = 0f;
			foreach (Character _character in playerController.allCharacters)
			{
				if (_character.transform.localPosition.x < leftBorder)
				{
					leftBorder = _character.transform.localPosition.x;
				}

				if (_character.transform.localPosition.x > rightBorder)
				{
					rightBorder = _character.transform.localPosition.x;
				}
			}

			MovableLeft  = inputs.MinXLimit - leftBorder;
			MovableRight = inputs.MaxXLimit - rightBorder;
			yield return new WaitForSeconds(1f);
		}
	}

	public void FightMove(Transform target)
	{
		TargetPos  = (target.position - transform.position).normalized;
		FightStart = true;
	}

	public void StopFightMove()
	{
		FightStart = false;
	}


	public IEnumerator MoveCenter()
	{
		inputs.isBlockHorizontalInput = true;
		while (transform.position != transform.position.SetX(0))
		{
			transform.position = Vector3.MoveTowards(transform.position, transform.position.SetX(0), inputs.MaxHorizontalSpeed * Time.deltaTime);
			yield return null;
		}

		StartCoroutine(FinishMove());
	}

	private IEnumerator FinishMove()
	{
		inputs.isBlockAllInput = true;
		int     count = 0;
		Vector3 targetPos;
		while (transform.position != finish.transform.position)
		{
			Debug.Log(transform.position +" "+ finish.transform.position);
			transform.position = Vector3.MoveTowards(transform.position, finish.transform.position, inputs.MoveSpeed * Time.deltaTime);
			yield return null;
		}

		Debug.Log("ajkshjkddk");
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

			playerController.finishObjects.RemoveAt(playerController.finishObjects.Count - 1);
		}

		if (count - 2 > 0)
		{
			yield return new WaitForSeconds(0.5f);
			finish.finishSteps[count - 2].particles.SetActive(true);
			yield return new WaitForSeconds(1.5f);
			LevelController.instance.Win();
		}
		else
		{
			yield return new WaitForSeconds(1f);
			LevelController.instance.Win();
		}
	}

}