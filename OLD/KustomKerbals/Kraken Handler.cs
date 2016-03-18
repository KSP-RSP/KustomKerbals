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
	
	[KSPAddon(KSPAddon.Startup.Flight, false)]
	public class ModuleKraken : PartModule
	{
		private static Rect windowPosition = new Rect(0, 0, 370, 240);
		private static GUIStyle windowStyle = null;
		public static bool windowState = false;
		public static bool isKSCLocked = false;
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
				}
			}
		}
		
		//Gets a new kerbal and sets his/her stats.

		//Gets a new kerbal and sets his/her stats.
		private void SpawnBadassKerbal(int count)
		{
					//KerbalRoster.SetExperienceTrait(kerbal, "Scientist");
			
			

			
			
		}

		
		//makes the new kerbal available for use.
		private void SpawnKerbal(ProtoCrewMember kerbal)
		{
			//kerbal.rosterStatus = ProtoCrewMember.RosterStatus.Available;
			//kerbal.type = ProtoCrewMember.KerbalType.Applicant;
			
		}
		
		
		
		//Tells the window to open.
		public void Update()
		{

			
			
			

			if ((GameSettings.MODIFIER_KEY.GetKey()) && Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.K))
			{
				windowState = true;
				if (!KK.isKSCLocked)
				{
					InputLockManager.SetControlLock(ControlTypes.All, "KSCLock");
					KK.isKSCLocked = true;
				}
				Debug.Log("Kraken window opened");
			}
		}

		private void FeedToKraken(){



		}
		
		private void OnWindow(int windowID)
		{

			
			//Button to create the kerbal using above paramaters.
			if (GUI.Button(new Rect(7, 190, 220, 35), "Feed Kerbal To The Kraken"))
			{
				if (isKSCLocked)
				{
					InputLockManager.RemoveControlLock("KSCLock");
					isKSCLocked = false;
				}



				part.explode();
			}
			
			GUI.DragWindow();
		}
	}
}