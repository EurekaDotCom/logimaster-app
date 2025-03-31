using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static  class ClientInfo 
{
    public static string token;
    public static int company_id;
    public static string email;
    public static string password;
    public static string username;
    public static string company_name;


    public static bool IsAutoFillLogin;
}

[Serializable]
public class Token
{
    public string token;
}
