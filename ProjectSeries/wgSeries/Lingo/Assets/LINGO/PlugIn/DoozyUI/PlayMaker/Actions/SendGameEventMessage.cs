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
    [Tooltip("DoozyUI - Sends a GameEventMessage")]
    public class SendGameEventMessage : FsmStateAction
    {
        #region Variables
        [RequiredField]
        public FsmString gameEvent;
        public bool debugThis = false;
        #endregion

        public override void Reset()
        {
            gameEvent = new FsmString { UseVariable = false };
        }

        public override void OnEnter()
        {
            //GameEventMessage m = new GameEventMessage();
            //m.command = gameEvent.Value;
            //Message.Send<GameEventMessage>(m);
            UIManager.SendGameEvent(gameEvent.Value);

            if (debugThis)
                Debug.Log("[DoozyUI] - Playmaker - State Name [" + State.Name + "] - Sent game event message - [" + gameEvent.Value + "]");

            Finish();
        }
    }
}
#endif