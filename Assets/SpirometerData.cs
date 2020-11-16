// ﻿using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.SceneManagement;
// using UnityEngine.Networking;
// //
// public class SpirometerData : MonoBehaviour
// {
//     private bool displayed = false;
//     public GameObject airflowValueText;
//     public GameObject volumeValueText;
//
//     public void Display() {
//       displayed = !displayed;
//       airflowValueText.SetActive(displayed);
//       volumeValueText.SetActive(displayed);
//     }
//
//     public void SetAirflowText(string newFlow) {
//       airflowValueText.GetComponent<Text>().text = newFlow;
//     }
// 
//     public void SetVolumeText(string newVolume) {
//       volumeValueText.GetComponent<Text>().text = newVolume;
//     }
//
//     public void GoToHome()
//     {
//         SceneManager.LoadScene("MenuScene");
//     }
//
//     public void OnApplicationQuit()
//     {
//         Debug.Log("QUITTING APPLICATION");
//         PlayerPrefs.DeleteAll();
//     }
//
// }
