using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;
using SimpleJSON;

// REST API Get Request in Unity: https://www.youtube.com/watch?v=GIxu8kA9EBU

public class GalleryManager : MonoBehaviour
{   
    private string getGalleryURL = "https://uql53bqfta.execute-api.us-east-1.amazonaws.com/Artsy/get-gallery";

    private List<string> originalTitles = new List<string>();
    private List<string> originialURLs = new List<string>();
    // private List<string> templateTitles = new List<string>();
    // private List<string> templateURLs = new List<string>();

    public GameObject painting;
    public GameObject originialContent;
    public GameObject templateContent;

    // Hardcoded urls

    // private string[] originals = 
    // {
    //     "http://pngimg.com/uploads/beach/beach_PNG96.png",
    //     "http://pngimg.com/uploads/arctic_fox/arctic_fox_PNG41386.png",
    //     "http://pngimg.com/uploads/beach/beach_PNG86.png",
    //     "http://pngimg.com/uploads/bouquet/bouquet_PNG14.png",
    //     "http://pngimg.com/uploads/jellyfish/jellyfish_PNG46.png",
    //     "http://pngimg.com/uploads/beach/beach_PNG96.png",
    //     "http://pngimg.com/uploads/arctic_fox/arctic_fox_PNG41386.png",
    //     "http://pngimg.com/uploads/beach/beach_PNG86.png",
    //     "http://pngimg.com/uploads/bouquet/bouquet_PNG14.png",
    //     "http://pngimg.com/uploads/jellyfish/jellyfish_PNG46.png",
    //     "http://pngimg.com/uploads/beach/beach_PNG96.png",
    //     "http://pngimg.com/uploads/arctic_fox/arctic_fox_PNG41386.png",
    //     "http://pngimg.com/uploads/beach/beach_PNG86.png",
    //     "http://pngimg.com/uploads/bouquet/bouquet_PNG14.png",
    //     "http://pngimg.com/uploads/jellyfish/jellyfish_PNG46.png"
    // };
    // private string[] originalTitles = 
    // {
    //     "Pikachu", "Pikachu and Pichu", "Dragon", "Sewing Basket", "The Beach", "Fairy Princess", "Rosalina and Luma",
    //     "Pikachu", "Pikachu and Pichu", "Dragon", "Sewing Basket", "The Beach", "Fairy Princess", "Rosalina and Luma",
    //     "Pikachu", "Pikachu and Pichu", "Dragon", "Sewing Basket", "The Beach", "Fairy Princess", "Rosalina and Luma"
    // };
                        
    private string[] templates = 
    {   
        "https://i.imgur.com/f8HiZdo.png",
        "https://i.imgur.com/l5cdz0q.png",
        "https://www.pngitem.com/pimgs/m/105-1057887_realistic-dragon-coloring-pages-for-adults-coloring-free.png", 
        "https://www.pngitem.com/pimgs/m/516-5165271_printable-sewing-coloring-pages-hd-png-download.png", 
        "https://www.pngitem.com/pimgs/m/516-5169784_beach-scene-coloring-pages-for-kids-beach-coloring.png",
        "https://www.pngitem.com/pimgs/m/203-2037856_fairy-princess-coloring-pages-princess-coloring-pages-png.png",
        "https://www.pngitem.com/pimgs/m/332-3328236_rosalina-and-luma-coloring-pages-princess-rosalina-and.png",
        "https://i.imgur.com/f8HiZdo.png",
        "https://i.imgur.com/l5cdz0q.png",
        "https://www.pngitem.com/pimgs/m/105-1057887_realistic-dragon-coloring-pages-for-adults-coloring-free.png", 
        "https://www.pngitem.com/pimgs/m/516-5165271_printable-sewing-coloring-pages-hd-png-download.png", 
        "https://www.pngitem.com/pimgs/m/516-5169784_beach-scene-coloring-pages-for-kids-beach-coloring.png",
        "https://www.pngitem.com/pimgs/m/203-2037856_fairy-princess-coloring-pages-princess-coloring-pages-png.png",
        "https://www.pngitem.com/pimgs/m/332-3328236_rosalina-and-luma-coloring-pages-princess-rosalina-and.png",
        "https://i.imgur.com/f8HiZdo.png",
        "https://i.imgur.com/l5cdz0q.png",
        "https://www.pngitem.com/pimgs/m/105-1057887_realistic-dragon-coloring-pages-for-adults-coloring-free.png", 
        "https://www.pngitem.com/pimgs/m/516-5165271_printable-sewing-coloring-pages-hd-png-download.png", 
        "https://www.pngitem.com/pimgs/m/516-5169784_beach-scene-coloring-pages-for-kids-beach-coloring.png",
        "https://www.pngitem.com/pimgs/m/203-2037856_fairy-princess-coloring-pages-princess-coloring-pages-png.png",
        "https://www.pngitem.com/pimgs/m/332-3328236_rosalina-and-luma-coloring-pages-princess-rosalina-and.png"
    };
    private string[] templateTitles = 
    {
        "Pikachu", "Pikachu and Pichu", "Dragon", "Sewing Basket", "The Beach", "Fairy Princess", "Rosalina and Luma",
        "Pikachu", "Pikachu and Pichu", "Dragon", "Sewing Basket", "The Beach", "Fairy Princess", "Rosalina and Luma",
        "Pikachu", "Pikachu and Pichu", "Dragon", "Sewing Basket", "The Beach", "Fairy Princess", "Rosalina and Luma"
    };

    void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetGalleryData(getGalleryURL));
        // Debug.Log("now orginials are: " + originialURLs.Count);
        // GameObject newObj;
        // for (int i = 0; i < originialURLs.Count; i++)
        // {
        //     newObj = (GameObject)Instantiate(painting, transform); // create new instances of prefab 
        //     newObj.transform.parent = originialContent.transform;
        //     // i swear to god this is the dumbest way to do this...
        //     // setting the name of the button so we have some way to reference which button was pressed :')
        //     // i assume i will need to do this in Omega hahaha - xuxf
        //     // newObj.transform.GetChild(0).name = i.ToString();

        //     // Add onClick to button
        //     // Button button = newObj.transform.GetChild(0).GetComponent<Button>();
        //     // button.onClick.AddListener(delegate { 
        //     //     string idxString = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        //     //     int idx = Int32.Parse(idxString);
        //     //     LoadCanvasSceneWithTemplate(idx); 
        //     // });
            
        //     Text title = newObj.transform.GetChild(1).GetComponent<Text>();
        //     title.text = originalTitles[i];

        //     RawImage image = newObj.transform.GetChild(0).GetChild(0).GetComponent<RawImage>();
        //     string url = originialURLs[i];
        //     if(image != null)
        //     {
        //         StartCoroutine(FetchImages(url, image));
        //     }
        // }
        GameObject newObj;
        for (int templateIdx = 0; templateIdx < templates.Length; templateIdx++)
        {
            newObj = (GameObject)Instantiate(painting, transform); // create new instances of prefab
            newObj.transform.parent = templateContent.transform; 
            // i swear to god this is the dumbest way to do this...
            // setting the name of the button so we have some way to reference which button was pressed :')
            // i assume i will need to do this in Omega hahaha - xuxf
            // newObj.transform.GetChild(0).name = templateIdx.ToString();

            // Add onClick to button
            // Button button = newObj.transform.GetChild(0).GetComponent<Button>();
            // button.onClick.AddListener(delegate { 
            //     string idxString = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
            //     int idx = Int32.Parse(idxString);
            //     LoadCanvasSceneWithTemplate(idx); 
            // });
            
            Text title = newObj.transform.GetChild(1).GetComponent<Text>();
            title.text = templateTitles[templateIdx];

            RawImage image = newObj.transform.GetChild(0).GetChild(0).GetComponent<RawImage>();
            string url = templates[templateIdx];
            if(image != null)
            {
                StartCoroutine(FetchImages(url, image));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToHome ()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void GoToSpirometer() 
    {
        SceneManager.LoadScene("SpirometerChallenge");
    }

    IEnumerator GetGalleryData(string getGalleryURL)
    {
        UnityWebRequest galleryDataRequest = UnityWebRequest.Get(getGalleryURL);
        yield return galleryDataRequest.SendWebRequest();

        if(galleryDataRequest.isNetworkError || galleryDataRequest.isHttpError)
        {
            Debug.Log(galleryDataRequest.error);
            yield break;
        }

        JSONNode galleryData = JSON.Parse(galleryDataRequest.downloadHandler.text);
        JSONNode originals = galleryData["canvases"];
        // JSONNode templates = galleryData["templates"];

        for(int i = 0; i < originals.Count; i++)
        {
            originalTitles.Add(originals[i]["title"]);
            originialURLs.Add(originals[i]["imageUrl"]);
            // Debug.Log(i + originals[i]["title"].Value + originals[i]["imageUrl"].Value);
        }
        // for(int i = 0; i < templates.Count; i++)
        // {
        //     templateTitles.Add(templates[i]["title"]);
        //     templateURLs.Add(templates[i]["imageUrl"]);
        // }
        Debug.Log("now orginials are: " + originialURLs.Count);
        GameObject newObj;
        for (int i = 0; i < originialURLs.Count; i++)
        {
            newObj = (GameObject)Instantiate(painting, transform); // create new instances of prefab 
            newObj.transform.parent = originialContent.transform;
            // i swear to god this is the dumbest way to do this...
            // setting the name of the button so we have some way to reference which button was pressed :')
            // i assume i will need to do this in Omega hahaha - xuxf
            // newObj.transform.GetChild(0).name = i.ToString();

            // Add onClick to button
            // Button button = newObj.transform.GetChild(0).GetComponent<Button>();
            // button.onClick.AddListener(delegate { 
            //     string idxString = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
            //     int idx = Int32.Parse(idxString);
            //     LoadCanvasSceneWithTemplate(idx); 
            // });
            
            Text title = newObj.transform.GetChild(1).GetComponent<Text>();
            title.text = originalTitles[i];

            RawImage image = newObj.transform.GetChild(0).GetChild(0).GetComponent<RawImage>();
            string url = originialURLs[i];
            if(image != null)
            {
                StartCoroutine(FetchImages(url, image));
            }
        }
    }

    IEnumerator FetchImages(string url, RawImage img)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if(www.isNetworkError || www.isHttpError) 
        {
            Debug.Log(www.error);
            yield break;
        }
        
        img.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
    }
}
