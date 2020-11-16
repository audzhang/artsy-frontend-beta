using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class fetchRecentDrawings : MonoBehaviour
{
    public RawImage drawing1;
    public RawImage drawing2;
    public RawImage drawing3;
    public RawImage drawing4;
    public RawImage drawing5;

    public Text textDate1;
    public Text textDate2;
    public Text textDate3;
    public Text textDate4;
    public Text textDate5;

    // Start is called before the first frame update
    void Start()
    {
        string url1 = "http://pngimg.com/uploads/beach/beach_PNG96.png";
        string url2 = "http://pngimg.com/uploads/arctic_fox/arctic_fox_PNG41386.png";
        string url3 = "http://pngimg.com/uploads/beach/beach_PNG86.png";
        string url4 = "http://pngimg.com/uploads/bouquet/bouquet_PNG14.png";
        string url5 = "http://pngimg.com/uploads/jellyfish/jellyfish_PNG46.png";

        string date1 = "2:00pm";
        string date2 = "10:40am";
        string date3 = "Yesterday";
        string date4 = "Tuesday";
        string date5 = "Last Week";


        StartCoroutine(FetchImages(url1, drawing1));
        StartCoroutine(FetchImages(url2, drawing2));
        StartCoroutine(FetchImages(url3, drawing3));
        StartCoroutine(FetchImages(url4, drawing4));
        StartCoroutine(FetchImages(url5, drawing5));

        textDate1.text = date1;
        textDate2.text = date2;
        textDate3.text = date3;
        textDate4.text = date4;
        textDate5.text = date5;
    }

    IEnumerator FetchImages(string url, RawImage img)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {
            img.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }
    }
}