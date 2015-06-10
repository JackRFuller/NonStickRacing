using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace UnityStandardAssets.Vehicles.Car
{
	public class RaceManager : MonoBehaviour {

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
		[SerializeField] private string LapFormattedTime;
		[SerializeField] private float CountdownTimer;
		[SerializeField] private int CurrentLap = 0;
		[SerializeField] private int MaxNumOfLaps;

		
		[Header("General UI Variables")]
		[SerializeField] Text RaceTimeText;
		[SerializeField] Text CountdownTimerText;
		[SerializeField] Text LapText;
		[SerializeField] Text[] IndividualLaps;		
		
		// Use this for initialization
		void Start () {

			CC_Script = GameObject.Find ("Player_Car").GetComponent<CarController> ();
			
			StartCoroutine (CountdownTimerManager ());

			LapText.text = "Current Lap: " + CurrentLap.ToString() + " / " + MaxNumOfLaps.ToString();
					
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

			float time = RaceTime;
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
			if(CheckpointNumber == 0)
			{
				CurrentLap += 1;
				UpdateLapText();
			}
		}

		void UpdateLapText()
		{
			LapText.text = "Current Lap: " + CurrentLap.ToString() + " / " + MaxNumOfLaps.ToString();
			if(CurrentLap > 1)
			{
				int SpecificLap = CurrentLap - 2;
				IndividualLaps[SpecificLap].text = "Lap " + SpecificLap.ToString() + ": " + LapFormattedTime;
				IndividualLaps[SpecificLap].transform.parent.GetComponent<Animation>().Play();
				CurrentLapTime = 0;
			}
		}
	}

}

