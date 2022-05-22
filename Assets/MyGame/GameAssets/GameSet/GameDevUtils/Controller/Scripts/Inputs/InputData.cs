using UnityEngine;


namespace GameAssets.GameSet.GameDevUtils.Controller.Scripts
{


	[CreateAssetMenu(menuName = "GameDevUtils/InputData")]
	public class InputData : ScriptableObject
	{

		[SerializeField] private float   moveSpeed          = 2;
		[SerializeField] private float   maxHorizontalSpeed = 8;
		[SerializeField] private Vector2 clampLimits;
		[SerializeField] private float   cameraOffset = 40f;

		public float MaxHorizontalSpeed => maxHorizontalSpeed;
		public float MoveSpeed          => moveSpeed;
		public float MinXLimit          => clampLimits.x;
		public float MaxXLimit          => clampLimits.y;
		public float CameraOffset       => cameraOffset;


		[HideInInspector] public bool isBlockAllInput;
		[HideInInspector] public bool isBlockVerticalInput;
		[HideInInspector] public bool isBlockHorizontalInput;

	}


}