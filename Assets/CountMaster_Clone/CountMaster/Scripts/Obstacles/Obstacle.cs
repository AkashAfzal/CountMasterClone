using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : ObstacleBase
{

	void Start()
	{
		switch (obstacleType)
		{
			case ObstacleType.Rotate:
				StartCoroutine(Rotate(rotateDirection));
				break;

			case ObstacleType.RotateBTWAngles:
				StartCoroutine(RotateBTWAngles(rot1, rot2));
				break;
		}
	}

}