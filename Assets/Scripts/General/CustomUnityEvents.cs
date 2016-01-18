using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using UnityEngine.Cloud.Analytics;

public class CustomUnityEvents : Singleton<CustomUnityEvents>
{
    //private string text = "";
    public void AcceptBark ()
    {
        //UnityAnalytics.CustomEvent("BarkAccept", new Dictionary<string, object>
        //{
        //    { "text", text },
        //});
    }

    public void RejectBark()
    {
        //UnityAnalytics.CustomEvent("BarkReject", new Dictionary<string, object>
        //{
        //    { "text", text },
        //});
    }

    public void AllBark(string message)
    {
        //text = message;
        //UnityAnalytics.CustomEvent("BarkAll", new Dictionary<string, object>
        //{
        //    { "text", message },
        //});
    }


}
