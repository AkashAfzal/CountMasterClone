using UnityEngine;


namespace GameAssets.GameSet.GameDevUtils.Controller.Scripts
{


	public abstract class InputBase : ScriptableObject
	{

		public delegate void InputEvent(float sensitivity, out float horizontal, out float vertical);

		public static event InputEvent inputEvent;

		[SerializeField] protected bool  isAutoRun;
		[SerializeField] protected float moveSpeed            = 2;
		[SerializeField] protected float rotationSpeed        = 0.1f;
		[SerializeField] protected float jumpForce            = 4;
		[SerializeField] protected float sensitivity          = 1;
		[SerializeField] protected float animationMultiplier  = 1;
		[SerializeField] protected float maxHorizontalSpeed   = 8;
		[SerializeField] protected float maxVerticalSpeed     = 8;
		[SerializeField] protected float verticalAcceleration = 25;
		[SerializeField] protected float verticalDeceleration = 25;
		[SerializeField] protected float verticalLerpTime     = 2;

		public float MaxHorizontalSpeed => maxHorizontalSpeed;

		public float MaxVerticalSpeed => maxVerticalSpeed;

		public float VerticalAcceleration => verticalAcceleration;

		public float VerticalDeceleration => verticalDeceleration;

		public float VerticalLerpTime => verticalLerpTime;

		public virtual float MoveSpeed => moveSpeed;

		public virtual float RotationSpeed       => rotationSpeed;
		public virtual float JumpForce           => jumpForce;
		public         float Sensitivity         => sensitivity;
		public virtual float AnimationMultiplier => animationMultiplier;


		public bool  Walk       { get; protected set; }
		public bool  Jump       { get; protected set; }
		public float Vertical   { get; protected set; }
		public float Horizontal { get; protected set; }

		[HideInInspector]
		public bool isBlockAllInput;
		[HideInInspector]
		public bool isBlockVerticalInput;
		[HideInInspector]
		public bool isBlockHorizontalInput;

		public virtual void OnStart()
		{
			isBlockAllInput = false;
		}

		public virtual void OnUpdate()
		{
			if (!isBlockAllInput)
			{
				float horizontal = 0, vertical = 0;
				inputEvent?.Invoke(Sensitivity, out horizontal, out vertical);
				Horizontal = horizontal;
				Vertical   = vertical;
			}
			else
			{
				Horizontal = 0;
				Vertical   = 0;
			}
		}
		
		
		

	}


}