//Copyright (c) 2014, Blake Meyler.
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
        private static Rect windowPosition = new Rect(0, 0, 250, 160);
        private static GUIStyle windowStyle = null;
        private static bool buttonState = false;
        public static bool windowState = true;
        public static bool isKSCLocked = false;
        public string stringToEdit = "Jebediah Kerman Jr.";
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
        }

        private void OnDraw()
        {
            //Checks if player has toggled the window on or off.
            if (windowState == true)
            {
                windowPosition = GUI.Window(1234, windowPosition, OnWindow, "Kustom Kerbals", windowStyle);

                //Resets window position to middle of screen.
                if (windowPosition.x == 0f && windowPosition.y == 0f)
                {
                    //windowPosition = windowPosition.CenterScreen();
                }
            }
        }

        //Gets a new kerbal and sets his stats.
        private void SpawnKerbal(int count)
        {
            ProtoCrewMember kerbal = HighLogic.CurrentGame.CrewRoster.GetNewKerbal();
            kerbal.name = stringToEdit;
            kerbal.courage = sliderValue;
            kerbal.stupidity = sliderValue2;
            kerbal.isBadass = buttonState;
        }

        //makes the new kerbal available for use.
        private void SpawnKerbal(ProtoCrewMember kerbal)
        {
            kerbal.rosterStatus = ProtoCrewMember.RosterStatus.Available;
        }

        //Tells the window to open.
        public void Update()
        {

        }

        private void OnWindow(int windowID)
        {
            //Field to type in kerbal's name.
            GUILayout.BeginHorizontal();
            GUILayout.Label("Name:");
            stringToEdit = GUILayout.TextField(stringToEdit, 50);
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

            //Button to create the kerbal using above paramaters.
            if (GUI.Button(new Rect(7, 125, 150, 30), "Kreate Kustom Kerbal"))
            {
                if (isKSCLocked)
                {
                    InputLockManager.RemoveControlLock("KSCLock");
                    isKSCLocked = false;
                }
                if (stringToEdit == "Jebediah Kerman Jr." && sliderValue == 0.0f && sliderValue2 == 0.0f && buttonState == false)
                {
                    ProtoCrewMember kerbal = HighLogic.CurrentGame.CrewRoster.GetNewKerbal();
                    kerbal.rosterStatus = ProtoCrewMember.RosterStatus.Available;
                    windowState = false;
                    ScreenMessages.PostScreenMessage("Random Kerbal Spawned, closing window...", 1, ScreenMessageStyle.UPPER_CENTER);
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

            GUI.DragWindow();
        }
    }
}