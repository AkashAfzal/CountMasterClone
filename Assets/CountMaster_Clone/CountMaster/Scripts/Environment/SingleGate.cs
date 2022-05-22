using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleGate : GateBase
{

	public MathOperation operation;
	public int           value;

	[Header("Prefabs")] public GameObject     gate;
	public                     TMPro.TMP_Text gateText;

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