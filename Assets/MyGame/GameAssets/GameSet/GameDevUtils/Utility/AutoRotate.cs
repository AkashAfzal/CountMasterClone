using UnityEngine;

public class AutoRotate : MonoBehaviour
{

	public float   rotateSpeed;
	public Vector3 rotation;

	void Update()
	{
		transform.Rotate(rotation * rotateSpeed * Time.deltaTime);
	}

}