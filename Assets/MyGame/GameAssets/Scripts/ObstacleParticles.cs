using UnityEngine;

public class ObstacleParticles : MonoBehaviour
{

	[SerializeField] GameObject[] particles;

	public void PlaySparkParticles()
	{
		foreach (GameObject particle in particles)
		{
			particle.SetActive(true);
		}
	}

}