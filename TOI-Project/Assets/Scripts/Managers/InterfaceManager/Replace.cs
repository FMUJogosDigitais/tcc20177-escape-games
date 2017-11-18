using System;
using System.Text.RegularExpressions;


public class Replace
{
    static string[] controller_pattern = new string[] { "\\[L_VERTICAL\\]", "\\[L_HORIZONTAL\\]", "\\[R_HORIZONTAL\\]", "\\[R_HORIZONTAL\\]", "\\[CENTER_1\\]", "\\[CENTER_2\\]", "\\[ACTION_A\\]", "\\[ACTION_B\\]", "\\[ACTION_X\\]","\\[ACTION_Y\\]","\\[LEFT_SHOULDER_1\\]","\\[RIGHT_SHOULDER_1\\]","\\[LEFT_SHOULDER_2\\]","\\[RIGHT_SHOULDER_2\\]","\\[DPAD\\]","\\[DPAD_UP]","\\[DPAD_DOWN]","\\[DPAD_LEFT\\]","\\[DPAD_RIGHT\\]","\\[LEFT_STICK_BUTTON\\]","\\[RIGHT_STICK_BUTTON\\]"};
    static string[] keyboard_pattern = new string[] { "\\[KEY_ESC\\]", "\\[KEY_F1\\]", "\\[KEY_F2\\]", "\\[KEY_F3\\]", "\\[KEY_F4\\]", "\\[KEY_F5\\]", "\\[KEY_F6\\]", "\\[KEY_F7\\]", "\\[KEY_F8\\]", "\\[KEY_F9\\]", "\\[KEY_F10\\]", "\\[KEY_F11\\]", "\\[KEY_F12\\]", "\\[KEY_PS\\]", "\\[KEY_NUMLK\\]", "\\[KEY_STAR\\]", "\\[KEY_BCKSP\\]", "\\[KEY_INS\\]", "\\[KEY_HOME\\]", "\\[KEY_PGUP\\]", "\\[KEY_TILDE\\]", "\\[KEY_1\\]", "\\[KEY_2\\]", "\\[KEY_3\\]", "\\[KEY_4\\]", "\\[KEY_5\\]", "\\[KEY_6\\]", "\\[KEY_7\\]", "\\[KEY_8\\]", "\\[KEY_9\\]", "\\[KEY_0\\]", "\\[KEY_MINUS\\]", "\\[KEY_PLUS\\]", "\\[KEY_DEL\\]", "\\[KEY_END\\]", "\\[KEY_PGDN\\]", "\\[KEY_BACKSLASH\\]", "\\[KEY_TAB\\]", "\\[KEY_Q\\]", "\\[KEY_W\\]", "\\[KEY_E\\]", "\\[KEY_R\\]", "\\[KEY_T\\]", "\\[KEY_Y\\]", "\\[KEY_U\\]", "\\[KEY_I\\]", "\\[KEY_O\\]", "\\[KEY_P\\]", "\\[KEY_L_BRACKET\\]", "\\[KEY_R_BRACKET\\]", "\\[KEY_CAPS\\]", "\\[KEY_A\\]", "\\[KEY_S\\]", "\\[KEY_D\\]", "\\[KEY_F\\]", "\\[KEY_G\\]", "\\[KEY_H\\]", "\\[KEY_J\\]", "\\[KEY_K\\]", "\\[KEY_L\\]", "\\[KEY_SEMICOLON\\]", "\\[KEY_QUOTE\\]", "\\[KEY_ENTER\\]", "\\[KEY_NUM_PLUS\\]", "\\[KEY_NUM_ENTER\\]", "\\[KEY_SHIFT\\]", "\\[KEY_R_SHIFT\\]", "\\[KEY_X\\]", "\\[KEY_C\\]", "\\[KEY_V\\]", "\\[KEY_B\\]", "\\[KEY_B\\]", "\\[KEY_MARK_LEFT\\]", "\\[KEY_MARK_RIGHT\\]", "\\[KEY_QUESTION\\]", "\\[KEY_Z\\]", "\\[KEY_M\\]", "\\[KEY_SPACE\\]", "\\[KEY_CTRL\\]", "\\[KEY_ALT\\]", "\\[KEY_WIN\\]", "\\[KEY_COMMAND\\]", "\\[KEY_BACKSPACE\\]", "\\[KEY_WS\\]", "\\[KEY_VERTICAL\\]", "\\[KEY_AD\\]", "\\[KEY_HORIZONTAL\\]", "\\[MOUSE_SCROLL\\]", "\\[MOUSE_LEFT\\]", "\\[MOUSE_MIDDLE\\]", "\\[MOUSE_RIGHT\\]", "\\[MOUSE_HORIZONTAL\\]", "\\[KEY_RETURN\\]", "\\[MOUSE_VERTICAL\\]","\\[KEY_UP\\]", "\\[KEY_DOWN\\]", "\\[KEY_LEFT\\]", "\\[KEY_RIGHT\\]" };


    public static string GetControllerIcons(string languageKey)
    {
        return GetIcons(controller_pattern, languageKey);
    }

    public static string GetKeyboardIcons(string languageKey)
    {
        return GetIcons(keyboard_pattern, languageKey);
    }


    private static string GetIcons(string[] pattern, string languageKey)
    {
        string newKey = languageKey;

        for (int i = 0; i < pattern.Length; i++)
        {
            newKey = Regex.Replace(newKey, pattern[i], "<sprite=" + i + ">");
        }

        return newKey;

    }



}
