// Copyright (c) 2015 - 2016 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

#if dUI_PlayMaker
using UnityEngine;
using HutongGames.PlayMaker;
using System.Collections;
using DoozyUI;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("DoozyUI")]
    [Tooltip("DoozyUI - Show/Hide UIElements")]
    public class ShowHideElements : FsmStateAction
    {
        #region Variables
        public string[] showElements;
        public string[] hideElements;
        public bool debugThis = false;
        #endregion

        public override void OnEnter()
        {

            if (showElements != null && showElements.Length > 0)
            {
                for (int i = 0; i < showElements.Length; i++)
                {
                    UIManager.ShowUiElement(showElements[i]);

                    if (debugThis)
                        Debug.Log("[DoozyUI] - Playmaker - State Name [" + State.Name + "] - Show^" + showElements[i]);
                }
            }

            if (hideElements != null && hideElements.Length > 0)
            {
                for (int i = 0; i < hideElements.Length; i++)
                {
                    UIManager.HideUiElement(hideElements[i]);

                    if (debugThis)
                        Debug.Log("[DoozyUI] - Playmaker - State Name [" + State.Name + "] - Hide^" + hideElements[i]);
                }
            }

            Finish();
        }
    }
}
#endif
