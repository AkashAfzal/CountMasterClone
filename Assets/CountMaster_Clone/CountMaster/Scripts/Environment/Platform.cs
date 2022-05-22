using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{

	Material roadMat;

	void Start()
	{
		roadMat = GetComponent<Renderer>().material;
		roadMat.SetTextureScale("_BaseMap", new Vector2(1f, transform.parent.localScale.z));
	}

}