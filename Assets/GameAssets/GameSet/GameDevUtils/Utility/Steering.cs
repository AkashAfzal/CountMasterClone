using GameAssets.GameSet.GameDevUtils.Controller.Scripts;
using GameDevUtils.CharacterController;
using UnityEngine;


namespace GameAssets.GameSet.GameDevUtils.Utility
{


	public class Steering : MonoBehaviour
	{

		SplineMovementController Controller;
		public Transform[]       tyres;


		public float leftRotated;
		float        rightRotated;

		public float horizontalInput;
		float        verticalInput;


		void Start()
		{
			Controller = transform.root.GetComponent<SplineMovementController>();
		}


		void Update()
		{
			horizontalInput = Mathf.MoveTowards(horizontalInput, Controller.inputs.Horizontal, 10 * Time.deltaTime);
			UpdateSteering();
		}

		void UpdateSteering()
		{
			if (Mathf.Abs(Controller.inputs.Horizontal) != 0f)
			{
				leftRotated = Controller.inputs.Horizontal >= 0 ? Mathf.Atan2(Controller.inputs.Vertical / 4, 1f - Controller.inputs.Horizontal) : Mathf.Atan2(-Controller.inputs.Vertical / 4, 1f - (-Controller.inputs.Horizontal));
				// leftRotated = Controller.inputs.Horizontal >= 0 ? Mathf.Atan2(Controller.inputs.Vertical, Controller.inputs.Horizontal) : Mathf.Atan2(-Controller.inputs.Vertical, -Controller.inputs.Horizontal);
			}
			else
			{
				leftRotated = 0;
			}

			leftRotated = Mathf.Rad2Deg * leftRotated;
			leftRotated = Mathf.Clamp(leftRotated, -30, 30);
			var targetRotation = Quaternion.Euler(Vector3.up                                                                   * (leftRotated));
			tyres[0].localRotation = Quaternion.Slerp(tyres[0].localRotation, targetRotation, (Controller.inputs.RotationSpeed * 2) * Time.deltaTime);
			tyres[1].localRotation = Quaternion.Slerp(tyres[1].localRotation, targetRotation, (Controller.inputs.RotationSpeed * 2) * Time.deltaTime);
		}

	}


}