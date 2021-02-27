// Copyright (c) 2015 - 2016 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

#if dUI_PlayMaker
using UnityEngine;
using HutongGames.PlayMaker;
using DoozyUI;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("DoozyUI")]
    [Tooltip("DoozyUI - Simulates a button click")]
    public class SendUIButtonMessage : FsmStateAction
    {
        #region Variables
        [RequiredField]
        public FsmString buttonName;
        public bool debugThis = false;
        #endregion

        public override void Reset()
        {
            buttonName = new FsmString { UseVariable = false };
        }

        public override void OnEnter()
        {
            //UIButtonMessage m = new UIButtonMessage();
            //m.buttonName = buttonName.Value;
            //Message.Send<UIButtonMessage>(m);
            UIManager.SendButtonClick(buttonName.Value);

            if (debugThis)
                Debug.Log("[DoozyUI] - Playmaker - State Name [" + State.Name + "] - Simulated button click event - [" + buttonName + "]");

            Finish();
        }
    }
}
#endif