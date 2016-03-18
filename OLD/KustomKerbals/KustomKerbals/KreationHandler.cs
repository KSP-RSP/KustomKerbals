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
using KustomKerbals.Extensions;
using UnityEngine;

namespace KustomKerbals
{

    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    public class KK : MonoBehaviour
    {
        private static Rect windowPosition = new Rect(0, 0, 370, 240);
        private static GUIStyle windowStyle = null;
        private static bool buttonState = false;
		private static bool maleState = true;
		private static bool femaleState = false;
		private static bool scientistState = false;
		private static bool engineerState = false;
		private static bool pilotState = true;
        public static bool windowState = false;
        public static bool isKSCLocked = false;
        public string stringToEdit = "Jebediah Kerman Jr.";
		public string traitString = "(Pilot, Engineer, Scientist)";
        public float sliderValue = 0.0f;
        public float sliderValue2 = 0.0f;
        public int kerbal { get; set; }
        public static string Path = (KSPUtil.ApplicationRootPath + HighLogic.CurrentGame + "peristent.sfs");

        public void Awake()
        {
            RenderingManager.AddToPostDrawQueue(0, OnDraw);
        }

        public void Start()
        {
            //Sets window style to KSP default.
            windowStyle = new GUIStyle(HighLogic.Skin.window);
            Debug.Log("Kustom Kerbals loaded.");
			Debug.Log("And so begins our space adventure of cryptographic messages...");
			Debug.Log("If you can decode them, go to the Kustom Kerbals forum thread, and say what message you have decoded...");

        }

        public void OnDraw()
        {

            //Checks if player has toggled the window on or off.
            if (windowState == true)
            {
                windowPosition = GUI.Window(1234, windowPosition, OnWindow, "Kustom Kerbals", windowStyle);

                //Resets window position to middle of screen.
                if (windowPosition.x == 0f && windowPosition.y == 0f)
                {
                    windowPosition = windowPosition.CenterScreen();
					Debug.Log("First Message: 00001b400048785b002820d500d0e363");
                }
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
			if (femaleState == true) {

				kerbal.gender = ProtoCrewMember.Gender.Female;

			}
			if (maleState == true) {
				
				kerbal.gender = ProtoCrewMember.Gender.Male;
				Debug.Log("Second message: 00f82a4a0022df530066e8cb00fceaae");
				Debug.Log("I know it isn't in order, but that's your fault by not creating a male until now.");

			}

			//Figure out whether the kerbal is a pilot, scientist, or engineer
			if (pilotState == true) {

				KerbalRoster.SetExperienceTrait(kerbal, "Pilot");

			}
			if (scientistState == true) {

				KerbalRoster.SetExperienceTrait(kerbal, "Scientist");

			}
			if (engineerState == true) {

				KerbalRoster.SetExperienceTrait(kerbal, "Engineer");

			}

			//How to set trait
			//				KerbalRoster.SetExperienceTrait(kerbal, "Pilot");
			//				KerbalRoster.SetExperienceTrait(kerbal, "Engineer");
			//				KerbalRoster.SetExperienceTrait(kerbal, "Scientist");
        }
		//Gets a new kerbal and sets his/her stats.
		private void SpawnBadassKerbal(int count)
		{
			ProtoCrewMember kerbal = HighLogic.CurrentGame.CrewRoster.GetNewKerbal();
			kerbal.name = stringToEdit;
			kerbal.courage = sliderValue;
			kerbal.stupidity = sliderValue2;
			kerbal.isBadass = true;

			//Figure out whether the kerbal is a pilot, scientist, or engineer
			if (pilotState == true) {

				KerbalRoster.SetExperienceTrait(kerbal, "Pilot");

			}
			if (scientistState == true) {

				KerbalRoster.SetExperienceTrait(kerbal, "Scientist");

			}
			if (engineerState == true) {

				KerbalRoster.SetExperienceTrait(kerbal, "Engineer");

			}

			//How to set trait
//				KerbalRoster.SetExperienceTrait(kerbal, "Pilot");
//				KerbalRoster.SetExperienceTrait(kerbal, "Engineer");
//				KerbalRoster.SetExperienceTrait(kerbal, "Scientist");


			if (femaleState == true) {

				kerbal.gender = ProtoCrewMember.Gender.Female;

			}
			if (maleState == true) {

				kerbal.gender = ProtoCrewMember.Gender.Male;

			}


		}
		private void SpawnMarkKerbal(int count)
		{
			ProtoCrewMember kerbal = HighLogic.CurrentGame.CrewRoster.GetNewKerbal();
			kerbal.name = "Mark Watney";
			kerbal.courage = sliderValue;
			kerbal.stupidity = sliderValue2;
			kerbal.isBadass = true;
			kerbal.gender = ProtoCrewMember.Gender.Male;
			KerbalRoster.SetExperienceTrait(kerbal, "Scientist");
			//Mark Watney is also an engineer, but this way he can science the shit out of this...

		}

        //makes the new kerbal available for use.
        private void SpawnKerbal(ProtoCrewMember kerbal)
        {
            //kerbal.rosterStatus = ProtoCrewMember.RosterStatus.Available;
			kerbal.type = ProtoCrewMember.KerbalType.Applicant;

        }



        //Tells the window to open.
        public void Update()
        {

			if (femaleState == true) {

				maleState = false;

			}
			if (maleState == true) {

				femaleState = false;

			}
			//Seems redundant, but is necessary if you deselect both
			if (femaleState == false) {
				
				maleState = true;
				
			}
			if (maleState == false) {
				
				femaleState = true;
				
			}

			if (traitString == "pilot") {

				pilotState = true;
				engineerState = false;
				scientistState = false;

			} else if (traitString == "scientist") {

				pilotState = false;
				engineerState = false;
				scientistState = true;

			} else if (traitString == "engineer") {

				pilotState = false;
				engineerState = true;
				scientistState = false;

			} 
			else {



			}
				



			if ((GameSettings.MODIFIER_KEY.GetKey()) && Input.GetKeyDown(KeyCode.K))
			{
				windowState = true;
				if (!KK.isKSCLocked)
				{
					InputLockManager.SetControlLock(ControlTypes.KSC_FACILITIES, "KSCLock");
					KK.isKSCLocked = true;
				}
				Debug.Log("KK window opened");
				Debug.Log ("Third message: 00a6915f404d32f30000006800ac8e28");
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
			GUILayout.Label("Female:");
			femaleState = GUILayout.Toggle(femaleState, "Female: " + femaleState);
			GUILayout.Label("Male:");
			maleState = GUILayout.Toggle(maleState, "Male: " + maleState);
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

            //Toggles badass state.
            GUILayout.BeginHorizontal();
            buttonState = GUILayout.Toggle(buttonState, "Badass: " + buttonState);
            GUILayout.EndHorizontal();

			//Sets whether the kerbal is a scientist engineer, or pilot
			GUILayout.BeginHorizontal();
			GUILayout.Label("Trait:");
			traitString = GUILayout.TextField(traitString, 50);
			GUILayout.EndHorizontal();

            //Button to create the kerbal using above paramaters.
            if (GUI.Button(new Rect(7, 190, 220, 35), "Kreate Kustom Kerbal"))
            {
                if (isKSCLocked)
                {
                    InputLockManager.RemoveControlLock("KSCLock");
                    isKSCLocked = false;
                }
				if (stringToEdit == "Jebediah Kerman Jr." && sliderValue == 0.0f && sliderValue2 == 0.0f && buttonState == false && femaleState == false)
                {
                    ProtoCrewMember kerbal = HighLogic.CurrentGame.CrewRoster.GetNewKerbal();
                    kerbal.rosterStatus = ProtoCrewMember.RosterStatus.Available;
                    windowState = false;
                    ScreenMessages.PostScreenMessage("Random Kerbal Spawned, closing window...", 1, ScreenMessageStyle.UPPER_CENTER);
					Debug.Log("Random Kerbal Spawned");
                }
				else if (stringToEdit == "Badass Kerman") {

					Debug.Log("Badass Kerbal Kreated.");
					Debug.Log("KK window closed.");
					SpawnBadassKerbal(kerbal);
					windowState = false;
					ScreenMessages.PostScreenMessage("Badass Kerbal Spawned, closing window...", 1, ScreenMessageStyle.UPPER_CENTER);

				}
				else if (stringToEdit == "badass kerman") {

					Debug.Log("Badass Kerbal Kreated.");
					Debug.Log("KK window closed.");
					SpawnBadassKerbal(kerbal);
					windowState = false;
					ScreenMessages.PostScreenMessage("Badass Kerbal Spawned, closing window...", 1, ScreenMessageStyle.UPPER_CENTER);

				}
				else if (stringToEdit == "Badass") {

					Debug.Log("Badass Kerbal Kreated.");
					Debug.Log("KK window closed.");
					SpawnBadassKerbal(kerbal);
					windowState = false;
					ScreenMessages.PostScreenMessage("Badass Kerbal Spawned, closing window...", 1, ScreenMessageStyle.UPPER_CENTER);

				}
				else if (stringToEdit == "badass") {

					Debug.Log("Badass Kerbal Kreated.");
					Debug.Log("KK window closed.");
					SpawnBadassKerbal(kerbal);
					windowState = false;
					ScreenMessages.PostScreenMessage("Badass Kerbal Spawned, closing window...", 1, ScreenMessageStyle.UPPER_CENTER);

				}
				else if (stringToEdit == "Duna") {

					Debug.Log("Mark Watney Kerbal Kreated.");
					Debug.Log("KK window closed.");
					SpawnMarkKerbal(kerbal);
					windowState = false;
					ScreenMessages.PostScreenMessage("Mark Watney Kerbal Spawned, let's science the shit out of this...", 1, ScreenMessageStyle.UPPER_CENTER);
					Debug.Log ("Fourth message: 00004bef00000d41087000b8000045c2");
					Debug.Log ("This one is heavily salted.");
				}
                else
                {
                    Debug.Log("Kustom Kerbal Kreated.");
                    Debug.Log("KK window closed.");
                    SpawnKerbal(kerbal);
                    windowState = false;
                    ScreenMessages.PostScreenMessage("Kustom Kerbal Spawned, closing window...", 1, ScreenMessageStyle.UPPER_CENTER);
					traitString.ToLower ();


				}
            }

            GUI.DragWindow();
        }
    }
}