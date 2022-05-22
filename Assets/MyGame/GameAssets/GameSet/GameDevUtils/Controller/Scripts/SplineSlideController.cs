using System;
using System.Threading.Tasks;
using DG.Tweening;
using GameAssets.GameSet.GameDevUtils.Managers;
using PathCreation;
using UnityEngine;


namespace GameAssets.GameSet.GameDevUtils.Controller.Scripts
{


	public class SplineSlideController : BaseController
	{

		float                                         rotated;
		public                   Transform            rotationObject;
		public                   PathCreator          pathCreator;
		public                   EndOfPathInstruction endOfPathInstruction;
		[SerializeField]         Vector2              defaultHorizontalLimit;
		[HideInInspector] public Vector2              horizontalLimit;

		float        XOffset;
		public float ForwardDistanceTravelled;
		bool         BlockHorizontalInput = false;
		public bool  IsReachedAtFinalMomentum;


		void Start()
		{
			horizontalLimit = defaultHorizontalLimit;
			if (pathCreator != null)
			{
				// Subscribed to the pathUpdated event so that we're notified if the path changes during the game
				pathCreator.pathUpdated += OnPathChanged;
			}
		}


		protected override void Movement()
		{
			Rotation();
			ApplyVelocity();
			JumpingAndLanding();
		}

		protected virtual void ApplyVelocity()
		{
			ForwardDistanceTravelled += (IsReachedAtFinalMomentum ? 0.25f : inputs.Vertical * (m_interpolation * inputs.MoveSpeed * Time.deltaTime));
			XOffset                  =  Mathf.Clamp(XOffset + (inputs.Horizontal * inputs.MaxHorizontalSpeed * Time.deltaTime), horizontalLimit.x, horizontalLimit.y);
			var     actualForwardMovement = pathCreator.path.GetPointAtDistance(ForwardDistanceTravelled, endOfPathInstruction);
			var     deltaRightPosition    = transform.right * (XOffset);
			Vector3 desirePosition        = actualForwardMovement + deltaRightPosition;
			desirePosition.y = transform.position.y /* - (yOffset.y - 2f)*/;
			rigidBody.MovePosition(desirePosition);
		}

		protected override void UpdateAnimator()
		{
			m_Animator.SetFloat("Value", inputs.Vertical);
		}

		protected virtual void Rotation()
		{
			transform.rotation = pathCreator.path.GetRotationAtDistance(ForwardDistanceTravelled, endOfPathInstruction);
			// if (Mathf.Abs(inputs.Vertical) > 0.1f || Mathf.Abs(inputs.Horizontal) > 0.1f)
			// {
			// 	rotated = Mathf.Atan2(inputs.Horizontal, inputs.Horizontal);
			// 	rotated = Mathf.Rad2Deg * rotated;
			// 	// rotated = xRange.Clamp(rotated);
			// 	var targetRotation = Quaternion.Euler(Vector3.up * (rotated));
			// 	rotationObject.localRotation = Quaternion.Slerp(rotationObject.localRotation, targetRotation, inputs.RotationSpeed * Time.deltaTime);
			// }
		}


		// If the path changes during the game, update the distance travelled so that the follower's position on the new path
		// is as close as possible to its position on the old path
		void OnPathChanged()
		{
			ForwardDistanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
		}


		// public void SetClampXLimit(bool overrideDefault, float minXLimit, float maxXLimit, float timeToLerp)
		// {
		// 	if (overrideDefault)
		// 	{
		// 		DOTween.To(() => horizontalLimit.x, x => horizontalLimit.x = x, minXLimit, timeToLerp);
		// 		DOTween.To(() => horizontalLimit.y, y => horizontalLimit.y = y, maxXLimit, timeToLerp);
		// 	}
		// 	else
		// 	{
		// 		DOTween.To(() => horizontalLimit.x, x => horizontalLimit.x = x, defaultHorizontalLimit.x, timeToLerp);
		// 		DOTween.To(() => horizontalLimit.y, y => horizontalLimit.y = y, defaultHorizontalLimit.y, timeToLerp);
		// 	}
		// }


		public async void PauseOrUnpausePlayer()
		{
			inputs.isBlockAllInput = true;
			await Task.Delay(TimeSpan.FromSeconds(3f));
			inputs.isBlockAllInput = false;
			//MakeUpStack.Instance.RearrangeStack();
		}


		public void MoveToFinalPosition()
		{
			GameManager.Instance.ChangeGameState(GameState.FinalMomentum);
			IsReachedAtFinalMomentum = true;
			inputs.isBlockAllInput   = true;
			//GameManager.Instance.finalCameraAsset.posOffset = GameManager.Instance.followCameraAsset.posOffset.Plus(1.5f, 3, 0);
			DOTween.To(() => XOffset, x => XOffset = x, 0f, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
			{
				//finalMomentumCamera.SetActive(true);
			});
		}

	}


}