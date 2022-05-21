using System;
using GameAssets.GameSet.GameDevUtils.Managers;
using UnityEngine;


namespace GameAssets.GameSet.GameDevUtils.Controller.Scripts
{


	public abstract class BaseController : MonoBehaviour
	{

		[SerializeField] public  InputBase       inputs;
		public                   Animator        m_Animator = null;
		[HideInInspector] public Rigidbody       rigidBody  = null;
		[HideInInspector] public CapsuleCollider collider;
		[SerializeField]  public LayerMask       groundLayerMask;

		public    bool  m_isGrounded;
		protected float m_jumpTimeStamp   = 0;
		protected float m_minJumpInterval = 0.25f;
		protected bool  m_jumpInput       = false;

		public readonly    float m_interpolation = 0.01f;
		protected readonly float m_walkScale     = 0.33f;
		protected          float groundDistance  = 0.5f;

		public                   Transform groundCheckPoint;
		[HideInInspector] public Vector3   yOffset;

		protected virtual void Awake()
		{
			rigidBody = gameObject.GetComponent<Rigidbody>();
			collider  = GetComponent<CapsuleCollider>();
			inputs.OnStart();
			if (m_Animator)
				m_Animator.speed = inputs.AnimationMultiplier;
		}

		private void Update()
		{
			inputs.OnUpdate();
			if (GameManager.Instance.GameCurrentState == GameState.Gameplay || GameManager.Instance.GameCurrentState == GameState.FinalMomentum)
			{
				if (!m_jumpInput && inputs.Jump)
				{
					m_jumpInput = true;
				}
			}

			if (m_Animator)
				UpdateAnimator();
		}

		void FixedUpdate()
		{
			if (GameManager.Instance.GameCurrentState == GameState.Gameplay || GameManager.Instance.GameCurrentState == GameState.FinalMomentum)
			{
				GroundCheck();
				SetDownForce();
				DoFixedUpdate();
			}
		}

		void GroundCheck()
		{
			m_isGrounded = Physics.CheckCapsule(collider.bounds.center, new Vector3(collider.bounds.center.x, collider.bounds.min.y - groundDistance, collider.bounds.center.z), 0.35f, groundLayerMask);
		}


		protected abstract void UpdateAnimator();

		protected virtual void DoFixedUpdate()
		{
			Movement();
			m_jumpInput = false;
		}

		protected abstract void Movement();

		protected virtual void SetDownForce()
		{
			if (!m_jumpInput)
			{
				RaycastHit hit;
				Ray        downRay = new Ray(groundCheckPoint.position, -transform.up);
				Physics.Raycast(downRay, out hit, Mathf.Infinity, groundLayerMask);
				yOffset = transform.position - hit.point;
			}
		}

		protected virtual void JumpingAndLanding()
		{
			bool jumpCooldownOver = (Time.time - m_jumpTimeStamp) >= m_minJumpInterval;
			if (jumpCooldownOver && m_isGrounded && m_jumpInput)
			{
				m_jumpTimeStamp = Time.time;
				rigidBody.AddForce(Vector3.up * inputs.JumpForce, ForceMode.Impulse);
			}
		}

	}


}