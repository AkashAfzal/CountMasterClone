using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GateBase : MonoBehaviour
{

	public Material decreaseGateMaterial;

	public enum MathOperation
	{

		Addition,
		Multiplication,
		Division,
		Subtraction

	}

	public virtual void SetGateTexts(MathOperation operation, int val, TMPro.TMP_Text textHolder, GameObject gatePlane)
	{
		switch (operation)
		{
			case MathOperation.Addition:
				textHolder.text = "+" + val.ToString();
				break;

			case MathOperation.Multiplication:
				textHolder.text = "x" + val.ToString();
				break;

			case MathOperation.Subtraction:
				textHolder.text                             = "-" + val.ToString();
				gatePlane.GetComponent<Renderer>().material = decreaseGateMaterial;
				break;

			case MathOperation.Division:
				textHolder.text                             = "/" + val.ToString();
				gatePlane.GetComponent<Renderer>().material = decreaseGateMaterial;
				break;
		}
	}

	public virtual void MathCall(MathOperation _op, int val)
	{
		switch (_op)
		{
			case MathOperation.Addition:
				PlayerController.Instance.AddCharacter(val, false);
				break;

			case MathOperation.Multiplication:
				PlayerController.Instance.AddCharacter(PlayerController.Instance.currentCharacterSize * val - PlayerController.Instance.currentCharacterSize, false);
				break;

			case MathOperation.Subtraction:
				PlayerController.Instance.RemoveCharacter(val);
				break;

			case MathOperation.Division:
				PlayerController.Instance.RemoveCharacter(PlayerController.Instance.currentCharacterSize - PlayerController.Instance.currentCharacterSize / val);
				break;
		}
	}

}