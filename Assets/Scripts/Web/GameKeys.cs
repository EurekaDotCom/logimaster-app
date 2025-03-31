using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameKeys 
{
    //Tags
    public const string Tag_Wood = "Wood";
    public const string Tag_Player = "Player";
    public const string Tag_Forks = "Forks";
    public const string Tag_LongForks = "LongForks";
    public const string Tag_StopForksTrigger  = "StopForksTrigger";

    //Animations
    public const string Animation_ForksBase = "ForksBase";
    public const string Animation_Forks = "Forks";

    //Scenes
    public const string Scene_Login = "Login";
    public const string Scene_SignUp = "SignUp";
    public const string Scene_SignUpWithCode = "SignUpWithCode";
    public const string Scene_PC_Multiform = "PC_Multiform";

    //Strings
    public const string Euro = "\u20AC";
    public const string WaitForklift = "Please wait until the forklift finishes its work.";

    //Values
    public const float PalletPrice = 20f;
    public const int ShowMoreLangID = 4;
    public const int ShowLessLangID = 5;
    public const int ThicknessLangID = 6;
    public const int WidthLangID = 7;
    public const int LenghtLangID = 8;
    public const int MaterialLangID = 2;
    public const float StandartPaletWidth = 0.8f;
    public const float StandartPaletLenght = 1.2f;

    //PathLocation
    public const string LanguagesLocation = "Languages";

    //Errors
    public const string EmptyField = "This field is mandatory!";
    public const string NotExist = "Invalid email or password";
    public const string PasswordsNotMatch = "Password do not match";
    public const string CharactersMissing = "The password should contain at least 8 characters, one capital letter, one digit and one special symbol.";
    public const string InvalidEmail = "Invalid email";

    //Save
    public const string RemeberMe = "RemeberMe";
    public const string Saved_Email = "Saved_Email";
    public const string Saved_Pass = "Saved_Pass";
}
