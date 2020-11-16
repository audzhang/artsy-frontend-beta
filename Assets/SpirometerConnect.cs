using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SpirometerConnect : MonoBehaviour
{
    private int[] _volList;
    private int[] _flowList;

    public Text volumeValue;
    public Text flowValue;
    public Text balanceHere;

    private const int numDataPoints = 21;

    //false if clicked once, true if clicked twice
    bool firstBreath = false;

    //private SpirometerData spiroChallenge;

    void Start ()
    {
      SetCoinBalance();
    }

    public void StartGoodBreath()
    {
      _volList = new int[] { 0, 2, 3, 4, 5, 6, 8, 9, 10, 10, 9, 8, 9, 10, 10, 10, 10, 10, 10, 10, 10};
      _flowList = new int[] { 0, 3, 2, 2, 2, 3, 3, 2, 2, 0, 0, 0, 3, 3, 3, 2, 3, 3, 3, 3, 3 };
      this.StartCoroutine(GetText());
    }

    IEnumerator GetText() {
      string url = "https://uql53bqfta.execute-api.us-east-1.amazonaws.com/Artsy/add-breath";

      // TODO: Waiting on backend to change Headers to Form Data
      List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
      UnityWebRequest request = UnityWebRequest.Post(url, formData);
      request.SetRequestHeader("userId", "testUser-1605385650936673515");
      //says they are string?
      string temp = "0";
      request.SetRequestHeader("volume", temp);
      request.SetRequestHeader("flow", temp);

      yield return request.SendWebRequest();

      string data = request.downloadHandler.text;
      Spirometer spiro = Spirometer.CreateFromJSON(data);
      PlayerPrefs.SetInt("COIN_BALANCE", spiro.balance);
      PlayerPrefs.SetInt("BREATH_COUNT", spiro.breathCount);

      if (spiro.breathCount >= 10)
      {
        PlayerPrefs.SetInt("UNLIMITED_STATUS", 1);
      }

      SetCoinBalance();

      startChallenge();

    }

    private void SetCoinBalance ()
    {
      if (PlayerPrefs.HasKey("COIN_BALANCE")) {
        int coins = PlayerPrefs.GetInt("COIN_BALANCE");
        GameObject balanceObject = GameObject.Find("CoinBalance");
        balanceObject.GetComponent<Text>().text = coins.ToString();
      }
    }

    void Breath1Volume() {
      //volumeValue = gameObject.GetComponent<Text>();
      volumeValue.text = _volList[12].ToString();
      //flow = -_flowList[1];
      //spiroChallenge.SetAirflowText(flow.ToString());
      //spiroChallenge.SetVolumeText(volume.ToString());
    }
    void Breath1Flow() {
      //volume = _volList[1];
      //flowValue = gameObject.GetComponent<Text>();
      flowValue.text = _flowList[12].ToString();
      //spiroChallenge.SetAirflowText(flow.ToString());
      //spiroChallenge.SetVolumeText(volume.ToString());
    }

    void Breath2Volume() {
      //volumeValue = gameObject.GetComponent<Text>();
      volumeValue.text = _volList[13].ToString();
      //flowValue.text = _flowList[2];
      //spiroChallenge.SetAirflowText(flow.ToString());
      //spiroChallenge.SetVolumeText(volume.ToString());
    }

    void Breath2Flow() {
      //volumeValue.text = _volList[2];
      //flowValue = gameObject.GetComponent<Text>();
      flowValue.text = _flowList[15].ToString();
      //spiroChallenge.SetAirflowText(flow.ToString());
      //spiroChallenge.SetVolumeText(volume.ToString());
    }

    void startChallenge() {
      if (firstBreath == false) {
        Breath1Volume();
        Breath1Flow();
        firstBreath = !firstBreath;
      }
      else {
        Breath2Volume();
        Breath2Flow();
        firstBreath = !firstBreath;
      }
    }
    //connor
    public void GoToHome()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void OnApplicationQuit()
    {
        Debug.Log("QUITTING APPLICATION");
        PlayerPrefs.DeleteAll();
    }
}

[System.Serializable]
public class Spirometer
{
    public int balance;
    public int breathCount;

    public static Spirometer CreateFromJSON(string jsonString = "helloWorld")
    {
        return JsonUtility.FromJson<Spirometer>(jsonString);
    }

}
