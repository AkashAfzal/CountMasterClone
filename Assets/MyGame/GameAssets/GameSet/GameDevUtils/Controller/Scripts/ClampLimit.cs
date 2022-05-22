using UnityEngine;

namespace GameAssets.GameSet.GameDevUtils.Controller.Scripts
{


	public class ClampLimit : MonoBehaviour
	{
		[SerializeField] bool  isOverrideDefaultLimit;
		public           float timeToCompleteLerp;
		public           float minXLimit;
		public           float maxXLimit;
		bool                   IsTriggered;

		void OnTriggerEnter(Collider other)
		{
			if (IsTriggered && !other.GetComponent<SlideController>()) return;
			IsTriggered = true;
			other.GetComponent<SlideController>()?.SetClampXLimit(isOverrideDefaultLimit, minXLimit, maxXLimit, timeToCompleteLerp);
			
		}

	}


}