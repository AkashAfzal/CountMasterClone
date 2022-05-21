using DG.Tweening;
using GameAssets.GameSet.GameDevUtils.Controller.Scripts;
using GameAssets.GameSet.GameDevUtils.Managers;
using TMPro;
using UnityEngine;


namespace GameAssets.GameSet.FinalMomentum.Scripts
{


	public class FinalMomentumCube : MonoBehaviour
	{

		[SerializeField] GameObject[] boxes;

		private Color       ColorToApply;
		bool                IsTriggered;
		static readonly int kColor = Shader.PropertyToID("_BaseColor");

		public void SetColor(Color colorToApply)
		{
			ColorToApply = colorToApply;
		}

		public void SetText(double numberX, string text)
		{
			transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text = $"{numberX}{text}";
			transform.gameObject.name                                         = $"Cube {numberX}";
		}

		public void ShowColor()
		{
			SetMeshMaterialColorProperty();
		}


		private void SetMeshMaterialColorProperty()
		{
			MaterialPropertyBlock prop           = new MaterialPropertyBlock();
			var                   myMeshRenderer = transform.GetComponent<Renderer>();
			prop.SetColor(kColor, ColorToApply);
			myMeshRenderer.SetPropertyBlock(prop);
		}


		void OnTriggerEnter(Collider other)
		{
			if (IsTriggered) return;
			IsTriggered = true;
			//CloseBoxes();
//			SoundManager.Instance.PlayOneShot(SoundManager.Instance.multiplierClip,1);
		}

		// private void CloseBoxes()
		// {
		// 	for (int i = 0; i < boxes.Length; i++)
		// 	{
		// 		var n    = i;
		// 		var tool = MakeUpStack.Instance.LastStackTool();
		// 		if (tool == null)
		// 		{
		// 			GameManager.Instance.ChangeGameState(GameState.Win);
		// 			return;
		// 		}
		//
		// 		// tool.gameObject.transform.parent = null;
		// 		SoundManager.Instance.PlayOneShot(SoundManager.Instance.makeUpToolClip, 1);
		// 		tool.transform.DOLocalMove(tool.gameObject.transform.localPosition.Plus(n == 1 ? -1 : 1, 3, 0), 0.3f).OnComplete(() =>
		// 		{
		// 			tool.transform.DOMove(boxes[n].transform.position, 0.15f).OnComplete(() =>
		// 			{
		// 				HapticFeedback.Generate(UIFeedbackType.ImpactMedium);
		// 				ShowColor();
		// 				boxes[n].GetComponent<Animation>().Play("BoxClose");
		// 				MakeUpStack.Instance.RemoveTool(tool);
		// 				
		// 			});
		// 		});
		// 	}
		// }

	}


}