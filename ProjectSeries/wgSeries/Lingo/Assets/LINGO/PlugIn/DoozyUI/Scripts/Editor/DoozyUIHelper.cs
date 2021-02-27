// Copyright (c) 2015 - 2016 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using UnityEngine;
using UnityEditor;

public static class DoozyUIHelper
{
    #region Enums
    public enum DoozyColor
    {
        Doozy,
        Move,
        Rotate,
        Scale,
        Fade,
        DarkBlue,
        DarkGreen,
        DarkOrange,
        DarkRed,
        DarkPurple,
        LightBlue,
        LightGreen,
        LightOranage,
        LightRed,
        LightPurple,
        Black,
        White,
        LightGrey,
        DarkGrey,
        Blue,
        Green,
        Orange,
        Red,
        Purple
    }
    #endregion

    #region Variables

    #region COLORS
    //Doozy Colors
    private static readonly Color doozyLightBlue = new Color(0.66f, 0.86f, 0.95f);
    //private static readonly Color doozyBlue = new Color(0.16f, 0.65f, 0.87f);
    private static readonly Color doozyDarkBlue = new Color(0.08f, 0.33f, 0.44f);
    //Green
    private static readonly Color greenLight = new Color(0.79f, 0.91f, 0.71f);
    //private static readonly Color green = new Color(0.48f, 0.79f, 0.26f);
    private static readonly Color greenDark = new Color(0.24f, 0.4f, 0.13f);
    //Orange
    private static readonly Color orangeLight = new Color(1f, 0.83f, 0.65f);
    //private static readonly Color orange = new Color(1f, 0.58f, 0.12f);
    private static readonly Color orangeDark = new Color(0.5f, 0.29f, 0.06f);
    //Red
    private static readonly Color redLight = new Color(0.97f, 0.65f, 0.66f);
    //private static readonly Color red = new Color(0.92f, 0.11f, 0.15f);
    private static readonly Color redDark = new Color(0.46f, 0.06f, 0.07f);
    //Purple
    private static readonly Color purpleLight = new Color(0.87f, 0.74f, 0.82f);
    //private static readonly Color purple = new Color(0.68f, 0.35f, 0.54f);
    private static readonly Color purpleDark = new Color(0.34f, 0.18f, 0.27f);
    //General Colors
    private static readonly Color black = new Color(0f, 0f, 0f);
    private static readonly Color greyDark = new Color(0.3f, 0.3f, 0.3f);
    private static readonly Color greyLight = new Color(0.7f, 0.7f, 0.7f);
    private static readonly Color white = new Color(1f, 1, 1f);
    //Main Colors
    private static readonly Color blue = new Color(0.16f, 0.65f, 0.87f);
    private static readonly Color green = new Color(0.48f, 0.79f, 0.26f);
    private static readonly Color orange = new Color(1f, 0.58f, 0.12f);
    private static readonly Color red = new Color(0.92f, 0.11f, 0.15f);
    private static readonly Color purple = new Color(0.68f, 0.35f, 0.54f);
    #endregion

    #region STRINGS
    //Strings
    private const string AlertTitle = "Doozy UI Alert";
    private const string AlertOkText = "Ok";
    private const string FoldOutTooltip = "Click to expand or collapse";
    #endregion

    #endregion

    #region Colors - SetZoneColor, GetColor, ResetColors
    public static void SetZoneColor(DoozyColor color)
    {
        GUI.backgroundColor = GetColor(color);
    }

    public static Color GetColor(DoozyColor color)
    {
        Color c = doozyLightBlue;
        switch (color)
        {
            case DoozyColor.Doozy: c = doozyLightBlue; break;

            case DoozyColor.Move: c = greenLight; break;
            case DoozyColor.Rotate: c = orangeLight; break;
            case DoozyColor.Scale: c = redLight; break;
            case DoozyColor.Fade: c = purpleLight; break;

            case DoozyColor.DarkBlue: c = doozyDarkBlue; break;
            case DoozyColor.DarkGreen: c = greenDark; break;
            case DoozyColor.DarkOrange: c = orangeDark; break;
            case DoozyColor.DarkRed: c = redDark; break;
            case DoozyColor.DarkPurple: c = purpleDark; break;

            case DoozyColor.LightBlue: c = doozyLightBlue; break;
            case DoozyColor.LightGreen: c = greenLight; break;
            case DoozyColor.LightOranage: c = orangeLight; break;
            case DoozyColor.LightRed: c = redLight; break;
            case DoozyColor.LightPurple: c = purpleLight; break;

            case DoozyColor.Black: c = black; break;
            case DoozyColor.DarkGrey: c = greyDark; break;
            case DoozyColor.LightGrey: c = greyLight; break;
            case DoozyColor.White: c = white; break;

            case DoozyColor.Blue: c = blue; break;
            case DoozyColor.Green: c = green; break;
            case DoozyColor.Orange: c = orange; break;
            case DoozyColor.Red: c = red; break;
            case DoozyColor.Purple: c = purple; break;
        }
        return c;
    }

    public static void ResetColors()
    {
        GUI.color = Color.white;
        GUI.contentColor = Color.white;
        GUI.backgroundColor = Color.white;
    }
    #endregion

    #region CreateTextStyle
    public static GUIStyle CreateTextStyle(DoozyColor color, TextAnchor textAlignment)
    {
        GUIStyle style = new GUIStyle
        {
            normal =
            {
                textColor = GetColor(color)

            },
            alignment = textAlignment
        };
        return style;
    }

    public static GUIStyle CreateTextStyle(DoozyColor color, TextAnchor textAlignment, int fontSize)
    {
        GUIStyle style = new GUIStyle
        {
            normal =
            {
                textColor = GetColor(color)

            },
            alignment = textAlignment,
            fontSize = fontSize
        };
        return style;
    }

    public static GUIStyle CreateTextStyle(DoozyColor color, FontStyle fontStyle)
    {
        GUIStyle style = new GUIStyle
        {
            normal =
            {
                textColor = GetColor(color)

            },
            fontStyle = fontStyle
        };
        return style;
    }

    public static GUIStyle CreateTextStyle(DoozyColor color, FontStyle fontStyle, int fontSize)
    {
        GUIStyle style = new GUIStyle
        {
            normal =
            {
                textColor = GetColor(color)

            },
            fontSize = fontSize,
            fontStyle = fontStyle
        };
        return style;
    }

    public static GUIStyle CreateTextStyle(DoozyColor color, TextAnchor textAlignment, FontStyle fontStyle)
    {
        GUIStyle style = new GUIStyle
        {
            normal =
            {
                textColor = GetColor(color)

            },
            alignment = textAlignment,
            fontStyle = fontStyle
        };
        return style;
    }

    public static GUIStyle CreateTextStyle(DoozyColor color, TextAnchor textAlignment, FontStyle fontStyle, int fontSize)
    {
        GUIStyle style = new GUIStyle
        {
            normal =
            {
                textColor = GetColor(color)

            },
            alignment = textAlignment,
            fontSize = fontSize,
            fontStyle = fontStyle
        };
        return style;
    }

    public static GUIStyle CreateTextStyle(DoozyColor color, TextAnchor textAlignment, FontStyle fontStyle, int fontSize, float fixedHeight, float fixedWidth)
    {
        GUIStyle style = new GUIStyle
        {
            normal =
            {
                textColor = GetColor(color)

            },
            alignment = textAlignment,
            fontSize = fontSize,
            fontStyle = fontStyle,
            fixedHeight = fixedHeight,
            fixedWidth = fixedWidth
        };
        return style;
    }
    #endregion

    #region Check for DarkSkin
    private static bool IsDarkSkin
    {
        get
        {
            return EditorPrefs.GetInt("UserSkin") == 1;
        }
    }
    #endregion

    #region Spaces - VerticalSpace, HorizontalSpace
    public static void VerticalSpace(int pixels)
    {
        EditorGUILayout.BeginVertical();
        GUILayout.Space(pixels);
        EditorGUILayout.EndVertical();
    }

    public static void HorizontalSpace(int pixels)
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(pixels);
        EditorGUILayout.EndHorizontal();
    }
    #endregion

    #region Prefab Utilities - GetPrefabType, IsPrefabInProjectView
    private static PrefabType GetPrefabType(Object gObject)
    {
        return PrefabUtility.GetPrefabType(gObject);
    }

    public static bool IsPrefabInProjectView(Object gObject)
    {
        return GetPrefabType(gObject) == PrefabType.Prefab;
    }
    #endregion

    #region Foldout
    public static bool Foldout(bool expanded, string label)
    {
        var content = new GUIContent(label, FoldOutTooltip);
        expanded = EditorGUILayout.Foldout(expanded, content);

        return expanded;
    }
    #endregion

    #region Warnings - ShowColorWarningm ShowRedError, ShowLargeBarAlert, ShowAlert
    public static void ShowColorWarning(string warningText)
    {
        EditorGUILayout.HelpBox(warningText, MessageType.Info);
    }

    public static void ShowRedError(string errorText)
    {
        EditorGUILayout.HelpBox(errorText, MessageType.Error);
    }

    public static void ShowLargeBarAlert(string errorText)
    {
        EditorGUILayout.HelpBox(errorText, MessageType.Warning);
    }

    public static void ShowAlert(string text)
    {
        if (Application.isPlaying)
        {
            Debug.LogWarning(text);
        }
        else
        {
            EditorUtility.DisplayDialog(AlertTitle, text,
                                        AlertOkText);
        }
    }
    #endregion

    #region DrawTexture
    public static void DrawTexture(Texture tex)
    {
        if (tex == null)
        {
            Debug.Log("Texture is null");
            return;
        }
        
        //tex.hideFlags = HideFlags.DontSave;
        var rect = GUILayoutUtility.GetRect(0f, 0f);
        rect.width = tex.width;
        rect.height = tex.height;
        GUILayout.Space(rect.height);
        GUI.DrawTexture(rect, tex);
    }

    public static void DrawTexture(Texture tex, float width, float height)
    {
        var rect = GUILayoutUtility.GetRect(0f, 0f);
        rect.width = width;
        rect.height = height;
        GUILayout.Space(rect.height);
        GUI.DrawTexture(rect, tex);
    }
    #endregion
}
