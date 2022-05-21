using GameDevUtils;
using UnityEngine;


namespace GameAssets.GameSet.GameDevUtils.Controller.Scripts
{


	public class MovementController : BaseController
	{

		protected float          rotated;
		[SerializeField] RangeBy xRange;
		private          Vector3 prevPosition;

		protected virtual void Start()
		{
			xRange.@from = transform.position.x;
		}

		protected override void Movement()
		{
			ApplyVelocity();
			Rotation();
			JumpingAndLanding();
		}

		protected virtual void ApplyVelocity()
		{
			Vector3 desiredVelocity = transform.forward * (Mathf.Abs(inputs.Vertical) > 0.1f || Mathf.Abs(inputs.Horizontal) > 0.1f ? inputs.MoveSpeed : 0);
			desiredVelocity.y  = rigidBody.velocity.y;
			rigidBody.velocity = desiredVelocity;
			UpdateAnimator(desiredVelocity);
		}

		protected virtual void UpdateAnimator(Vector3 desiredVelocity)
		{
			// animator.SetFloat("Value", Mathf.Clamp01(desiredVelocity.magnitude));
		}
		
		protected override void UpdateAnimator()
		{
			// animator.SetFloat("Value", Mathf.Clamp01(desiredVelocity.magnitude));
		}
		
		

		protected virtual void Rotation()
		{
			if (Mathf.Abs(inputs.Vertical) > 0.1f || Mathf.Abs(inputs.Horizontal) > 0.1f)
			{
				rotated = Mathf.Atan2(inputs.Horizontal, inputs.Vertical);
				rotated = Mathf.Rad2Deg * rotated;
				rotated = xRange.Clamp(rotated);
				var targetRotation = Quaternion.Euler(Vector3.up * (rotated));
				transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, inputs.RotationSpeed * Time.deltaTime);
			}
		}

	}


}