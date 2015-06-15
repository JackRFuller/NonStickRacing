using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.Vehicles.Car
{
	public class CheckpointBehaviour : MonoBehaviour {

		public int CheckpointID;
		[SerializeField] private RaceManager RM_Script;
		
		// Use this for initialization
		void Start () {

			RM_Script = GameObject.Find ("RaceManager").GetComponent<RaceManager> ();
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		void OnTriggerEnter(Collider col)
		{
			if(col.GetComponent<Collider>().tag == "Player")
			{
				RM_Script.ManageCheckpoints(CheckpointID);
			}
		}
	}
}


