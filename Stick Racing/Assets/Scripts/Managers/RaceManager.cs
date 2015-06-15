using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace UnityStandardAssets.Vehicles.Car
{
	public class RaceManager : MonoBehaviour {

		[SerializeField] private GameObject PlayersCar;
		private CarController CC_Script;
		
		public enum RaceModes
		{
			TimeTrial
		}
		
		public RaceModes CurrentRaceMode;
		
		[Header("General Race Variables")]
		public bool RaceStarted = false;
		public bool RaceFinished = false;
		[SerializeField] private float RaceTime = 0;
		[SerializeField] private float CurrentLapTime = 0;
		[SerializeField] private float LastLapTime;
		[SerializeField] private string LapFormattedTime;
		[SerializeField] private float CountdownTimer;
		[SerializeField] private int CurrentLap = 0;
		[SerializeField] private int MaxNumOfLaps;

		
		[Header("General UI Variables")]
		[SerializeField] Text RaceTimeText;
		[SerializeField] Text CountdownTimerText;
		[SerializeField] Text LapText;
		[SerializeField] Text[] IndividualLaps;	

		[Header("Checkpoint Variables")]
		[SerializeField] private GameObject[] Checkpoints;
		[SerializeField] private int CurrentCheckpoint = 0;
		[SerializeField] private int MaxNumofCheckpoints;
		[SerializeField] private GameObject CurrentCheckpointObj;

		[Header("FinishingScreen Variables")]
		[SerializeField] private GameObject FinishingScreen;


		// Use this for initialization
		void Start () {

			PlayersCar = GameObject.Find ("Player_Car");
			CC_Script = GameObject.Find ("Player_Car").GetComponent<CarController> ();			
			StartCoroutine (CountdownTimerManager ());
			LapText.text = "Current Lap: " + CurrentLap.ToString() + " / " + MaxNumOfLaps.ToString();

			Checkpoints = GameObject.FindGameObjectsWithTag ("Checkpoint");
			MaxNumofCheckpoints = Checkpoints.Length;
					
		}
		
		// Update is called once per frame
		void Update () {
			
			
			if(RaceStarted)
			{
				RaceTimer();
				LapTimer();
			}
			
		}
		
		void RaceTimer()
		{
			RaceTime += Time.deltaTime;
			
			float time = RaceTime;
			float minutes = (int)time / 60;
			float seconds = (int)time % 60;
			float fraction = RaceTime * 1000;
			fraction = fraction % 1000;
			
			string FormattedTime = string.Format ("{0:00}:{1:00}:{2:00}", minutes, seconds, fraction);
			RaceTimeText.text = "Time: " + FormattedTime;
			
		}

		void LapTimer()
		{
			CurrentLapTime += Time.deltaTime;

			float time = CurrentLapTime;
			float minutes = (int)time / 60;
			float seconds = (int)time % 60;
			float fraction = RaceTime * 1000;
			fraction = fraction % 1000;
			
			LapFormattedTime = string.Format ("{0:00}:{1:00}:{2:00}", minutes, seconds, fraction);
		}
		
		IEnumerator CountdownTimerManager()
		{
			for(int i = 3; i > -1; i--)
			{
				yield return new WaitForSeconds(1.0F);
				CountdownTimerText.text = i.ToString();
				if(i == 0)
				{
					CC_Script.CarMoving = true;
					CountdownTimerText.text = "Go!";
					yield return new WaitForSeconds(1.0F);
					CountdownTimerText.enabled = false;
					RaceStarted = true;

				}
			}
		}

		public void ManageCheckpoints(int CheckpointNumber)
		{
			if(RaceStarted)
			{
				if(CurrentCheckpoint == 0 && CheckpointNumber == 0)
				{				
					CurrentLap += 1;
					if(CurrentLap > MaxNumOfLaps)
					{
						CurrentLap = MaxNumOfLaps;
						FinishedRace();
					}

					UpdateLapText();
					CurrentLapTime = 0;

				}
				
				if(CheckpointNumber == CurrentCheckpoint)
				{
					CurrentCheckpoint++;
					if(CurrentCheckpoint == Checkpoints.Length)
					{
						CurrentCheckpoint = 0;
					}
				}

				RevealCheckpoint();
			}
		}

		void RevealCheckpoint()
		{
			CurrentCheckpointObj.GetComponent<Renderer> ().enabled = false;
			foreach(GameObject Checkpoint in Checkpoints)
			{
				CheckpointBehaviour CB_Script = Checkpoint.GetComponent<CheckpointBehaviour>();
				if(CB_Script.CheckpointID == CurrentCheckpoint)
				{
					CurrentCheckpointObj = Checkpoint;
					CurrentCheckpointObj.GetComponent<Renderer>().enabled = true;
				}
			}
		}

		public void Reset()
		{
			Application.LoadLevel (Application.loadedLevelName);
		}

		void FinishedRace()
		{
			RaceStarted = false;
		}

		void UpdateLapText()
		{
			LapText.text = "Current Lap: " + CurrentLap.ToString() + " / " + MaxNumOfLaps.ToString();

			if(CurrentLap > 1)
			{


				if(CurrentLap == 2)
				{
					LastLapTime = CurrentLapTime;
					IndividualLaps[0].text = "Last Lap: " + LapFormattedTime;
					CurrentLapTime = 0;


					IndividualLaps[0].text = "Last Lap: " + LapFormattedTime;
					IndividualLaps[0].transform.parent.GetComponent<Animation>().Play();
				}
				if(CurrentLap > 2)
				{
					float LapDifference = LastLapTime - CurrentLapTime;
					string LapDifferenceTime = "";

					bool TimeConverted = false;

					if(LapDifference > 0 && !TimeConverted)
					{
						LapDifference = -LapDifference;
						LapDifferenceTime = LapDifference.ToString("F2");
						TimeConverted = true;
					}

					if(LapDifference < 0 && !TimeConverted)
					{
						LapDifference = -LapDifference;
						Debug.Log(LapDifference);
						LapDifferenceTime = "+" + LapDifference.ToString("F2");
						TimeConverted = true;
					}
					IndividualLaps[0].text = "Last Lap: " + LapFormattedTime + " (" + LapDifferenceTime.ToString() + ")";
				}


				CurrentLapTime = 0;


//				int SpecificLap = CurrentLap - 2;
//				int LapNumber = SpecificLap + 1;
//				IndividualLaps[SpecificLap].text = "Lap " + LapNumber.ToString() + ": " + LapFormattedTime;
//				IndividualLaps[SpecificLap].transform.parent.GetComponent<Animation>().Play();
//				CurrentLapTime = 0;
			}
		}
	}

}

