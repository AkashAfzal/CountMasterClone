using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharacterBase
{

	public override void Die()
	{
		if (!isDead)
		{
			isDead = true;
			base.Die();
			transform.parent.GetComponent<EnemyCollection>().CharacterDeath(id);
		}
	}

	private void OnCollisionStay(Collision collision)
	{
		if (collision.transform.CompareTag("Character") && !isDead)
		{
			if (!collision.transform.GetComponent<Character>().isDead)
			{
				collision.transform.GetComponent<Character>().Die();
				Die();
			}
		}
	}

}