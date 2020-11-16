using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class fetchRecentTemplates : MonoBehaviour
{
    public RawImage template1;
    public RawImage template2;
    public RawImage template3;
    public RawImage template4;
    public RawImage template5;

    public Text textDate1;
    public Text textDate2;
    public Text textDate3;
    public Text textDate4;
    public Text textDate5;

    // Start is called before the first frame update
    void Start()
    {
        string url1 = "https://www.pngitem.com/pimgs/m/105-1057887_realistic-dragon-coloring-pages-for-adults-coloring-free.png";
        string url2 = "https://www.pngitem.com/pimgs/m/516-5165271_printable-sewing-coloring-pages-hd-png-download.png";
        string url3 = "https://www.pngitem.com/pimgs/m/516-5169784_beach-scene-coloring-pages-for-kids-beach-coloring.png";
        string url4 = "https://www.pngitem.com/pimgs/m/203-2037856_fairy-princess-coloring-pages-princess-coloring-pages-png.png";
        string url5 = "https://www.pngitem.com/pimgs/m/332-3328236_rosalina-and-luma-coloring-pages-princess-rosalina-and.png";

        string date1 = "9:00pm";
        string date2 = "8:30am";
        string date3 = "Monday";
        string date4 = "Last Week";
        string date5 = "2 Weeks Ago";

        StartCoroutine(FetchImages(url1, template1));
        StartCoroutine(FetchImages(url2, template2));
        StartCoroutine(FetchImages(url3, template3));
        StartCoroutine(FetchImages(url4, template4));
        StartCoroutine(FetchImages(url5, template5));

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