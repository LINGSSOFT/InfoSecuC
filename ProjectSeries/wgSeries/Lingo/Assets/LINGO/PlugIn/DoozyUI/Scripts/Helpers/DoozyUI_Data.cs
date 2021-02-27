// Copyright (c) 2015 - 2016 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using UnityEngine;
using System.Collections.Generic;
using DoozyUI;

[CreateAssetMenu(fileName = "DoozyUI_Data", menuName = "DoozyUI/Data", order = 1)]
public class DoozyUI_Data : ScriptableObject
{
    public List<UIElement.ElementName> elementNames;
    public List<UIAnimator.SoundDetails> elementSounds;

    public List<DoozyUI.UIButton.ButtonName> buttonNames;
    public List<DoozyUI.UIButton.ButtonSound> buttonSounds;
}
