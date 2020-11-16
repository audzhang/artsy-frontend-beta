using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

[RequireComponent(typeof(Camera))]
public class SnapshotCamera : MonoBehaviour
{
    Camera snapCam;
    int resWidth = 1024;
    int resHeight = 1024;

    private static string backendUrlSave = "https://uql53bqfta.execute-api.us-east-1.amazonaws.com/Artsy/save-drawing";

    void Awake() {
      snapCam = GetComponent<Camera>();
      if (snapCam.targetTexture == null) {
        snapCam.targetTexture = new RenderTexture(resWidth, resHeight, 24);
      } else {
        resWidth = snapCam.targetTexture.width;
        resHeight = snapCam.targetTexture.height;
      }

      // not using Camera
      snapCam.gameObject.SetActive(false);
    }

    public void CallTakeSnapshot() {
      // turn on snapshot camera
      snapCam.gameObject.SetActive(true);
    }

    void LateUpdate() {
      if (snapCam.gameObject.activeInHierarchy) {
        // taking picture
        Texture2D snapshot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        snapCam.Render();
        RenderTexture.active = snapCam.targetTexture;
        snapshot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        byte[] bytes = snapshot.EncodeToPNG();
        string filename = SnapshotName();
        System.IO.File.WriteAllBytes(filename, bytes);
        //send screenshot to backend
        Debug.Log("before sending to backend");
        this.StartCoroutine(sendScreenshot(filename));
        Debug.Log("Snapshot taken!");
        snapCam.gameObject.SetActive(false);
      }
    }

    string SnapshotName() {
      return string.Format("{0}/Snapshots/snap_{1}x{2}_{3}.png",
        Application.dataPath,
        resWidth,
        resHeight,
        System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }

    IEnumerator sendScreenshot(string filename) {
      Debug.Log("starting to send screenshot to backend");
      string urlSave = "https://uql53bqfta.execute-api.us-east-1.amazonaws.com/Artsy/save-drawing";
      Debug.Log("1");
      List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
      Debug.Log("2");

      UnityWebRequest request = UnityWebRequest.Post(urlSave, formData);
      Debug.Log("3");
      string drawingId = PlayerPrefs.GetString("DRAWING_ID");
      Debug.Log(drawingId);
      Debug.Log("4");
      request.SetRequestHeader("drawingID", drawingId);
      Debug.Log("4");
      request.SetRequestHeader("filename", filename);
      Debug.Log("6");

      yield return request.SendWebRequest();
      Debug.Log("7");
      var data = request.downloadHandler.text;
      Debug.Log("my 8");

      Save save = Save.CreateFromJSON(data);
      Debug.Log("9 i am dead inside");
      PlayerPrefs.SetString("PNG_FILE", save.png_file);
      Debug.Log("10");
      PlayerPrefs.SetString("SVG_FILE", save.svg_file);
      Debug.Log("11");

      if (request.isNetworkError || request.isHttpError)
      {
          Debug.Log(request.error);
          Debug.Log("request error");
      } else {
        Debug.Log("sent drawing to backend");
      }
}

[System.Serializable]
public class Save {
  public string png_file;
  public string svg_file;

  public static Save CreateFromJSON(string jsonString = "helloWorld")
  {
      return JsonUtility.FromJson<Save>(jsonString);
  }
}}
