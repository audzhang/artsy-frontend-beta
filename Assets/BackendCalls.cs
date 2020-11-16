// ﻿using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Networking;
//
// public static class BackendCalls
// {
//     private static string backendUrl = "https://e6oda8c1s9.execute-api.us-east-1.amazonaws.com/beta";
//     // private int[] palettesOwneed;
//
//     // public static void fetchOwnedPalettes() {
//         // Debug.Log("STARTING TO FETCH");
//         // string url = backendUrl + "/get-user-info?userId=testPatient";
//         // UnityWebRequest req = UnityWebRequest.Get(url);
//         // yield return req.SendWebRequest();
//
//         // if (req.isNetworkError || req.isHttpError)
//         // {
//         //     Debug.LogError(req.error);
//         //     yield break;
//         // }
//
//         // Debug.Log(req.downloadHandler.text);
//         // JSONNode userInfo = JSON.Parse(req.downloadHandler.text);
//         // string[] palettes = userInfo["palettes"];
//         // Debug.Log(palettes[0]);
//         // using (UnityWebRequest webRequest = UnityWebRequest.Get(backendUrl + "/get-user-info?userId=testPatient"))
//         // {
//         // Debug.Log("FETCHING PALETTES");
//             // yield return webRequest.SendWebRequest();
//
//             // string[] pages = uri.Split('/');
//             // int page = pages.Length - 1;
//
//             // if (webRequest.isNetworkError)
//             // {
//             //     Debug.Log(pages[page] + ": Error: " + webRequest.error);
//             // }
//             // else
//             // {
//             //     Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
//             // }
//         // int[] arr = {1,2};
//         // return arr;
//         // }
//     // }
//
//     public static IEnumerator Awake()
//      {
//         string url = backendUrl + "/get-user-info?userId=testPatient";
//         Debug.Log(url);
//         var request = UnityWebRequest.Get(url);
//
//          // Wait for the response and then get our data
//          yield return request.SendWebRequest();
//          var data = request.downloadHandler.text;
//
//         Action<string> callback = ResponseCallback;
//
//          // This isn't required, but I prefer to pass in a callback so that I can
//          // act on the response data outside of this function
//          // if (callback != null)
//          //     callback(data);
//
//
//         // this.StartCoroutine(RequestRoutine(url, ResponseCallback));
//      }
//
//      // Web requests are typially done asynchronously, so Unity's web request system
//      // returns a yield instruction while it waits for the response.
//      //
//      // private static IEnumerator RequestRoutine(string url, Action<string> callback = null)
//      // {
//      //     // Using the static constructor
//      //    var request = UnityWebRequest.Get(url);
//
//      //     // Wait for the response and then get our data
//      //     yield return request.SendWebRequest();
//      //     var data = request.downloadHandler.text;
//
//      //     // This isn't required, but I prefer to pass in a callback so that I can
//      //     // act on the response data outside of this function
//      //     if (callback != null)
//      //         callback(data);
//      // }
//
//      // Callback to act on our response data
//      private static void ResponseCallback(string data)
//      {
//          Debug.Log(data);
//          // recentData = data;
//      }
//
// }
