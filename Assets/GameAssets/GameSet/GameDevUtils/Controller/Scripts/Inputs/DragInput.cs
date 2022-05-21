using GameAssets.GameSet.GameDevUtils.Controller.Scripts;
using UnityEngine;


[CreateAssetMenu(menuName = "GameDevUtils/DragInput")]
public class DragInput : InputBase
{

	private Vector3 prevPosition;

	public override void OnUpdate()
	{
		if (!isBlockAllInput)
		{
			if (isAutoRun)
				Vertical = 1;
			if (Input.GetMouseButtonDown(0))
			{
				Vertical     = 1;
				prevPosition = Input.mousePosition;
			}

			if (Input.GetMouseButton(0))
			{
				Vector3 touchDelta    = Input.mousePosition - prevPosition; // screen touch delta
				var     positionDelta = touchDelta * Sensitivity;
				positionDelta.x /= Screen.width / 2f;
				Horizontal      =  positionDelta.x;
				prevPosition    =  Input.mousePosition;
			}
			else
			{
				if (!isAutoRun)
					Vertical = 0;
				Horizontal = 0;
			}
		}
		else
		{
			Vertical     = 0;
			Horizontal   = 0;
			prevPosition = Input.mousePosition;
		}
	}

}