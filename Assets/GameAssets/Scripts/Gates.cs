using UnityEngine;
using TMPro;

public class Gates : GateBase
{

	[SerializeField] MathOperation leftOperation;
	[SerializeField] int           leftValue;
	[SerializeField] MathOperation rightOperation;
	[SerializeField] int           rightValue;
	
	[SerializeField] GameObject    leftGate;
	[SerializeField] GameObject    rightGate;
	[SerializeField] TMP_Text      leftGateText;
	[SerializeField] TMP_Text      rightGateText;


	private void Start()
	{
		SetGateTexts(leftOperation,  leftValue,  leftGateText,  leftGate);
		SetGateTexts(rightOperation, rightValue, rightGateText, rightGate);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.position.x < 0)
		{
			MathCall(leftOperation, leftValue);
			leftGate.SetActive(false);
		}
		else
		{
			MathCall(rightOperation, rightValue);
			rightGate.SetActive(false);
		}
	}

}