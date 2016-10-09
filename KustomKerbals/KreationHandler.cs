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
using KSP.UI.Screens;

namespace KustomKerbals
{

	[KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
	public class KK : MonoBehaviour
	{

        #region defs

        //Bools
        private static bool buttonState = false;
		private static bool male = true;
        bool veteran = false;
		public static bool windowState = false;
		public static bool isKSCLocked = false;
		bool exists = false;
		bool warningState = false;
		bool overrideState = false;
		bool closeOnComplete = true;
		public static bool KrakenEnabled = false;
		bool KrakenArmed = false;
		public static bool editorEnabled = false;
		private static bool editMale = true;
		private static bool editBadS = false;
        bool editVeteran = false;
        bool krakenFloat = false;
		bool editFloat = false;
		bool editExists = false;
		bool editOverride = false;

		//Strings
		public string gender = "Male";
		public string stringToEdit = "My Kustom Kerbal";
		public static string Path = (KSPUtil.ApplicationRootPath + HighLogic.CurrentGame + "peristent.sfs");
		string Trait = "Pilot";
		public string editGender = "Male";
		public string editName = "No Kerbal Selected!";
		string editTrait = "Pilot";

		//floats
		public float sliderValue = 0.0f;
		public float sliderValue2 = 0.0f;
		public float editCourage = 0.0f;
		public float editStupidity = 0.0f;

		//Ints
		public int kerbal { get; set; }
		int traitInt = 0;
		int editTraitInt = 0;

		//Rects
		private static Rect windowPosition = new Rect(0, 0, 370, 325);
		public Rect krakenRect = new Rect(0, 0, 200, 400);
		public static Rect warningRect = new Rect (0, 0, 230, 180);
		private static Rect editorRect = new Rect(0, 0, 370, 400);

		//Lists
		List<string> names = new List<string>();

		//Vector2s
		public Vector2 KrakenScrollPosition = new Vector2(0, 0);
		public Vector2 editorScrollPosition = new Vector2(0, 0);

		//Etc.
		private static ApplicationLauncherButton appLauncherButton;
		ProtoCrewMember kerbalToEdit;
        ProtoCrewMember newKerbal;
        bool getRandomKerbal;

        #endregion

        public void Start()
		{
			
			Debug.Log("Kustom Kerbals loaded.");

			//Move the window to the center of the screen
			windowPosition.x = Screen.width / 2 - windowPosition.width / 2;
			windowPosition.y = Screen.height / 2 - windowPosition.height / 2;
			warningRect.x = Screen.width / 2 - warningRect.width / 2;
			warningRect.y = Screen.height / 2 - warningRect.height / 2;
			//Move window to next to the main window
			krakenRect.x = windowPosition.x - windowPosition.width / 2 - 15;
			krakenRect.y = windowPosition.y;
			editorRect.x = windowPosition.x + windowPosition.width;
			editorRect.y = windowPosition.y;

			//Thanks bananashavings http://forum.kerbalspaceprogram.com/index.php?/profile/156147-bananashavings/ - https://gist.github.com/bananashavings/e698f4359e1628b5d6ef
			//Also thanks to Crzyrndm for the fix to that code!
			if (appLauncherButton == null) {

				appLauncherButton = ApplicationLauncher.Instance.AddModApplication(
					() => { toggleGUI(true); },
					() => { toggleGUI(false); },
					() => {},
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

		//Tells the window to open, gets whether the Kerbal is male or female, handles figuring out whether a Kerbal with that name exists, etc.
		public void Update()
		{

            if (getRandomKerbal)
            {
                newKerbal = HighLogic.CurrentGame.CrewRoster.GetNewKerbal();
                if (!HighLogic.CurrentGame.CrewRoster.Exists(newKerbal.name))
                {
                    stringToEdit = newKerbal.name;
                    getRandomKerbal = false;
                }
            }

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

			if (editMale) {

				editGender = "Male";

			} else {

				editGender = "Female";

			}

			if (editTraitInt == 0) {

				editTrait = "Pilot";

			} else if (editTraitInt == 1) {

				editTrait = "Engineer";

			} else if (editTraitInt == 2) {

				editTrait = "Scientist";

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

			if (HighLogic.CurrentGame.CrewRoster.Exists(stringToEdit))
            {
                exists = true;
            } else
            {
                exists = false;
            }

			if (HighLogic.CurrentGame.CrewRoster.Exists(editName) && editName != kerbalToEdit.name) {

				editExists = true;

			} else {

				editExists = false;

			}

			if (windowState == false) {

				KrakenEnabled = false;
				editorEnabled = false;

			}
				
			if (krakenFloat == false) {

				krakenRect.x = windowPosition.x - windowPosition.width / 2 - 15;
				krakenRect.y = windowPosition.y;

			}

			if (editFloat == false) {

				editorRect.x = windowPosition.x + windowPosition.width;
				editorRect.y = windowPosition.y;

			}

		}

		//Gets a new kerbal and sets his/her stats based on whether there is already a Kerbal of that name or not, or if the person wants o override that check
		private void SpawnKerbal(int count, bool overrideState)
		{

			if (exists && overrideState == false) {

				warningState = true;

			} else {

                newKerbal = HighLogic.CurrentGame.CrewRoster.GetNewKerbal ();
                newKerbal.ChangeName(stringToEdit);
                newKerbal.courage = sliderValue;
                newKerbal.stupidity = sliderValue2;
                newKerbal.isBadass = buttonState;
                newKerbal.veteran = veteran;

				KerbalRoster.SetExperienceTrait (newKerbal, Trait);

				//Find out and set the gender
				if (male == false) {

                    newKerbal.gender = ProtoCrewMember.Gender.Female;

				}
				if (male) {

                    newKerbal.gender = ProtoCrewMember.Gender.Male;

				}

				if (closeOnComplete) {

					appLauncherButton.SetFalse (true);
					ScreenMessages.PostScreenMessage ("Kustom Kerbal Spawned, closing window...", 1, ScreenMessageStyle.UPPER_CENTER);

				} else {

					ScreenMessages.PostScreenMessage ("Kustom Kerbal Spawned.", 1, ScreenMessageStyle.UPPER_CENTER);

				}

			}

		}

		#region GUI

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

			if (editorEnabled) {

				editorRect = GUI.Window (1237, editorRect, editor, "Kerbal Editor");

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
            /*if (GUILayout.Button("Random"))
            {
                getRandomKerbal = true;
            }*/
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

            //Toggles Veteran state.
            GUILayout.BeginHorizontal();
            veteran = GUILayout.Toggle(veteran, "Veteran: " + veteran);
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
					if (closeOnComplete) {

						ScreenMessages.PostScreenMessage("Closing window...", 1, ScreenMessageStyle.UPPER_CENTER);
						windowState = false;

					}

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
			if (GUILayout.Button("Kerbal Editor")) {

				editorEnabled = !editorEnabled;

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
			KrakenScrollPosition = GUILayout.BeginScrollView (KrakenScrollPosition);
			List<ProtoCrewMember> kerbs = new List<ProtoCrewMember>(HighLogic.CurrentGame.CrewRoster.Crew);
			for (int i = kerbs.Count - 1; i >= 0; --i)
			{

				ProtoCrewMember kerb = kerbs[i];

				if (GUILayout.Button (kerb.name)) {

					if (KrakenArmed) {
						
						kerb.Die ();
						kerb.type = ProtoCrewMember.KerbalType.Unowned;

					} else {

						ScreenMessages.PostScreenMessage ("You must arm the Kraken before using it.", 1);

					}

				}

			}
			GUILayout.EndScrollView ();
			GUILayout.BeginVertical ();
			KrakenArmed = GUILayout.Toggle (KrakenArmed, "Armed: " + KrakenArmed.ToString());
			krakenFloat = GUILayout.Toggle (krakenFloat, "Float window");
			if (GUILayout.Button("Close")) {

				KrakenEnabled = false;

			}
			GUILayout.EndVertical ();

			if (krakenFloat) {

				GUI.DragWindow ();

			}

		}

		#endregion

		#region editor

		public void editor(int windowID) {

			editorScrollPosition = GUILayout.BeginScrollView (editorScrollPosition);
			List<ProtoCrewMember> kerbs = new List<ProtoCrewMember>(HighLogic.CurrentGame.CrewRoster.Crew);
			for (int i = kerbs.Count - 1; i >= 0; --i)
			{

				ProtoCrewMember kerb = kerbs[i];

				if (GUILayout.Button (kerb.name)) {

					kerbalToEdit = kerb;
					editCourage = kerb.courage;
					editStupidity = kerb.stupidity;
					editBadS = kerb.isBadass;
					editName = kerb.name;
					if (kerb.gender == ProtoCrewMember.Gender.Female) {

						editGender = "Female";
						editMale = false;

					} else {

						editGender = "Male";
						editMale = true;

					}
					editTrait = kerb.trait;
                    editVeteran = kerb.veteran;
					if (editTrait == "Pilot") {
						
						editTraitInt = 0;

					} else if (editTrait == "Engineer") {

						editTraitInt = 1;

					} else {

						editTraitInt = 2;

					}

				}

			}
			GUILayout.EndScrollView ();

			//Field to type in kerbal's name.
			GUILayout.BeginHorizontal();
			GUILayout.Label("Name:");
			editName = GUILayout.TextField(editName, 50);
			GUILayout.EndHorizontal();

			//Toggle female/male
			GUILayout.BeginHorizontal();
			if (GUILayout.Button (editGender)) {

				editMale = !editMale;

			}
			if (GUILayout.Button (editTrait)) {

				if (editTraitInt < 2) {

					editTraitInt += 1;

				} else {

					editTraitInt = 0;

				}

			}

			GUILayout.EndHorizontal ();

			//Sets kerbal's courage. 
			GUILayout.BeginHorizontal();
			GUILayout.Label("Courage:");
			editCourage = GUILayout.HorizontalSlider(editCourage, 0.0f, 1.0f);
			GUILayout.EndHorizontal();

			//Sets kerbal's stupidity.
			GUILayout.BeginHorizontal();
			GUILayout.Label("Stupidity:");
			editStupidity = GUILayout.HorizontalSlider(editStupidity, 0.0f, 1.0f);
			GUILayout.EndHorizontal();

			//Toggles BadS state.
			GUILayout.BeginHorizontal();
			editBadS = GUILayout.Toggle(editBadS, "BadS: " + editBadS);
			GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            editVeteran = GUILayout.Toggle(editVeteran, "Veteran: " + editVeteran);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal ();
			editFloat = GUILayout.Toggle (editFloat, "Float window");
			GUILayout.EndHorizontal ();

			if (editExists) {

				GUILayout.BeginHorizontal ();
				editOverride = GUILayout.Toggle (editOverride, "Override existing Kerbal check");
				GUILayout.EndHorizontal ();

			}

			if (GUILayout.Button ("Edit Kerbal")) {

				if (kerbalToEdit == null) {

					ScreenMessages.PostScreenMessage ("You must select a Kerbal first!", 1);

				} else {

					if (editExists) {

						if (editOverride) {
                            
                            kerbalToEdit.ChangeName(editName);
							if (editGender == "Female") {

								kerbalToEdit.gender = ProtoCrewMember.Gender.Female;

							} else {

								kerbalToEdit.gender = ProtoCrewMember.Gender.Male;

							}

							KerbalRoster.SetExperienceTrait (kerbalToEdit, editTrait);

							kerbalToEdit.courage = editCourage;
							kerbalToEdit.stupidity = editStupidity;

							kerbalToEdit.isBadass = editBadS;
                            kerbalToEdit.veteran = editVeteran;

							ScreenMessages.PostScreenMessage (kerbalToEdit.name + " has been edited!", 1, ScreenMessageStyle.UPPER_CENTER);

						} else {

							ScreenMessages.PostScreenMessage ("A Kerbal with the name " + kerbalToEdit.name + " already exists.  If you would like to continue, enable override existing Kerbal ckeck.", 1, ScreenMessageStyle.UPPER_CENTER);

						}

					} else {
						
                        kerbalToEdit.ChangeName(editName);
						if (editGender == "Female") {
					
							kerbalToEdit.gender = ProtoCrewMember.Gender.Female;
						
						} else {

							kerbalToEdit.gender = ProtoCrewMember.Gender.Male;

						}

						KerbalRoster.SetExperienceTrait (kerbalToEdit, editTrait);

						kerbalToEdit.courage = editCourage;
						kerbalToEdit.stupidity = editStupidity;

						kerbalToEdit.isBadass = editBadS;

						ScreenMessages.PostScreenMessage (kerbalToEdit.name + " has been edited!", 1, ScreenMessageStyle.UPPER_CENTER);
					
					}

				}

			}

			if (GUILayout.Button ("Close")) {

				editorEnabled = false;

			}

			if (editFloat) {

				GUI.DragWindow ();

			}

		}

		#endregion

	}
}