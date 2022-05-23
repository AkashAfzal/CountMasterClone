using UnityEngine;

public class SingleGate : GateBase
{

	[SerializeField] MathOperation operation;
	[SerializeField] int           value;

	[SerializeField] GameObject     gate;
	[SerializeField] TMPro.TMP_Text gateText;

	private void Start()
	{
		SetGateTexts(operation, value, gateText, gate);
	}

	private void OnTriggerEnter(Collider other)
	{
		MathCall(operation, value);
		gate.SetActive(false);
	}

}