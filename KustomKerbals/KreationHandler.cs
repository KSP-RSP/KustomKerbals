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
using System.IO;
using UnityEngine;

namespace KustomKerbals
{

	[KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
	public class KK : MonoBehaviour
	{
		private static Rect windowPosition = new Rect(0, 0, 370, 265);
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

		public void Start()
		{
			
			Debug.Log("Kustom Kerbals loaded.");

			//Move the window to the center of the screen
			windowPosition.x = Screen.width / 2 - windowPosition.width / 2;
			windowPosition.y = Screen.height / 2 - windowPosition.height / 2;

		}

		//Tells the window to open.
		public void Update()
		{

			if (male) {

				gender = "Male";

			} else {

				gender = "Female";

			}

			if ((GameSettings.MODIFIER_KEY.GetKey()) && Input.GetKeyDown(KeyCode.K))
			{
				windowState = !windowState;

				Debug.Log("KK window opened");

			}
		}

		//Gets a new kerbal and sets his/her stats.
		private void SpawnKerbal(int count)
		{

			ProtoCrewMember kerbal = HighLogic.CurrentGame.CrewRoster.GetNewKerbal();
			kerbal.name = stringToEdit;
			kerbal.courage = sliderValue;
			kerbal.stupidity = sliderValue2;
			kerbal.isBadass = buttonState;


			//Find out gender
			if (male == false) {

				kerbal.gender = ProtoCrewMember.Gender.Female;

			}
			if (male) {

				kerbal.gender = ProtoCrewMember.Gender.Male;

			}

			//How to set trait
			//				KerbalRoster.SetExperienceTrait(kerbal, "Pilot");
			//				KerbalRoster.SetExperienceTrait(kerbal, "Engineer");
			//				KerbalRoster.SetExperienceTrait(kerbal, "Scientist");
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

		}

		private void OnWindow(int windowID)
		{
			//Warn the user not to make multiple kerbals to prevent errors
			GUILayout.BeginHorizontal();
			GUILayout.Label("Note: do not create multiple kerbals with the same name.");
			GUILayout.EndHorizontal();

			//Field to type in kerbal's name.
			GUILayout.BeginHorizontal();
			GUILayout.Label("Name:");
			stringToEdit = GUILayout.TextField(stringToEdit, 50);
			GUILayout.EndHorizontal();

			//Toggle female/male
			GUILayout.BeginHorizontal();
			GUILayout.Label("Toggle Male/Female: ");
			if (GUILayout.Button (gender)) {

				male = !male;

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

			//Button to create the kerbal using above paramaters.
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Kreate Kustom Kerbal")) {
				
				if (stringToEdit == "Jebediah Kerman Jr." && sliderValue == 0.0f && sliderValue2 == 0.0f && buttonState == false && male == true)
				{
					
					ProtoCrewMember kerbal = HighLogic.CurrentGame.CrewRoster.GetNewKerbal();
					kerbal.rosterStatus = ProtoCrewMember.RosterStatus.Available;
					windowState = false;
					ScreenMessages.PostScreenMessage("Random Kerbal Spawned, closing window...", 1, ScreenMessageStyle.UPPER_CENTER);
					Debug.Log("Random Kerbal Spawned");
				}
				else
				{
					
					Debug.Log("Kustom Kerbal Kreated.");
					Debug.Log("KK window closed.");
					SpawnKerbal(kerbal);
					windowState = false;
					ScreenMessages.PostScreenMessage("Kustom Kerbal Spawned, closing window...", 1, ScreenMessageStyle.UPPER_CENTER);

				}
			}
			GUILayout.EndHorizontal ();

			GUI.DragWindow();
		}

		#endregion
	}
}