using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace SimpleInputNamespace
{


	public class SteeringWheel : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
	{

		[SerializeField] float maximumSteeringAngle = 200f;
		[SerializeField] float wheelReleasedSpeed   = 350f;

		private Graphic       SteerWheel;
		private RectTransform SteerWheelTR;
		private Vector2       CenterPoint;
		private float         WheelAngle     = 0f;
		private float         WheelPrevAngle = 0f;
		private bool          WheelBeingHeld = false;

		private float Horizontal;
		private float Vertical;


		void OnEnable()
		{
			SteeringInput.inputEvent += InputUpdate;
		}

		void OnDisable()
		{
			SteeringInput.inputEvent -= InputUpdate;
		}


		private void Awake()
		{
			SteerWheel   = GetComponent<Graphic>();
			SteerWheelTR = SteerWheel.rectTransform;
		}
		
		void InputUpdate(float sensitivity, out float horizontal, out float vertical)
		{
			
			
			
			// If the wheel is released, reset the rotation
			// to initial (zero) rotation by wheelReleasedSpeed degrees per second
			if (!WheelBeingHeld && WheelAngle != 0f)
			{
				float deltaAngle = wheelReleasedSpeed * Time.deltaTime;
				if (Mathf.Abs(deltaAngle) > Mathf.Abs(WheelAngle))
					WheelAngle = 0f;
				else if (WheelAngle > 0f)
					WheelAngle -= deltaAngle;
				else
					WheelAngle += deltaAngle;
			}

			// Rotate the wheel image
			SteerWheelTR.localEulerAngles = new Vector3(0f, 0f, -WheelAngle);
			Horizontal                    = WheelAngle * sensitivity / maximumSteeringAngle;
			
			//out values to Steering Input
			horizontal                    = Horizontal;
			vertical                      = Vertical;

		}

		public void OnPointerDown(PointerEventData eventData)
		{
			
			// Executed when mouse/finger starts touching the steering wheel
			SteerWheelTR.transform.DOScale(new Vector3(0.95f, 0.95f, 0.95f), 0.2f).SetEase(Ease.Linear);
			WheelBeingHeld = true;
			Vertical       = 1;
			CenterPoint    = RectTransformUtility.WorldToScreenPoint(eventData.pressEventCamera, SteerWheelTR.position);
			WheelPrevAngle = Vector2.Angle(Vector2.up, eventData.position - CenterPoint);
		}

		public void OnDrag(PointerEventData eventData)
		{
			// Executed when mouse/finger is dragged over the steering wheel
			Vector2 pointerPos    = eventData.position;
			float   wheelNewAngle = Vector2.Angle(Vector2.up, pointerPos - CenterPoint);

			// Do nothing if the pointer is too close to the center of the wheel
			if ((pointerPos - CenterPoint).sqrMagnitude >= 400f)
			{
				if (pointerPos.x > CenterPoint.x)
					WheelAngle += wheelNewAngle - WheelPrevAngle;
				else
					WheelAngle -= wheelNewAngle - WheelPrevAngle;
			}

			// Make sure wheel angle never exceeds maximumSteeringAngle
			WheelAngle     = Mathf.Clamp(WheelAngle, -maximumSteeringAngle, maximumSteeringAngle);
			WheelPrevAngle = wheelNewAngle;
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			// Executed when mouse/finger stops touching the steering wheel
			// Performs one last OnDrag calculation, just in case
			OnDrag(eventData);
			Vertical       = 0;
			SteerWheelTR.transform.DOScale(new Vector3(1f,    1f, 1f), 0.2f).SetEase(Ease.Linear);
			WheelBeingHeld = false;
		}

	}


}