using UnityEngine;

public abstract class GateBase : MonoBehaviour
{

	public enum MathOperation
	{

		Addition,
		Multiplication,

	}

	protected virtual void SetGateTexts(MathOperation operation, int val, TMPro.TMP_Text textHolder, GameObject gatePlane)
	{
		textHolder.text = operation == MathOperation.Addition ? "+" + val.ToString() : "x" + val.ToString();
	}

	protected virtual void MathCall(MathOperation _op, int val)
	{
		PlayerController.Instance.AddNewCharacter(_op == MathOperation.Addition ? val : PlayerController.Instance.currentCharacterSize * val - PlayerController.Instance.currentCharacterSize, false);
	}

}