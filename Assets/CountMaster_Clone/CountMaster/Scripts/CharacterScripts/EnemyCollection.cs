using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyCollection : MonoBehaviour
{

	public  GameObject enemyPrefab;
	public  int        enemySize;
	public  TMP_Text   enemySizeText;
	private int        currentEnemySize = 0;

	private List<GameObject> enemies = new List<GameObject>();

	[Header("ParticleSettings")] public GameObject particle;
	public                              float      particleScaleFactor = 1.5f;
	public                              float      particleScaleSpeed  = 10f;
	public                              float      particleAlphaSpeed  = 10f;

	private void Start()
	{
		AddEnemy(enemySize);
	}

	public void AddEnemy(int size)
	{
		for (int i = 0; i < size; i++)
		{
			Vector3    pos            = new Vector3(Random.Range(transform.position.x - 1, transform.position.x + 1), transform.position.y, Random.Range(transform.position.z - 1, transform.position.z + 1));
			GameObject addedCharacter = Instantiate(enemyPrefab, pos, Quaternion.identity);
			addedCharacter.transform.parent            = transform;
			addedCharacter.transform.localEulerAngles  = Vector3.zero;
			addedCharacter.GetComponent<Enemy>().id = enemies.Count;
			currentEnemySize++;
			enemies.Add(addedCharacter);
			UpdateEnemySizeText();
		}
	}

	private void UpdateEnemySizeText()
	{
		enemySizeText.text = currentEnemySize.ToString();
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.CompareTag("Character"))
		{
			Attack();
		}
	}

	private void Attack()
	{
		for (int i = 0; i < enemies.Count; i++)
		{
			enemies[i].GetComponent<Enemy>().targetPos = PlayerController.Instance.transform;
			enemies[i].GetComponent<Enemy>().StartMoving();
		}
	}

	public void CharacterDeath(int index)
	{
		GameObject removedCharacter = enemies[index];
		enemies.RemoveAt(index);
		for (int i = enemies.Count - 1; i > index - 1; i--)
		{
			if (i > 0)
			{
				enemies[i].GetComponent<Enemy>().id = i;
			}
			else
			{
				enemies[i].GetComponent<Enemy>().id = 0;
			}
		}

		removedCharacter.transform.parent = null;
		removedCharacter.gameObject.SetActive(false);
		currentEnemySize--;
		UpdateEnemySizeText();
		if (enemies.Count == 0)
		{
			PlayerController.Instance.FightEnded();
			enemySizeText.transform.parent.gameObject.SetActive(false);
			StartCoroutine(ParticleColorAnimation());
			StartCoroutine(ParticleScaleAnimation());
		}
	}

	public void Victory()
	{
		for (int i = 0; i < enemies.Count; i++)
		{
			enemies[i].GetComponent<Enemy>().Victory();
		}
	}

	IEnumerator ParticleScaleAnimation()
	{
		Vector3 targetScale = particle.transform.localScale * particleScaleFactor;
		while (particle.transform.localScale != targetScale)
		{
			particle.transform.localScale = Vector3.MoveTowards(particle.transform.localScale, targetScale, particleScaleSpeed * Time.deltaTime);
			yield return null;
		}
	}

	IEnumerator ParticleColorAnimation()
	{
		Color col = particle.GetComponent<ParticleSystem>().startColor;
		while (col.a != 0f)
		{
			float newAlpha = Mathf.MoveTowards(col.a, 0f, particleAlphaSpeed * Time.deltaTime);
			col.a                                              = newAlpha;
			particle.GetComponent<ParticleSystem>().startColor = col;
			yield return null;
		}
	}

}