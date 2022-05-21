using System;
using System.Threading.Tasks;
using DG.Tweening;
using GameAssets.GameSet.GameDevUtils.Controller.Scripts;
using GameAssets.GameSet.GameDevUtils.Managers;
using PathCreation;
using UnityEngine;


namespace GameDevUtils.CharacterController
{


	public class SplineMovementController : BaseController
	{

		//Inspector Fields
		[SerializeField]         Transform            rotationObject;
		[SerializeField]         PathCreator          pathCreator;
		[SerializeField]         EndOfPathInstruction endOfPathInstruction;
		[SerializeField]         Vector2              defaultHorizontalLimit;
		[HideInInspector] public Vector2              horizontalLimit;
		[SerializeField]         RangeBy              xRange;
		[SerializeField]         Transform            camTarget;
		[SerializeField]         GameObject           finalMomentumCamera;
		public                   GameObject           topCamera;

		bool IsReachedAtFinalMomentum;


		//Private Fields
		public  float rotated;
		private float _verticalSpeed;       // In meters/second
		private float _targetVerticalSpeed; // In meters/secon
		private float XOffset;
		private float ForwardDistanceTravelled;


		public float ForwardSpeed => _verticalSpeed;


		//Properties
		Vector3 CurrentVerticalVelocity => rigidBody.velocity.Multiply(0.0f, 0f, 1f);


		protected virtual void Start()
		{
			xRange.@from    = transform.position.x;
			horizontalLimit = defaultHorizontalLimit;
			if (pathCreator != null)
			{
				// Subscribed to the pathUpdated event so that we're notified if the path changes during the game
				pathCreator.pathUpdated += OnPathChanged;
			}
		}

		protected override void Movement()
		{
			AccelerateVerticalSpeed();
			ApplyVelocity();
			Rotation();
			JumpingAndLanding();
		}

		private void AccelerateVerticalSpeed()
		{
			// inputs.isBlockInput = !(fuelSystem.startFuel > 0);
			// if (inputs.Vertical > 0 && fuelSystem.startFuel > 0)
			// {
			// 	fuelSystem.ReduceFuel();
			// }
			_targetVerticalSpeed = (IsReachedAtFinalMomentum ? 1.5f : inputs.Vertical) * inputs.MaxVerticalSpeed;
			float verticalAcceleration = inputs.Vertical > 0.0f ? inputs.VerticalAcceleration : inputs.VerticalDeceleration;
			_verticalSpeed                                = Mathf.MoveTowards(_verticalSpeed, _targetVerticalSpeed, verticalAcceleration * Time.deltaTime);
			//SoundManager.Instance.engineSoundSource.pitch = _verticalSpeed.Remap(0, inputs.MaxHorizontalSpeed, 0.7f, 1.2f);
		}


		protected virtual void ApplyVelocity()
		{
			// camTarget.transform.position =  camTarget.transform.position.SetY(boingBones.FirstBonePosition.y > boingBones.LastBonePosition.y ? 
				// boingBones.FirstBonePosition.y - 2.08174f : boingBones.LastBonePosition.y - 2.08174f);
			ForwardDistanceTravelled     += (m_interpolation * _verticalSpeed);
			XOffset                      =  Mathf.Clamp(XOffset + (inputs.Horizontal * inputs.MaxHorizontalSpeed * Time.deltaTime), horizontalLimit.x, horizontalLimit.y);
			var     actualForwardMovement = pathCreator.path.GetPointAtDistance(ForwardDistanceTravelled, endOfPathInstruction);
			var     deltaRightPosition    = transform.right * (XOffset);
			Vector3 desirePosition        = actualForwardMovement + deltaRightPosition;
			desirePosition.y = transform.position.y - yOffset.y;
			rigidBody.MovePosition(desirePosition);
			if (m_Animator)
				UpdateAnimator();
		}

		protected override void UpdateAnimator()
		{
			float normVerticalSpeed = CurrentVerticalVelocity.magnitude / inputs.MaxVerticalSpeed;
			m_Animator.SetFloat("Value", normVerticalSpeed);
		}

		protected virtual void Rotation()
		{
			transform.rotation = pathCreator.path.GetRotationAtDistance(ForwardDistanceTravelled, endOfPathInstruction);
			// if (Mathf.Abs(inputs.Horizontal) != 0f)
			// {
			// 	rotated = inputs.Horizontal >= 0 ? Mathf.Atan2(inputs.Vertical / 4, 1f - inputs.Horizontal) : Mathf.Atan2(-inputs.Vertical / 4, 1f - (-inputs.Horizontal));
			// }
			// else
			// {
			// 	rotated = 0;
			// }
			//
			// rotated = Mathf.Rad2Deg * rotated;
			// // rotated = inputs.Horizontal >= 0 ? Mathf.Abs(rotated) : -Mathf.Abs(rotated);
			// rotated = xRange.Clamp(rotated);
			// var targetRotation = Quaternion.Euler(Vector3.up * (rotated));
			// rotationObject.localRotation = Quaternion.Slerp(rotationObject.localRotation, targetRotation, inputs.RotationSpeed* _verticalSpeed/2 * Time.deltaTime);
		}

		// If the path changes during the game, update the distance travelled so that the follower's position on the new path
		// is as close as possible to its position on the old path
		void OnPathChanged()
		{
			ForwardDistanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
		}


	}


}