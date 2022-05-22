using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObstacleBase : MonoBehaviour
{

	public ObstacleType obstacleType;
	public float        rotateSpeed;

	[Header("RotatorSettings")] public Vector3 rotateDirection;

	[Header("RotateBTWAnglesSettings")] public Vector3 rot1;
	public                                     Vector3 rot2;
	public                                     Vector3 rot3;
	public                                     Vector3 rot4;

	public enum ObstacleType
	{

		Rotate,
		RotateBTWAngles

	}

	public IEnumerator Rotate(Vector3 rotDir)
	{
		while (true)
		{
			transform.Rotate(rotDir * rotateSpeed * Time.deltaTime);
			yield return null;
		}
	}

	public IEnumerator RotateBTWAngles(Vector3 rot1, Vector3 rot2)
	{
		Quaternion toAngle1 = Quaternion.Euler(rot1);
		Quaternion toAngle2 = Quaternion.Euler(rot2);
		Quaternion toAngle3 = Quaternion.Euler(rot3);
		Quaternion toAngle4 = Quaternion.Euler(rot4);
		while (true)
		{
			while (transform.localRotation != toAngle1)
			{
				transform.localRotation = Quaternion.Slerp(transform.localRotation, toAngle1, rotateSpeed * Time.deltaTime);
				yield return null;
			}

			while (transform.localRotation != toAngle2)
			{
				transform.localRotation = Quaternion.Slerp(transform.localRotation, toAngle2, rotateSpeed * Time.deltaTime);
				yield return null;
			}

			while (transform.localRotation != toAngle3)
			{
				transform.localRotation = Quaternion.Slerp(transform.localRotation, toAngle3, rotateSpeed * 2f * Time.deltaTime);
				yield return null;
			}

			while (transform.localRotation != toAngle4)
			{
				transform.localRotation = Quaternion.Slerp(transform.localRotation, toAngle4, rotateSpeed * 2f * Time.deltaTime);
				yield return null;
			}

			yield return null;
		}
	}

}