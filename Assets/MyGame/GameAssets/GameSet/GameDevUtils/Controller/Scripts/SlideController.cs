using System;
using DG.Tweening;
using GameAssets.GameSet.GameDevUtils.Managers;
using UnityEngine;


namespace GameAssets.GameSet.GameDevUtils.Controller.Scripts
{


	public class SlideController : BaseController
	{

		protected float rotated;

		// public           Transform rotationObject;
		public           float      turnTime;
		[SerializeField] GameObject topPlayerShadow;
		[SerializeField] Transform  camTargetObject;
		[SerializeField] GameObject finalMomentumCamTarget;
		[SerializeField] Vector2    defaultHorizontalLimit;

		[HideInInspector] public Vector3 initPos;
		[HideInInspector] public Vector2 horizontalLimit;

		Vector3    PrevPosition;
		float      XOffset;
		float      ZOffset;
		bool       BlockHorizontalInput = false;
		bool       IsTopPlayerLanded    = false;
		GameObject TopPlayer;
		GameObject BottomPlayer;


		void OnEnable()
		{
			GameManager.onFinalMomentumEvent += TopPlayerJumpDown;
		}

		void OnDisable()
		{
			GameManager.onFinalMomentumEvent -= TopPlayerJumpDown;
		}


		void Start()
		{
			horizontalLimit = defaultHorizontalLimit;
		}

		protected override void Movement()
		{
			Rotation();
			ApplyVelocity();
			JumpingAndLanding();
		}

		protected virtual void ApplyVelocity()
		{
			ZOffset += (inputs.Vertical * (m_interpolation * inputs.MoveSpeed * Time.deltaTime));
			XOffset =  Mathf.Clamp(XOffset + inputs.Horizontal, horizontalLimit.x, horizontalLimit.y);
			var actualForwardPosition = ZOffset * transform.forward;
			var desiredCamPosition    = (actualForwardPosition - new Vector3(0, 0, 3)) + initPos;
			desiredCamPosition.y     = transform.position.y - (yOffset.y               - 1.3f);
			camTargetObject.position = desiredCamPosition;
			camTargetObject.rotation = transform.rotation;
			var     deltaRightPosition = transform.right * (XOffset);
			Vector3 desirePosition     = initPos + deltaRightPosition;
			desirePosition   = actualForwardPosition + desirePosition;
			desirePosition.y = transform.position.y  - (yOffset.y);
			rigidBody.MovePosition(desirePosition);
		}

		protected override void UpdateAnimator()
		{
			m_Animator.SetFloat("Value", inputs.Vertical);
		}

		public void Turn(Vector3 position)
		{
			ZOffset = 0;
			initPos = position;
		}

		protected virtual void Rotation()
		{
			// if (Mathf.Abs(inputs.Vertical) > 0.1f || Mathf.Abs(inputs.Horizontal) > 0.1f)
			// {
			// 	rotated = Mathf.Atan2(inputs.Horizontal, inputs.Vertical);
			// 	rotated = Mathf.Rad2Deg * rotated;
			// 	// rotated = xRange.Clamp(rotated);
			// 	var targetRotation = Quaternion.Euler(Vector3.up * (rotated));
			// 	rotationObject.localRotation = Quaternion.Slerp(rotationObject.localRotation, targetRotation, inputs.RotationSpeed * Time.deltaTime);
			// }
		}


		public void SetClampXLimit(bool overrideDefault, float minXLimit, float maxXLimit, float timeToLerp)
		{
			if (overrideDefault)
			{
				DOTween.To(() => horizontalLimit.x, x => horizontalLimit.x = x, minXLimit, timeToLerp);
				DOTween.To(() => horizontalLimit.y, y => horizontalLimit.y = y, maxXLimit, timeToLerp);
			}
			else
			{
				DOTween.To(() => horizontalLimit.x, x => horizontalLimit.x = x, defaultHorizontalLimit.x, timeToLerp);
				DOTween.To(() => horizontalLimit.y, y => horizontalLimit.y = y, defaultHorizontalLimit.y, timeToLerp);
			}
		}


		private void TopPlayerJumpDown()
		{
			IsTopPlayerLanded     = true;
			rigidBody.isKinematic = true;
			topPlayerShadow.SetActive(false);
			var desiredPos = BottomPlayer.transform.position;
			BottomPlayer.transform.DOLocalMove(new Vector3(-1f, 0, 0), 0.1f);
			TopPlayer.transform.DOMove(desiredPos.Plus(0, 7.2f, 0), 1f).SetDelay(0.7f).SetEase(Ease.InQuad).OnComplete(() => { Invoke(nameof(DisablePlayers), 0.4f); });
			m_Animator.Play("DownLanding", 0);
		}

		private void DisablePlayers()
		{
			TopPlayer.SetActive(false);
			BottomPlayer.SetActive(false);
		}

	}


}