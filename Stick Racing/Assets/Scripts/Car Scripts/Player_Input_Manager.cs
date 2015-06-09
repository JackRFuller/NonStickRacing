using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.Vehicles.Car
{
	public class Player_Input_Manager : MonoBehaviour {	

		[SerializeField] CarController CC_Script;		
		[SerializeField] GameObject[] Inputs = new GameObject[2];
		
		[Header("Input Settings")]
		public float InputSensitivity;
		public float HorizontalInput = 0;
		public float VerticalInput = 1;
		public float BrakingSensitivity;
		public float AccerlerationSensitivity;

		[Header("Brake Objects")]
		[SerializeField] GameObject[] Brakes = new GameObject[2];
		[SerializeField] Material[] BrakeID = new Material[2];
		
		// Use this for initialization
		void Start () {

			CC_Script = GameObject.Find ("Player_Car").GetComponent<CarController> ();
			VerticalInput = 1;

			foreach (GameObject Brake in Brakes)
			{
				Brake.GetComponent<Renderer>().material = BrakeID[0];
			}
			
		}
		
		// Update is called once per frame
		void FixedUpdate () {
			
			if(Input.touchCount > 0)
			{
				SendOutRaycast();
			}
			
			if(Input.touchCount == 0)
			{
				RemoveInput();
				AddAccelertation();
			}

			CC_Script.Move (HorizontalInput, VerticalInput, 0, 0F);
		}

		void AddAccelertation()
		{
			if (VerticalInput < 1)
			{
				VerticalInput += AccerlerationSensitivity;
				if(VerticalInput > 1)
				{
					VerticalInput = 1;
				}
			}
		}
		
		void RemoveInput()
		{
			
			HorizontalInput = 0;
			
			foreach (GameObject InputObj in Inputs)
			{
				InputObj.GetComponent<Renderer>().enabled = false;
			}

			foreach (GameObject Brake in Brakes)
			{
				Brake.GetComponent<Renderer>().material = BrakeID[0];
			}
		}
		
		void SendOutRaycast()
		{
			Ray ray = Camera.main.ScreenPointToRay (Input.GetTouch (0).position);
			
			RaycastHit hit;
			
			if (Physics.Raycast (ray, out hit, 100))
			{
				if(hit.collider.name == "Left")
				{
					hit.collider.gameObject.GetComponent<Renderer>().enabled = true;
					
					if(HorizontalInput > - 1)
					{
						HorizontalInput -= InputSensitivity;
						if(HorizontalInput < -1)
						{
							HorizontalInput = -1;
						}
					}
					
				}
				if(hit.collider.name == "Right")
				{
					hit.collider.gameObject.GetComponent<Renderer>().enabled = true;
					
					if(HorizontalInput < 1)
					{
						HorizontalInput += InputSensitivity;
						if(HorizontalInput > 1)
						{
							HorizontalInput = 1;
						}
					}
					
				}

				if(hit.collider.name == "Brake")
				{
					foreach (GameObject Brake in Brakes)
					{
						Brake.GetComponent<Renderer>().material = BrakeID[1];
					}

					if(VerticalInput > 0)
					{
						VerticalInput -= BrakingSensitivity;
						if(VerticalInput < 0)
						{
							VerticalInput = 0;
						}
					}
				}
			}
		}
		
		
	}

}

