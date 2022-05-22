using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gates : GateBase
{

	public MathOperation leftOperation;
	public int           leftValue;

	[Space(10)] public MathOperation rightOperation;
	public             int           rightValue;

	[Space(10)] [Header("Prefabs")] public GameObject leftGate;
	public                                 GameObject rightGate;
	public                                 TMP_Text   leftGateText;
	public                                 TMP_Text   rightGateText;


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