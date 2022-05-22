using GameAssets.GameSet.GameDevUtils.Managers;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class CharacterBase : MonoBehaviour
{

	//Inspector Fields
	[SerializeField] private float      movementForce = 100f;
	[SerializeField] private GameObject dead_FX;
	[SerializeField] public  GameObject groundSpot;


	//Public Hidden Fields
	[HideInInspector] public int       id = -1;
	[HideInInspector] public Rigidbody rigidBody;
	[HideInInspector] public Animator  m_Animator;
	[HideInInspector] public Transform targetPos;
	[HideInInspector] public bool      isDead = false;
	//[HideInInspector] 
	public bool isMoveAble = false;


	//Private Fields
	private Vector3 MoveDirection;


	protected virtual void Start()
	{
		rigidBody  = GetComponent<Rigidbody>();
		m_Animator = GetComponent<Animator>();
	}

	private void Update()
	{
		if (GameManager.Instance.GameCurrentState == GameState.Gameplay || GameManager.Instance.GameCurrentState == GameState.FinalMomentum)
		{
			DoUpdate();
		}
	}

	void FixedUpdate()
	{
		if (GameManager.Instance.GameCurrentState == GameState.Gameplay || GameManager.Instance.GameCurrentState == GameState.FinalMomentum)
		{
			DoFixedUpdate();
		}
	}

	protected virtual void DoUpdate()
	{
	}

	protected virtual void DoFixedUpdate()
	{
		Movement();
	}

	protected virtual void Movement()
	{
		if (isMoveAble)
		{
			MoveDirection = (targetPos.position - transform.position).normalized;
			rigidBody.AddForce(MoveDirection * Vector3.Distance(targetPos.position, transform.position) * movementForce, ForceMode.Force);
		}
	}

	public virtual void StartMoving()
	{
		m_Animator.SetBool("Run", true);
		isMoveAble = true;
	}

	public virtual void StopMoving()
	{
		m_Animator.SetBool("Run", false);
		isMoveAble = false;
	}

	public virtual void Die()
	{
		SoundManager.Instance.PlayDeadSound();
		dead_FX.transform.parent = null;
		dead_FX.SetActive(true);
		groundSpot.SetActive(true);
		groundSpot.transform.parent = null;
		gameObject.SetActive(false);
	}

	public virtual void Victory()
	{
		isMoveAble = false;
		m_Animator.SetBool("Victory", true);
	}

}