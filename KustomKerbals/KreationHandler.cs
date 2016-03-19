//Copyright (c) 2015, Clifton Marien. Original Copyright (c) 2014, Blake Meyler.
//
//Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files 
//(the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, 
//distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, 
//subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
//DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using KustomKerbals;

namespace KustomKerbals
{

	[KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
	public class KK : MonoBehaviour
	{
		private static Rect windowPosition = new Rect(0, 0, 370, 320);
		private static bool buttonState = false;
		private static bool male = true;
		public string gender = "Male";
		public static bool windowState = false;
		public static bool isKSCLocked = false;
		public string stringToEdit = "My Kustom Kerbal";
		public float sliderValue = 0.0f;
		public float sliderValue2 = 0.0f;
		public int kerbal { get; set; }
		public static string Path = (KSPUtil.ApplicationRootPath + HighLogic.CurrentGame + "peristent.sfs");
		string Trait = "Pilot";
		int traitInt = 0;
		private static ApplicationLauncherButton appLauncherButton;
		bool exists = false;
		bool warningState = false;
		public static Rect warningRect = new Rect (0, 0, 230, 180);
		bool overrideState = false;
		bool closeOnComplete = true;
		List<string> names = new List<string>();
		public static bool KrakenEnabled = false;
		public Rect krakenRect = new Rect(400, 400, 200, 400);
		public Vector2 KrakenScrollPosition = new Vector2(0, 0);
		bool KrakenArmed = false;

		public void Start()
		{
			
			Debug.Log("Kustom Kerbals loaded.");

			//Move the window to the center of the screen
			windowPosition.x = Screen.width / 2 - windowPosition.width / 2;
			windowPosition.y = Screen.height / 2 - windowPosition.height / 2;
			warningRect.x = Screen.width / 2 - warningRect.width / 2;
			warningRect.y = Screen.height / 2 - warningRect.height / 2;

			if (ApplicationLauncher.Ready && appLauncherButton == null) {

				appLauncherButton = ApplicationLauncher.Instance.AddModApplication(
					() => { toggleGUI(true); },
					() => { toggleGUI(false); },
					() => {}, // DoNothing! :)
					() => {},
					() => {},
					() => {},
					ApplicationLauncher.AppScenes.SPACECENTER,
					(Texture)GameDatabase.Instance.GetTexture("KustomKerbals/textures/toolbar", false));
			}

		}

		void toggleGUI(bool state) {

			windowState = state;

		}

		//Tells the window to open.
		public void Update()
		{

			if (male) {

				gender = "Male";

			} else {

				gender = "Female";

			}

			if (traitInt == 0) {

				Trait = "Pilot";

			} else if (traitInt == 1) {

				Trait = "Engineer";

			} else if (traitInt == 2) {

				Trait = "Scientist";

			}

			if ((GameSettings.MODIFIER_KEY.GetKey()) && Input.GetKeyDown(KeyCode.K))
			{
				windowState = !windowState;

				if (windowState) {
					
					Debug.Log ("KK window opened");

				}

			}
			if (windowState) {

				appLauncherButton.SetTrue (true);

			} else {

				appLauncherButton.SetFalse (true);

			}

			foreach (ProtoCrewMember kerb in HighLogic.CurrentGame.CrewRoster.Crew) {

				names.Add (kerb.name);

			}

			if (names.Contains (stringToEdit)) {

				exists = true;

			} else {

				exists = false;

			}

			if (windowState == false) {

				KrakenEnabled = false;

			}

		}

		//Gets a new kerbal and sets his/her stats.
		private void SpawnKerbal(int count, bool overrideState)
		{

			if (exists && overrideState == false) {

				warningState = true;

			} else {

				ProtoCrewMember kerbal = HighLogic.CurrentGame.CrewRoster.GetNewKerbal ();
				kerbal.name = stringToEdit;
				kerbal.courage = sliderValue;
				kerbal.stupidity = sliderValue2;
				kerbal.isBadass = buttonState;

				KerbalRoster.SetExperienceTrait (kerbal, Trait);

				//Find out the gender
				if (male == false) {

					kerbal.gender = ProtoCrewMember.Gender.Female;

				}
				if (male) {

					kerbal.gender = ProtoCrewMember.Gender.Male;

				}

				if (closeOnComplete) {

					appLauncherButton.SetFalse (true);
					ScreenMessages.PostScreenMessage ("Kustom Kerbal Spawned, closing window...", 1, ScreenMessageStyle.UPPER_CENTER);

				} else {

					ScreenMessages.PostScreenMessage ("Kustom Kerbal Spawned.", 1, ScreenMessageStyle.UPPER_CENTER);

				}

			}

		}

		#region rendering

		public void OnGUI()
		{

			//Set skin to KSP
			GUI.skin = HighLogic.Skin;

			//Checks if player has toggled the window on or off.
			if (windowState == true)
			{

				windowPosition = GUI.Window(1234, windowPosition, OnWindow, "Kustom Kerbals");

			}

			if (warningState) {

				warningRect = GUI.Window (1235, warningRect, warningWindow, "Warning: ");

			}

			if (KrakenEnabled) {

				krakenRect = GUI.Window (1236, krakenRect, krakenWindow, "Kraken Feeding");

			}

		}
			
		void warningWindow (int windowID) {

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("A Kerbal with this name already exists.  This can cause unexpected problems when both are assigned, or on EVA.");
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			overrideState = GUILayout.Toggle (overrideState, "Override");
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button("OK (Override: " + overrideState.ToString() + ")")) {

				if (overrideState) {

					SpawnKerbal(kerbal, true);
					warningState = false;
					overrideState = false;

				} else {

					warningState = false;

				}

			}
			GUILayout.EndHorizontal ();

		}

		private void OnWindow(int windowID)
		{

			//Add some space
			GUILayout.BeginHorizontal ();
			GUILayout.Label ("");
			GUILayout.EndHorizontal ();

			//Field to type in kerbal's name.
			GUILayout.BeginHorizontal();
			GUILayout.Label("Name:");
			stringToEdit = GUILayout.TextField(stringToEdit, 50);
			GUILayout.EndHorizontal();

			//Toggle female/male
			GUILayout.BeginHorizontal();
			if (GUILayout.Button (gender)) {

				male = !male;

			}
			if (GUILayout.Button (Trait)) {

				if (traitInt < 2) {

					traitInt += 1;

				} else {

					traitInt = 0;

				}

			}
			GUILayout.EndHorizontal();

			//Sets kerbal's courage. 
			GUILayout.BeginHorizontal();
			GUILayout.Label("Courage:");
			sliderValue = GUILayout.HorizontalSlider(sliderValue, 0.0f, 1.0f);
			GUILayout.EndHorizontal();

			//Sets kerbal's stupidity.
			GUILayout.BeginHorizontal();
			GUILayout.Label("Stupidity:");
			sliderValue2 = GUILayout.HorizontalSlider(sliderValue2, 0.0f, 1.0f);
			GUILayout.EndHorizontal();

			//Toggles BadS state.
			GUILayout.BeginHorizontal();
			buttonState = GUILayout.Toggle(buttonState, "BadS: " + buttonState);
			GUILayout.EndHorizontal();

			//Toggles close on complete
			GUILayout.BeginHorizontal();
			closeOnComplete = GUILayout.Toggle(closeOnComplete, "Close On Complete: " + closeOnComplete);
			GUILayout.EndHorizontal();

			//Button to create the kerbal using above paramaters.
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Kreate Kustom Kerbal")) {
				
				if (stringToEdit == "My Kustom Kerbal" && sliderValue == 0.0f && sliderValue2 == 0.0f && buttonState == false && male == true && Trait == "Pilot")
				{
					
					ProtoCrewMember kerbal = HighLogic.CurrentGame.CrewRoster.GetNewKerbal();
					kerbal.rosterStatus = ProtoCrewMember.RosterStatus.Available;
					ScreenMessages.PostScreenMessage("Random Kerbal Spawned.", 1, ScreenMessageStyle.UPPER_CENTER);
					Debug.Log("Random Kerbal Spawned");

				}
				else
				{
					
					Debug.Log ("Kustom Kerbal Kreated.");
					Debug.Log ("KK window closed.");
					SpawnKerbal (kerbal, false);

				}
			}
			if (GUILayout.Button ("Close")) {

				appLauncherButton.SetFalse (true);

			}

			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button("Kraken Feeding")) {

				KrakenEnabled = !KrakenEnabled;

			}
			GUILayout.EndHorizontal ();

			GUI.DragWindow();

		}

		#endregion


		#region KrakenHandler

		public void krakenWindow(int windowID) {

			GUILayout.BeginVertical ();
			GUILayout.Label ("Choose a Kerbal to feed to the Kraken");
			GUILayout.EndVertical ();
			Debug.Log ("Test");
			KrakenScrollPosition = GUILayout.BeginScrollView (KrakenScrollPosition);
			foreach (ProtoCrewMember kerb in HighLogic.CurrentGame.CrewRoster.Crew) {

				if (GUILayout.Button (kerb.name)) {

					if (KrakenArmed) {
						kerb.Die ();
						kerb.type = ProtoCrewMember.KerbalType.Unowned;
					}

				}

			}
			GUILayout.EndScrollView ();

			GUILayout.BeginVertical ();
			KrakenArmed = GUILayout.Toggle (KrakenArmed, "Armed: " + KrakenArmed.ToString());
			if (GUILayout.Button("Close")) {

				KrakenEnabled = false;

			}
			GUILayout.EndVertical ();

			GUI.DragWindow ();

		}

		#endregion


	}
}