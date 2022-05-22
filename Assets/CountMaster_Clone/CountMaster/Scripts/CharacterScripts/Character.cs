using DG.Tweening;
using UnityEngine;

public class Character : CharacterBase
{

	[SerializeField] private float jumpInterval     = .1f;
	[SerializeField] private float jumpUpInterval   = 0.5f;
	[SerializeField] private float jumpFallInterval = .55f;

	private bool IsJumping = false;

	protected override void Start()
	{
		StartMoving();
		targetPos = transform.parent;
	}


	private void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.CompareTag("Obstacle") && !IsJumping)
		{
			Die();
		}

		if (collision.transform.CompareTag("Finish"))
		{
			PlayerController.Instance.Finish();
			collision.collider.enabled = false;
		}

		if (collision.transform.CompareTag("EnemyCollection"))
		{
			PlayerController.Instance.FightWithEnemyGroup(collision.transform.GetComponent<EnemyCollection>());
			collision.collider.enabled = false;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.CompareTag("Ramp"))
		{
			isMoveAble = false; 
			Jump(other.GetComponent<Ramp>().rampEnd);
		}
	}

	public override void Die()
	{
		if (!isDead)
		{
			isDead = true;
			PlayerController.Instance.CharacterDeath(id);
			base.Die();
		}
	}

	public void FinishPlacement(Vector3 localPos)
	{
		transform.DOLocalMove(localPos, 0.3f).SetEase(Ease.Linear);
	}

	void Jump(Transform target)
	{
		IsJumping = true;
		Vector3 endValue = transform.localPosition.SetY(target.position.y);
		transform.DOLocalMove(endValue, jumpUpInterval).SetEase(Ease.Linear).OnComplete(() =>
		{
			Vector3 endValue1 = transform.localPosition.SetY(transform.localPosition.y + Random.Range(0.3f, 0.7f));
			transform.DOLocalMove(endValue1, Random.Range(jumpInterval, jumpInterval - 0.1f)).SetEase(Ease.Linear).OnComplete(() =>
			{
				Vector3 endValue2 = transform.localPosition.SetY(0);
				transform.DOLocalMove(endValue2, jumpFallInterval).SetEase(Ease.Linear).OnComplete(() =>
				{
					IsJumping  = false;
					isMoveAble = true;
				});
			});
		});
	}

}