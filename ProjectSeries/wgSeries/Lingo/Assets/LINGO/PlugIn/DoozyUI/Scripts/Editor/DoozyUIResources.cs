// Copyright (c) 2015 - 2017 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using DoozyUI;

public static class DoozyUIResources
{
    private static string uIImagesFolderPath;
    public static string GetUIImagesFolderPath
    {
        get
        {
            if (string.IsNullOrEmpty(uIImagesFolderPath))
            {
                uIImagesFolderPath = DoozyUI.FileHelper.GetRelativeFolderPath("DoozyUI") + "/Images/";
            }
            return uIImagesFolderPath;
        }
    }

    public static Texture GetTexture(string fileName)
    {
        return AssetDatabase.LoadAssetAtPath<Texture>(GetUIImagesFolderPath + fileName + ".png");
    }

    #region Logos
    private static Texture mLogoDoozyUI;
    public static Texture LogoDoozyUI { get { if (mLogoDoozyUI == null) mLogoDoozyUI = GetTexture("inspector_logo_doozyUI") as Texture; return mLogoDoozyUI; } }

    private static Texture mLogoEBT;
    public static Texture LogoEBT { get { if (mLogoEBT == null) mLogoEBT = GetTexture("inspector_logo_ebt") as Texture; return mLogoEBT; } }
    #endregion

    #region Window - backgrounds
    private static Texture mWindowBackgroundLightGrey;
    public static Texture WindowBackgroundLightGrey { get { if (mWindowBackgroundLightGrey == null) mWindowBackgroundLightGrey = GetTexture("window_background_light_grey") as Texture; return mWindowBackgroundLightGrey; } }
    #endregion

    #region Window - headers
    private static Texture mWindowHeaderDoozy;
    public static Texture WindowHeaderDoozy { get { if (mWindowHeaderDoozy == null) mWindowHeaderDoozy = GetTexture("window_header_doozy") as Texture; return mWindowHeaderDoozy; } }

    private static Texture mWindowSubheaderControlPanel;
    public static Texture WindowSubheaderControlPanel { get { if (mWindowSubheaderControlPanel == null) mWindowSubheaderControlPanel = GetTexture("window_subheader_control_panel") as Texture; return mWindowSubheaderControlPanel; } }

    private static Texture mWindowSubheaderQuickHelp;
    public static Texture WindowSubheaderQuickHelp { get { if (mWindowSubheaderQuickHelp == null) mWindowSubheaderQuickHelp = GetTexture("window_subheader_quick_help") as Texture; return mWindowSubheaderQuickHelp; } }

    private static Texture mWindowSubheaderVideoTutorials;
    public static Texture WindowSubheaderVideoTutorials { get { if (mWindowSubheaderVideoTutorials == null) mWindowSubheaderVideoTutorials = GetTexture("window_subheader_video_tutorials") as Texture; return mWindowSubheaderVideoTutorials; } }

    private static Texture mWindowMiniheaderEditDatabase;
    public static Texture WindowMiniheaderEditDatabase { get { if (mWindowMiniheaderEditDatabase == null) mWindowMiniheaderEditDatabase = GetTexture("window_miniheader_edit_database") as Texture; return mWindowMiniheaderEditDatabase; } }

    private static Texture mWindowMiniheaderHelpAndTutorials;
    public static Texture WindowMiniheaderHelpAndTutorials { get { if (mWindowMiniheaderHelpAndTutorials == null) mWindowMiniheaderHelpAndTutorials = GetTexture("window_miniheader_help_and_tutorials") as Texture; return mWindowMiniheaderHelpAndTutorials; } }

    private static Texture mWindowSubheaderElementNames;
    public static Texture WindowSubheaderElementNames { get { if (mWindowSubheaderElementNames == null) mWindowSubheaderElementNames = GetTexture("window_subheader_element_names") as Texture; return mWindowSubheaderElementNames; } }

    private static Texture mWindowSubheaderElementSounds;
    public static Texture WindowSubheaderElementSounds { get { if (mWindowSubheaderElementSounds == null) mWindowSubheaderElementSounds = GetTexture("window_subheader_element_sounds") as Texture; return mWindowSubheaderElementSounds; } }

    private static Texture mWindowSubheaderButtonNames;
    public static Texture WindowSubheaderButtonNames { get { if (mWindowSubheaderButtonNames == null) mWindowSubheaderButtonNames = GetTexture("window_subheader_button_names") as Texture; return mWindowSubheaderButtonNames; } }

    private static Texture mWindowSubheaderButtonSounds;
    public static Texture WindowSubheaderButtonSounds { get { if (mWindowSubheaderButtonSounds == null) mWindowSubheaderButtonSounds = GetTexture("window_subheader_button_sounds") as Texture; return mWindowSubheaderButtonSounds; } }

    #endregion

    #region Window - buttons
    private static Texture mWindowButtonMAEnabled;
    public static Texture WindowButtonMAEnabled { get { if (mWindowButtonMAEnabled == null) mWindowButtonMAEnabled = GetTexture("window_button_ma_enabled") as Texture; return mWindowButtonMAEnabled; } }

    private static Texture mWindowButtonMADisabled;
    public static Texture WindowButtonMADisabled { get { if (mWindowButtonMADisabled == null) mWindowButtonMADisabled = GetTexture("window_button_ma_disabled") as Texture; return mWindowButtonMADisabled; } }

    private static Texture mWindowButtonTMPEnabled;
    public static Texture WindowButtonTMPEnabled { get { if (mWindowButtonTMPEnabled == null) mWindowButtonTMPEnabled = GetTexture("window_button_tmp_enabled") as Texture; return mWindowButtonTMPEnabled; } }

    private static Texture mWindowButtonTMPDisabled;
    public static Texture WindowButtonTMPDisabled { get { if (mWindowButtonTMPDisabled == null) mWindowButtonTMPDisabled = GetTexture("window_button_tmp_disabled") as Texture; return mWindowButtonTMPDisabled; } }

    private static Texture mWindowButtonEBTEnabled;
    public static Texture WindowButtonEBTEnabled { get { if (mWindowButtonEBTEnabled == null) mWindowButtonEBTEnabled = GetTexture("window_button_ebt_enabled") as Texture; return mWindowButtonEBTEnabled; } }

    private static Texture mWindowButtonEBTDisabled;
    public static Texture WindowButtonEBTDisabled { get { if (mWindowButtonEBTDisabled == null) mWindowButtonEBTDisabled = GetTexture("window_button_ebt_disabled") as Texture; return mWindowButtonEBTDisabled; } }

    private static Texture mWindowButtonPMEnabled;
    public static Texture WindowButtonPMEnabled { get { if (mWindowButtonPMEnabled == null) mWindowButtonPMEnabled = GetTexture("window_button_pm_enabled") as Texture; return mWindowButtonPMEnabled; } }

    private static Texture mWindowButtonPMDisabled;
    public static Texture WindowButtonPMDisabled { get { if (mWindowButtonPMDisabled == null) mWindowButtonPMDisabled = GetTexture("window_button_pm_disabled") as Texture; return mWindowButtonPMDisabled; } }

    private static Texture mWindowButtonNavEnabled;
    public static Texture WindowButtonNavEnabled { get { if (mWindowButtonNavEnabled == null) mWindowButtonNavEnabled = GetTexture("window_button_nav_enabled") as Texture; return mWindowButtonNavEnabled; } }

    private static Texture mWindowButtonNavDisabled;
    public static Texture WindowButtonNavDisabled { get { if (mWindowButtonNavDisabled == null) mWindowButtonNavDisabled = GetTexture("window_button_nav_disabled") as Texture; return mWindowButtonNavDisabled; } }

    private static Texture mWindowButtonOMEnabled;
    public static Texture WindowButtonOMEnabled { get { if (mWindowButtonOMEnabled == null) mWindowButtonOMEnabled = GetTexture("window_button_om_enabled") as Texture; return mWindowButtonOMEnabled; } }

    private static Texture mWindowButtonOMDisabled;
    public static Texture WindowButtonOMDisabled { get { if (mWindowButtonOMDisabled == null) mWindowButtonOMDisabled = GetTexture("window_button_om_disabled") as Texture; return mWindowButtonOMDisabled; } }

    private static Texture mWindowButtonElementNames;
    public static Texture WindowButtonElementNames { get { if (mWindowButtonElementNames == null) mWindowButtonElementNames = GetTexture("window_button_element_names") as Texture; return mWindowButtonElementNames; } }

    private static Texture mWindowButtonElementSounds;
    public static Texture WindowButtonElementSounds { get { if (mWindowButtonElementSounds == null) mWindowButtonElementSounds = GetTexture("window_button_element_sounds") as Texture; return mWindowButtonElementSounds; } }

    private static Texture mWindowButtonButtonNames;
    public static Texture WindowButtonButtonNames { get { if (mWindowButtonButtonNames == null) mWindowButtonButtonNames = GetTexture("window_button_button_names") as Texture; return mWindowButtonButtonNames; } }

    private static Texture mWindowButtonButtonSounds;
    public static Texture WindowButtonButtonSounds { get { if (mWindowButtonButtonSounds == null) mWindowButtonButtonSounds = GetTexture("window_button_button_sounds") as Texture; return mWindowButtonButtonSounds; } }

    private static Texture mWindowButtonUpgradeScene;
    public static Texture WindowButtonUpgradeScene { get { if (mWindowButtonUpgradeScene == null) mWindowButtonUpgradeScene = GetTexture("window_button_upgrade_scene") as Texture; return mWindowButtonUpgradeScene; } }

    private static Texture mWindowButtonResetElementNames;
    public static Texture WindowButtonResetElementNames { get { if (mWindowButtonResetElementNames == null) mWindowButtonResetElementNames = GetTexture("window_button_reset_element_names") as Texture; return mWindowButtonResetElementNames; } }

    private static Texture mWindowButtonResetElementSounds;
    public static Texture WindowButtonResetElementsounds { get { if (mWindowButtonResetElementSounds == null) mWindowButtonResetElementSounds = GetTexture("window_button_reset_element_names") as Texture; return mWindowButtonResetElementSounds; } }

    private static Texture mWindowButtonResetButtonNames;
    public static Texture WindowButtonResetButtonNames { get { if (mWindowButtonResetButtonNames == null) mWindowButtonResetButtonNames = GetTexture("window_button_reset_button_names") as Texture; return mWindowButtonResetButtonNames; } }

    private static Texture mWindowButtonResetButtonSounds;
    public static Texture WindowButtonResetButtonSounds { get { if (mWindowButtonResetButtonSounds == null) mWindowButtonResetButtonSounds = GetTexture("window_button_reset_button_sounds") as Texture; return mWindowButtonResetButtonSounds; } }

    private static Texture mWindowButtonResetRed;
    public static Texture WindowButtonResetRed { get { if (mWindowButtonResetRed == null) mWindowButtonResetRed = GetTexture("window_button_reset_red") as Texture; return mWindowButtonResetRed; } }

    private static Texture mWindowButtonCancelGreen;
    public static Texture WindowButtonCancelGreen { get { if (mWindowButtonCancelGreen == null) mWindowButtonCancelGreen = GetTexture("window_button_cancel_green") as Texture; return mWindowButtonCancelGreen; } }

    private static Texture mWindowButtonQuickHelp;
    public static Texture WindowButtonQuickHelp { get { if (mWindowButtonQuickHelp == null) mWindowButtonQuickHelp = GetTexture("window_button_quick_help") as Texture; return mWindowButtonQuickHelp; } }

    private static Texture mWindowButtonVideoTutorials;
    public static Texture WindowButtonVideoTutorials { get { if (mWindowButtonVideoTutorials == null) mWindowButtonVideoTutorials = GetTexture("window_button_video_tutorials") as Texture; return mWindowButtonVideoTutorials; } }

    private static Texture mWindowButtonPlay;
    public static Texture WindowButtonPlay { get { if (mWindowButtonPlay == null) mWindowButtonPlay = GetTexture("window_button_play") as Texture; return mWindowButtonPlay; } }

    private static Texture mWindowButtonYoutube;
    public static Texture WindowButtonYoutube { get { if (mWindowButtonYoutube == null) mWindowButtonYoutube = GetTexture("window_button_youtube") as Texture; return mWindowButtonYoutube; } }

    private static Texture mWindowButtonOpenDocumentation;
    public static Texture WindowButtonOpenDocumentation { get { if (mWindowButtonOpenDocumentation == null) mWindowButtonOpenDocumentation = GetTexture("window_button_online_documentation") as Texture; return mWindowButtonOpenDocumentation; } }
    #endregion

    #region Window - Messages
    private static Texture mWindowWarningPlayMode;
    public static Texture WindowWarningPlayMode { get { if (mWindowWarningPlayMode == null) mWindowWarningPlayMode = GetTexture("window_warning_play_mode") as Texture; return mWindowWarningPlayMode; } }

    private static Texture mWindowMessageAddDoozy;
    public static Texture WindowMessageAddDoozy { get { if (mWindowMessageAddDoozy == null) mWindowMessageAddDoozy = GetTexture("window_message_add_doozy") as Texture; return mWindowMessageAddDoozy; } }

    private static Texture mWindowMessageWaitCompile;
    public static Texture WindowMessageWaitCompile { get { if (mWindowMessageWaitCompile == null) mWindowMessageWaitCompile = GetTexture("window_message_wait_compile") as Texture; return mWindowMessageWaitCompile; } }
    #endregion

    #region Bars

    #region Component Header Bars
    private static Texture mBarUiElement;
    public static Texture BarUiElement { get { if (mBarUiElement == null) mBarUiElement = GetTexture("inspector_bar_ui_element") as Texture; return mBarUiElement; } }

    private static Texture mBarUiEffect;
    public static Texture BarUiEffect { get { if (mBarUiEffect == null) mBarUiEffect = GetTexture("inspector_bar_ui_effect") as Texture; return mBarUiEffect; } }

    private static Texture mBarUiButton;
    public static Texture BarUiButton { get { if (mBarUiButton == null) mBarUiButton = GetTexture("inspector_bar_ui_button") as Texture; return mBarUiButton; } }

    private static Texture mBarUiNotification;
    public static Texture BarUiNotification { get { if (mBarUiNotification == null) mBarUiNotification = GetTexture("inspector_bar_ui_notification") as Texture; return mBarUiNotification; } }

    private static Texture mBarUiManager;
    public static Texture BarUiManager { get { if (mBarUiManager == null) mBarUiManager = GetTexture("inspector_bar_ui_manager") as Texture; return mBarUiManager; } }

    private static Texture mBarUiTrigger;
    public static Texture BarUiTrigger { get { if (mBarUiTrigger == null) mBarUiTrigger = GetTexture("inspector_bar_ui_trigger") as Texture; return mBarUiTrigger; } }

    private static Texture mBarSceneLoader;
    public static Texture BarSceneLoader { get { if (mBarSceneLoader == null) mBarSceneLoader = GetTexture("inspector_bar_scene_loader") as Texture; return mBarSceneLoader; } }

    private static Texture mBarPmEventDispatcher;
    public static Texture BarPmEventDispatcher { get { if (mBarPmEventDispatcher == null) mBarPmEventDispatcher = GetTexture("inspector_bar_pm_event_dispatcher") as Texture; return mBarPmEventDispatcher; } }

    private static Texture mBarEnabled;
    public static Texture BarEnabled { get { if (mBarEnabled == null) mBarEnabled = GetTexture("inspector_bar_enabled") as Texture; return mBarEnabled; } }

    private static Texture mBarDisabled;
    public static Texture BarDisabled { get { if (mBarDisabled == null) mBarDisabled = GetTexture("inspector_bar_disabled") as Texture; return mBarDisabled; } }
    #endregion

    #region UITrigger Bars
    private static Texture mBarSelectListener;
    public static Texture BarSelectListener { get { if (mBarSelectListener == null) mBarSelectListener = GetTexture("inspector_bar_select_listener") as Texture; return mBarSelectListener; } }

    private static Texture mBarEnterGameEvent;
    public static Texture BarEnterGameEvent { get { if (mBarEnterGameEvent == null) mBarEnterGameEvent = GetTexture("inspector_bar_enter_game_event") as Texture; return mBarEnterGameEvent; } }

    private static Texture mBarEnterButtonName;
    public static Texture BarEnterButtonName { get { if (mBarEnterButtonName == null) mBarEnterButtonName = GetTexture("inspector_bar_enter_button_name") as Texture; return mBarEnterButtonName; } }
    #endregion

    #region UIButton Bars
    private static Texture mBarShowElements;
    public static Texture BarShowElements { get { if (mBarShowElements == null) mBarShowElements = GetTexture("inspector_bar_show_elements") as Texture; return mBarShowElements; } }

    private static Texture mBarHideElements;
    public static Texture BarHideElements { get { if (mBarHideElements == null) mBarHideElements = GetTexture("inspector_bar_hide_elements") as Texture; return mBarHideElements; } }

    private static Texture mBarNavigationDisabled;
    public static Texture BarNavigationDisabled { get { if (mBarNavigationDisabled == null) mBarNavigationDisabled = GetTexture("inspector_bar_navigation_disabled") as Texture; return mBarNavigationDisabled; } }

    private static Texture mBarSendGameEvents;
    public static Texture BarSendGameEvents { get { if (mBarSendGameEvents == null) mBarSendGameEvents = GetTexture("inspector_bar_send_game_events") as Texture; return mBarSendGameEvents; } }

    private static Texture mBarGeneralInfo;
    public static Texture BarGeneralInfo { get { if (mBarGeneralInfo == null) mBarGeneralInfo = GetTexture("inspector_bar_general_info") as Texture; return mBarGeneralInfo; } }
    #endregion

    #region UIManager Bars
    private static Texture mBarMaDisabled;
    public static Texture BarMaDisabled { get { if (mBarMaDisabled == null) mBarMaDisabled = GetTexture("inspector_bar_ma_disabled") as Texture; return mBarMaDisabled; } }

    private static Texture mBarMaEnabled;
    public static Texture BarMaEnabled { get { if (mBarMaEnabled == null) mBarMaEnabled = GetTexture("inspector_bar_ma_enabled") as Texture; return mBarMaEnabled; } }

    private static Texture mBarTmpDisabled;
    public static Texture BarTmpDisabled { get { if (mBarTmpDisabled == null) mBarTmpDisabled = GetTexture("inspector_bar_tmp_disabled") as Texture; return mBarTmpDisabled; } }

    private static Texture mBarTmpEnabled;
    public static Texture BarTmpEnabled { get { if (mBarTmpEnabled == null) mBarTmpEnabled = GetTexture("inspector_bar_tmp_enabled") as Texture; return mBarTmpEnabled; } }

    private static Texture mBarEbtDisabled;
    public static Texture BarEbtDisabled { get { if (mBarEbtDisabled == null) mBarEbtDisabled = GetTexture("inspector_bar_ebt_disabled") as Texture; return mBarEbtDisabled; } }

    private static Texture mBarEbtEnabled;
    public static Texture BarEbtEnabled { get { if (mBarEbtEnabled == null) mBarEbtEnabled = GetTexture("inspector_bar_ebt_enabled") as Texture; return mBarEbtEnabled; } }

    private static Texture mBarPmDisabled;
    public static Texture BarPmDisabled { get { if (mBarPmDisabled == null) mBarPmDisabled = GetTexture("inspector_bar_pm_disabled") as Texture; return mBarPmDisabled; } }

    private static Texture mBarPmEnabled;
    public static Texture BarPmEnabled { get { if (mBarPmEnabled == null) mBarPmEnabled = GetTexture("inspector_bar_pm_enabled") as Texture; return mBarPmEnabled; } }

    private static Texture mBarNavDisabled;
    public static Texture BarNavDisabled { get { if (mBarNavDisabled == null) mBarNavDisabled = GetTexture("inspector_bar_nav_disabled") as Texture; return mBarNavDisabled; } }

    private static Texture mBarNavEnabled;
    public static Texture BarNavEnabled { get { if (mBarNavEnabled == null) mBarNavEnabled = GetTexture("inspector_bar_nav_enabled") as Texture; return mBarNavEnabled; } }

    private static Texture mBarOmDisabled;
    public static Texture BarOmDisabled { get { if (mBarOmDisabled == null) mBarOmDisabled = GetTexture("inspector_bar_om_disabled") as Texture; return mBarOmDisabled; } }

    private static Texture mBarOmEnabled;
    public static Texture BarOmEnabled { get { if (mBarOmEnabled == null) mBarOmEnabled = GetTexture("inspector_bar_om_enabled") as Texture; return mBarOmEnabled; } }
    #endregion

    #region Bar Buttons - Enable and Disable
    private static Texture mBarButtonEnable;
    public static Texture BarButtonEnable { get { if (mBarButtonEnable == null) mBarButtonEnable = GetTexture("inspector_bar_button_enable") as Texture; return mBarButtonEnable; } }

    private static Texture mBarButtonDisable;
    public static Texture BarButtonDisable { get { if (mBarButtonDisable == null) mBarButtonDisable = GetTexture("inspector_bar_button_disable") as Texture; return mBarButtonDisable; } }
    #endregion

    #region MiniBars
    private static Texture mMiniBarLightBlue;
    public static Texture MiniBarLightBlue { get { if (mMiniBarLightBlue == null) mMiniBarLightBlue = GetTexture("inspector_minibar_light_blue") as Texture; return mMiniBarLightBlue; } }

    private static Texture mMiniBarLightGrey;
    public static Texture MiniBarLightGrey { get { if (mMiniBarLightGrey == null) mMiniBarLightGrey = GetTexture("inspector_minibar_light_grey") as Texture; return mMiniBarLightGrey; } }

    private static Texture mMiniBarGreen;
    public static Texture MiniBarGreen { get { if (mMiniBarGreen == null) mMiniBarGreen = GetTexture("inspector_minibar_green") as Texture; return mMiniBarGreen; } }

    private static Texture mMiniBarOrange;
    public static Texture MiniBarOrange { get { if (mMiniBarOrange == null) mMiniBarOrange = GetTexture("inspector_minibar_orange") as Texture; return mMiniBarOrange; } }

    private static Texture mMiniBarRed;
    public static Texture MiniBarRed { get { if (mMiniBarRed == null) mMiniBarRed = GetTexture("inspector_minibar_red") as Texture; return mMiniBarRed; } }

    private static Texture mMiniBarPurple;
    public static Texture MiniBarPurple { get { if (mMiniBarPurple == null) mMiniBarPurple = GetTexture("inspector_minibar_purple") as Texture; return mMiniBarPurple; } }
    #endregion

    #endregion

    #region Buttons
    private static Texture mButtonCancel;
    public static Texture ButtonCancel { get { if (mButtonCancel == null) mButtonCancel = GetTexture("inspector_button_cancel") as Texture; return mButtonCancel; } }

    private static Texture mButtonBack;
    public static Texture ButtonBack { get { if (mButtonBack == null) mButtonBack = GetTexture("inspector_button_back") as Texture; return mButtonBack; } }

    private static Texture mButtonUnlinkFromNotification;
    public static Texture ButtonUnlinkFromNotification { get { if (mButtonUnlinkFromNotification == null) mButtonUnlinkFromNotification = GetTexture("inspector_button_unlink_from_notification") as Texture; return mButtonUnlinkFromNotification; } }

    private static Texture mButtonPortraitDisabled;
    public static Texture ButtonPortraitDisabled { get { if (mButtonPortraitDisabled == null) mButtonPortraitDisabled = GetTexture("inspector_bar_button_portrait_disabled") as Texture; return mButtonPortraitDisabled; } }

    private static Texture mButtonPortraitEnabled;
    public static Texture ButtonPortraitEnabled { get { if (mButtonPortraitEnabled == null) mButtonPortraitEnabled = GetTexture("inspector_bar_button_portrait_enabled") as Texture; return mButtonPortraitEnabled; } }

    private static Texture mButtonLandscapeDisabled;
    public static Texture ButtonLandscapeDisabled { get { if (mButtonLandscapeDisabled == null) mButtonLandscapeDisabled = GetTexture("inspector_bar_button_landscape_disabled") as Texture; return mButtonLandscapeDisabled; } }

    private static Texture mButtonLandscapeEnabled;
    public static Texture ButtonLandscapeEnabled { get { if (mButtonLandscapeEnabled == null) mButtonLandscapeEnabled = GetTexture("inspector_bar_button_landscape_enabled") as Texture; return mButtonLandscapeEnabled; } }
    #endregion

    #region Special Messages
    private static Texture mMessageLinkedToNotification;
    public static Texture MessageLinkedToNotification { get { if (mMessageLinkedToNotification == null) mMessageLinkedToNotification = GetTexture("inspector_message_linked_to_notification") as Texture; return mMessageLinkedToNotification; } }

    private static Texture mMessageWaitCompile;
    public static Texture MessageWaitCompile { get { if (mMessageWaitCompile == null) mMessageWaitCompile = GetTexture("inspector_message_wait_compile") as Texture; return mMessageWaitCompile; } }
    #endregion

    #region UIElement - Icons

    #region IN, LOOP, OUT
    private static Texture mIconInEnabled;
    public static Texture IconInEnabled { get { if (mIconInEnabled == null) mIconInEnabled = GetTexture("inspector_icon_in_enabled") as Texture; return mIconInEnabled; } }

    private static Texture mIconInDisabled;
    public static Texture IconInDisabled { get { if (mIconInDisabled == null) mIconInDisabled = GetTexture("inspector_icon_in_disabled") as Texture; return mIconInDisabled; } }

    private static Texture mIconLoopEnabled;
    public static Texture IconLoopEnabled { get { if (mIconLoopEnabled == null) mIconLoopEnabled = GetTexture("inspector_icon_loop_enabled") as Texture; return mIconLoopEnabled; } }

    private static Texture mIconLoopDisabled;
    public static Texture IconLoopDisabled { get { if (mIconLoopDisabled == null) mIconLoopDisabled = GetTexture("inspector_icon_loop_disabled") as Texture; return mIconLoopDisabled; } }

    private static Texture mIconOutEnabled;
    public static Texture IconOutEnabled { get { if (mIconOutEnabled == null) mIconOutEnabled = GetTexture("inspector_icon_out_enabled") as Texture; return mIconOutEnabled; } }

    private static Texture mIconOutDisabled;
    public static Texture IconOutDisabled { get { if (mIconOutDisabled == null) mIconOutDisabled = GetTexture("inspector_icon_out_disabled") as Texture; return mIconOutDisabled; } }
    #endregion

    #region MOVE, ROTATION, SCALE, FADE
    private static Texture mIconMoveEnabled;
    public static Texture IconMoveEnabled { get { if (mIconMoveEnabled == null) mIconMoveEnabled = GetTexture("inspector_icon_move_enabled") as Texture; return mIconMoveEnabled; } }

    private static Texture mIconMoveDisabled;
    public static Texture IconMoveDisabled { get { if (mIconMoveDisabled == null) mIconMoveDisabled = GetTexture("inspector_icon_move_disabled") as Texture; return mIconMoveDisabled; } }

    private static Texture mIconRotationEnabled;
    public static Texture IconRotationEnabled { get { if (mIconRotationEnabled == null) mIconRotationEnabled = GetTexture("inspector_icon_rotation_enabled") as Texture; return mIconRotationEnabled; } }

    private static Texture mIconRotationDisabled;
    public static Texture IconRotationDisabled { get { if (mIconRotationDisabled == null) mIconRotationDisabled = GetTexture("inspector_icon_rotation_disabled") as Texture; return mIconRotationDisabled; } }

    private static Texture mIconScaleEnabled;
    public static Texture IconScaleEnabled { get { if (mIconScaleEnabled == null) mIconScaleEnabled = GetTexture("inspector_icon_scale_enabled") as Texture; return mIconScaleEnabled; } }

    private static Texture mIconScaleDisabled;
    public static Texture IconScaleDisabled { get { if (mIconScaleDisabled == null) mIconScaleDisabled = GetTexture("inspector_icon_scale_disabled") as Texture; return mIconScaleDisabled; } }

    private static Texture mIconFadeEnabled;
    public static Texture IconFadeEnabled { get { if (mIconFadeEnabled == null) mIconFadeEnabled = GetTexture("inspector_icon_fade_enabled") as Texture; return mIconFadeEnabled; } }

    private static Texture mIconFadeDisabled;
    public static Texture IconFadeDisabled { get { if (mIconFadeDisabled == null) mIconFadeDisabled = GetTexture("inspector_icon_fade_disabled") as Texture; return mIconFadeDisabled; } }
    #endregion

    #endregion

    #region UIElement - Labels

    #region  IN, LOOP, OUT  - ANIMATIONS
    private static Texture mLabelInAnimations;
    public static Texture LabelInAnimations { get { if (mLabelInAnimations == null) mLabelInAnimations = GetTexture("inspector_label_in_animations") as Texture; return mLabelInAnimations; } }

    private static Texture mLabelInAnimationsDisabled;
    public static Texture LabelInAnimationsDisabled { get { if (mLabelInAnimationsDisabled == null) mLabelInAnimationsDisabled = GetTexture("inspector_label_in_animations_disabled") as Texture; return mLabelInAnimationsDisabled; } }

    private static Texture mLabelLoopAnimations;
    public static Texture LabelLoopAnimations { get { if (mLabelLoopAnimations == null) mLabelLoopAnimations = GetTexture("inspector_label_loop_animations") as Texture; return mLabelLoopAnimations; } }

    private static Texture mLabelLoopAnimationsDisabled;
    public static Texture LabelLoopAnimationsDisabled { get { if (mLabelLoopAnimationsDisabled == null) mLabelLoopAnimationsDisabled = GetTexture("inspector_label_loop_animations_disabled") as Texture; return mLabelLoopAnimationsDisabled; } }

    private static Texture mLabelOutAnimations;
    public static Texture LabelOutAnimations { get { if (mLabelOutAnimations == null) mLabelOutAnimations = GetTexture("inspector_label_out_animations") as Texture; return mLabelOutAnimations; } }

    private static Texture mLabelOutAnimationsDisabled;
    public static Texture LabelOutAnimationsDisabled { get { if (mLabelOutAnimationsDisabled == null) mLabelOutAnimationsDisabled = GetTexture("inspector_label_out_animations_disabled") as Texture; return mLabelOutAnimationsDisabled; } }
    #endregion

    #region  MOVE - IN, LOOP, OUT
    private static Texture mLabelMoveInEnabled;
    public static Texture LabelMoveInEnabled { get { if (mLabelMoveInEnabled == null) mLabelMoveInEnabled = GetTexture("inspector_label_moveIn_enabled") as Texture; return mLabelMoveInEnabled; } }

    private static Texture mLabelMoveInDisabled;
    public static Texture LabelMoveInDisabled { get { if (mLabelMoveInDisabled == null) mLabelMoveInDisabled = GetTexture("inspector_label_moveIn_disabled") as Texture; return mLabelMoveInDisabled; } }

    private static Texture mLabelMoveLoopEnabled;
    public static Texture LabelMoveLoopEnabled { get { if (mLabelMoveLoopEnabled == null) mLabelMoveLoopEnabled = GetTexture("inspector_label_moveLoop_enabled") as Texture; return mLabelMoveLoopEnabled; } }

    private static Texture mLabelMoveLoopDisabled;
    public static Texture LabelMoveLoopDisabled { get { if (mLabelMoveLoopDisabled == null) mLabelMoveLoopDisabled = GetTexture("inspector_label_moveLoop_disabled") as Texture; return mLabelMoveLoopDisabled; } }

    private static Texture mLabelMoveOutEnabled;
    public static Texture LabelMoveOutEnabled { get { if (mLabelMoveOutEnabled == null) mLabelMoveOutEnabled = GetTexture("inspector_label_moveOut_enabled") as Texture; return mLabelMoveOutEnabled; } }

    private static Texture mLabelMoveOutDisabled;
    public static Texture LabelMoveOutDisabled { get { if (mLabelMoveOutDisabled == null) mLabelMoveOutDisabled = GetTexture("inspector_label_moveOut_disabled") as Texture; return mLabelMoveOutDisabled; } }
    #endregion

    #region ROTATION - IN, LOOP, OUT
    private static Texture mLabelRotateInEnabled;
    public static Texture LabelRotateInEnabled { get { if (mLabelRotateInEnabled == null) mLabelRotateInEnabled = GetTexture("inspector_label_rotateIn_enabled") as Texture; return mLabelRotateInEnabled; } }

    private static Texture mLabelRotateInDisabled;
    public static Texture LabelRotateInDisabled { get { if (mLabelRotateInDisabled == null) mLabelRotateInDisabled = GetTexture("inspector_label_rotateIn_disabled") as Texture; return mLabelRotateInDisabled; } }

    private static Texture mLabelRotateLoopEnabled;
    public static Texture LabelRotateLoopEnabled { get { if (mLabelRotateLoopEnabled == null) mLabelRotateLoopEnabled = GetTexture("inspector_label_rotateLoop_enabled") as Texture; return mLabelRotateLoopEnabled; } }

    private static Texture mLabelRotateLoopDisabled;
    public static Texture LabelRotateLoopDisabled { get { if (mLabelRotateLoopDisabled == null) mLabelRotateLoopDisabled = GetTexture("inspector_label_rotateLoop_disabled") as Texture; return mLabelRotateLoopDisabled; } }

    private static Texture mLabelRotateOutEnabled;
    public static Texture LabelRotateOutEnabled { get { if (mLabelRotateOutEnabled == null) mLabelRotateOutEnabled = GetTexture("inspector_label_rotateOut_enabled") as Texture; return mLabelRotateOutEnabled; } }

    private static Texture mLabelRotateOutDisabled;
    public static Texture LabelRotateOutDisabled { get { if (mLabelRotateOutDisabled == null) mLabelRotateOutDisabled = GetTexture("inspector_label_rotateOut_disabled") as Texture; return mLabelRotateOutDisabled; } }
    #endregion

    #region SCALE - IN, LOOP, OUT
    private static Texture mLabelScaleInEnabled;
    public static Texture LabelScaleInEnabled { get { if (mLabelScaleInEnabled == null) mLabelScaleInEnabled = GetTexture("inspector_label_scaleIn_enabled") as Texture; return mLabelScaleInEnabled; } }

    private static Texture mLabelScaleInDisabled;
    public static Texture LabelScaleInDisabled { get { if (mLabelScaleInDisabled == null) mLabelScaleInDisabled = GetTexture("inspector_label_scaleIn_disabled") as Texture; return mLabelScaleInDisabled; } }

    private static Texture mLabelScaleLoopEnabled;
    public static Texture LabelScaleLoopEnabled { get { if (mLabelScaleLoopEnabled == null) mLabelScaleLoopEnabled = GetTexture("inspector_label_scaleLoop_enabled") as Texture; return mLabelScaleLoopEnabled; } }

    private static Texture mLabelScaleLoopDisabled;
    public static Texture LabelScaleLoopDisabled { get { if (mLabelScaleLoopDisabled == null) mLabelScaleLoopDisabled = GetTexture("inspector_label_scaleLoop_disabled") as Texture; return mLabelScaleLoopDisabled; } }

    private static Texture mLabelScaleOutEnabled;
    public static Texture LabelScaleOutEnabled { get { if (mLabelScaleOutEnabled == null) mLabelScaleOutEnabled = GetTexture("inspector_label_scaleOut_enabled") as Texture; return mLabelScaleOutEnabled; } }

    private static Texture mLabelScaleOutDisabled;
    public static Texture LabelScaleOutDisabled { get { if (mLabelScaleOutDisabled == null) mLabelScaleOutDisabled = GetTexture("inspector_label_scaleOut_disabled") as Texture; return mLabelScaleOutDisabled; } }
    #endregion

    #region FADE - IN, LOOP, OUT
    private static Texture mLabelFadeInEnabled;
    public static Texture LabelFadeInEnabled { get { if (mLabelFadeInEnabled == null) mLabelFadeInEnabled = GetTexture("inspector_label_fadeIn_enabled") as Texture; return mLabelFadeInEnabled; } }

    private static Texture mLabelFadeInDisabled;
    public static Texture LabelFadeInDisabled { get { if (mLabelFadeInDisabled == null) mLabelFadeInDisabled = GetTexture("inspector_label_fadeIn_disabled") as Texture; return mLabelFadeInDisabled; } }

    private static Texture mLabelFadeLoopEnabled;
    public static Texture LabelFadeLoopEnabled { get { if (mLabelFadeLoopEnabled == null) mLabelFadeLoopEnabled = GetTexture("inspector_label_fadeLoop_enabled") as Texture; return mLabelFadeLoopEnabled; } }

    private static Texture mLabelFadeLoopDisabled;
    public static Texture LabelFadeLoopDisabled { get { if (mLabelFadeLoopDisabled == null) mLabelFadeLoopDisabled = GetTexture("inspector_label_fadeLoop_disabled") as Texture; return mLabelFadeLoopDisabled; } }

    private static Texture mLabelFadeOutEnabled;
    public static Texture LabelFadeOutEnabled { get { if (mLabelFadeOutEnabled == null) mLabelFadeOutEnabled = GetTexture("inspector_label_fadeOut_enabled") as Texture; return mLabelFadeOutEnabled; } }

    private static Texture mLabelFadeOutDisabled;
    public static Texture LabelFadeOutDisabled { get { if (mLabelFadeOutDisabled == null) mLabelFadeOutDisabled = GetTexture("inspector_label_fadeOut_disabled") as Texture; return mLabelFadeOutDisabled; } }
    #endregion

    #endregion

    #region UIButton - Labels

    #region ON CLICK, NORMAL, HIGHLIGHTED - ANIMATIONS
    private static Texture mLabelOnClickAnimations;
    public static Texture LabelOnClickAnimations { get { if (mLabelOnClickAnimations == null) mLabelOnClickAnimations = GetTexture("inspector_label_on_click_animations") as Texture; return mLabelOnClickAnimations; } }

    private static Texture mLabelOnClickAnimationsDisabled;
    public static Texture LabelOnClickAnimationsDisabled { get { if (mLabelOnClickAnimationsDisabled == null) mLabelOnClickAnimationsDisabled = GetTexture("inspector_label_on_click_animations_disabled") as Texture; return mLabelOnClickAnimationsDisabled; } }

    private static Texture mLabelNormalAnimations;
    public static Texture LabelNormalAnimations { get { if (mLabelNormalAnimations == null) mLabelNormalAnimations = GetTexture("inspector_label_normal_animations") as Texture; return mLabelNormalAnimations; } }

    private static Texture mLabelNormalAnimationsDisabled;
    public static Texture LabelNormalAnimationsDisabled { get { if (mLabelNormalAnimationsDisabled == null) mLabelNormalAnimationsDisabled = GetTexture("inspector_label_normal_animations_disabled") as Texture; return mLabelNormalAnimationsDisabled; } }

    private static Texture mLabelHighlightedAnimations;
    public static Texture LabelHighlightedAnimations { get { if (mLabelHighlightedAnimations == null) mLabelHighlightedAnimations = GetTexture("inspector_label_highlighted_animations") as Texture; return mLabelHighlightedAnimations; } }

    private static Texture mLabelHighlightedAnimationsDisabled;
    public static Texture LabelHighlightedAnimationsDisabled { get { if (mLabelHighlightedAnimationsDisabled == null) mLabelHighlightedAnimationsDisabled = GetTexture("inspector_label_highlighted_animations_disabled") as Texture; return mLabelHighlightedAnimationsDisabled; } }
    #endregion

    #region PUNCH - MOVE, ROTATE, SCALE
    private static Texture mLabelMovePunchEnabled;
    public static Texture LabelMovePunchEnabled { get { if (mLabelMovePunchEnabled == null) mLabelMovePunchEnabled = GetTexture("inspector_label_movePunch_enabled") as Texture; return mLabelMovePunchEnabled; } }

    private static Texture mLabelMovePunchDisabled;
    public static Texture LabelMovePunchDisabled { get { if (mLabelMovePunchDisabled == null) mLabelMovePunchDisabled = GetTexture("inspector_label_movePunch_disabled") as Texture; return mLabelMovePunchDisabled; } }

    private static Texture mLabelRotatePunchEnabled;
    public static Texture LabelRotatePunchEnabled { get { if (mLabelRotatePunchEnabled == null) mLabelRotatePunchEnabled = GetTexture("inspector_label_rotatePunch_enabled") as Texture; return mLabelRotatePunchEnabled; } }

    private static Texture mLabelRotatePunchDisabled;
    public static Texture LabelRotatePunchDisabled { get { if (mLabelRotatePunchDisabled == null) mLabelRotatePunchDisabled = GetTexture("inspector_label_rotatePunch_disabled") as Texture; return mLabelRotatePunchDisabled; } }

    private static Texture mLabelScalePunchEnabled;
    public static Texture LabelScalePunchEnabled { get { if (mLabelScalePunchEnabled == null) mLabelScalePunchEnabled = GetTexture("inspector_label_scalePunch_enabled") as Texture; return mLabelScalePunchEnabled; } }

    private static Texture mLabelScalePunchDisabled;
    public static Texture LabelScalePunchDisabled { get { if (mLabelScalePunchDisabled == null) mLabelScalePunchDisabled = GetTexture("inspector_label_scalePunch_disabled") as Texture; return mLabelScalePunchDisabled; } }
    #endregion

    #endregion
}
#endif

