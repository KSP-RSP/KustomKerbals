using System.Collections.Generic;
using UnityEngine;

namespace KustomKerbals.Extensions
{
    [KSPAddon(KSPAddon.Startup.EveryScene, false)]
    public class KKToolbar : MonoBehaviour
    {
        private static Rect windowPosition = new Rect(0, 0, 180, 135);
        private static GUIStyle windowStyle = null;
        public static bool isTBAvailable = false;
        public static bool Mainmenu = false;
        private IButton KKbutton;

        public void Awake()
        {
            RenderingManager.AddToPostDrawQueue(0, OnDraw);
            if (ToolbarManager.ToolbarAvailable)
            {
                isTBAvailable = true;
            }
            else if (!ToolbarManager.ToolbarAvailable)
            {
                isTBAvailable = false;
            }
        }

        public void Start()
        {
            //Sets window style to KSP default.
            windowStyle = new GUIStyle(HighLogic.Skin.window);
        }

        private void OnDraw()
        {
            //Checks if player has toggled the window on or off.
            if (Mainmenu == true)
            {
                windowPosition = GUI.Window(1234, windowPosition, OnWindow, "Kustom Kerbals Menu", windowStyle);

                //Resets window position to middle of screen.
                if (windowPosition.x == 0f && windowPosition.y == 0f)
                {
                    windowPosition = windowPosition.CenterScreen();
                }
            }
        }

        public void Update()
        {
            if ((GameSettings.MODIFIER_KEY.GetKey()) && Input.GetKeyDown(KeyCode.K))
            {
                Mainmenu = true;
                if (!KK.isKSCLocked)
                {
                    InputLockManager.SetControlLock(ControlTypes.KSC_FACILITIES, "KSCLock");
                    KK.isKSCLocked = true;
                }
                Debug.Log("KK window opened");
            }
        }

        private void OnWindow(int WindowID)
        {
            if (GUI.Button(new Rect(14, 27, 150, 30), "Open Kustomizer"))
            {
                Mainmenu = false;
                KK.windowState = true;
                ScreenMessages.PostScreenMessage("Opening Kerbal Kusomizer...", 1, ScreenMessageStyle.UPPER_CENTER);
                Debug.Log("Creator opened");
            }
            if (GUI.Button(new Rect(14, 64, 150, 30), "Open Editor"))
            {
                Mainmenu = false;
                ScreenMessages.PostScreenMessage("Not Yet Implemented!", 1, ScreenMessageStyle.UPPER_CENTER);
                Debug.Log("Editor opened");
            }
            if (GUI.Button(new Rect(14, 101, 150, 30), "Feed the Kraken"))
            {
                Mainmenu = false;
                ScreenMessages.PostScreenMessage("Not Yet Implemented!", 1, ScreenMessageStyle.UPPER_CENTER);
                Debug.Log("Deleter opened");
            }
            GUI.DragWindow();
        }

        internal KKToolbar()
        {
            if (isTBAvailable == true)
            {
                KKbutton = ToolbarManager.Instance.add("test", "KKbutton");
                KKbutton.TexturePath = "000_Toolbar/img_buttonTypeMNode";
                KKbutton.ToolTip = "Open Kustom Kerbals Menu";
                KKbutton.OnClick += (e) => Mainmenu = true; InputLockManager.SetControlLock(ControlTypes.KSC_FACILITIES, "KSCLock"); KK.isKSCLocked = true;
            }
        }

        internal void OnDestroy()
        {
            KKbutton.Destroy();
        }
    }
}
