// Copyright (c) 2015 - 2016 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using UnityEngine;
using System.Collections.Generic;

namespace DoozyUI
{
    public class UIButtonMessage : Message
    {
        public string buttonName;

        public bool addToNavigationHistory;
        public bool backButton;

        public List<string> showElements;
        public List<string> hideElements;
        public List<string> gameEvents;

        public GameObject gameObject;
    }
}

