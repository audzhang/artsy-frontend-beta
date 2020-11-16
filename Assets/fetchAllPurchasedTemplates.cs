using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// grid layout and scroll window: https://www.youtube.com/watch?v=VyIo5tlNNeA

public class fetchAllPurchasedTemplates : MonoBehaviour
{
    public RawImage template1;
    public GameObject prefab; // our prefab object

    public Dictionary<string, string> allTemplates = new Dictionary<string, string>();

    public void setTemplates()
    {
        Debug.Log("setting templates");
        GameObject tempObj = GameObject.Find("HomeScreen");
        HomeScreen homeScreen = tempObj.GetComponent<HomeScreen>();
        allTemplates = homeScreen.allTemplates;
        // Add any templates owned to the owned templates array
        string[] templatesOwned = {};
        if (PlayerPrefs.HasKey("OWNED_TEMPLATES"))
        {
            string ownedTemplatesString = PlayerPrefs.GetString("OWNED_TEMPLATES");
            // Templates are stored as a string seperated by a : in PlayerPrefs
            templatesOwned = ownedTemplatesString.Split(':');
        }

        List<string> templates = new List<string>();
        List<string> templateTitles = new List<string>();
        foreach(string template in templatesOwned)
        {
            if(allTemplates.ContainsKey(template))
            {
                templateTitles.Add(template);
                templates.Add(allTemplates[template]);
            }
        }

        // creating UI grid elements
        GameObject newObj; // create gameobject instance
        for (int templateIdx = 0; templateIdx < templates.Count; templateIdx++)
        {
            newObj = (GameObject)Instantiate(prefab, transform); // create new instances of prefab
            // i swear to god this is the dumbest way to do this...
            // setting the name of the button so we have some way to reference the url of the button that was pressed :')
            newObj.transform.GetChild(0).name = templates[templateIdx];

            // Add onClick to button
            Button button = newObj.transform.GetChild(0).GetComponent<Button>();
            button.onClick.AddListener(delegate {
                string templateURL = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
                LoadCanvasSceneWithTemplate(templateURL);
            });

            // Set the title of the template
            Text title = newObj.transform.GetChild(1).GetComponent<Text>();
            title.text = templateTitles[templateIdx];

            // Set the image of the template
            RawImage image = newObj.transform.GetChild(0).GetChild(0).GetComponent<RawImage>();
            string url = templates[templateIdx];
            if(image!=null)
            {
                StartCoroutine(FetchImages(url, image));
            }

        }
    }

    public void LoadCanvasSceneWithTemplate(string templateURL)
    {
        PlayerPrefs.SetString("CURRENT_TEMPLATE_URL", templateURL);
        PlayerPrefs.SetInt("IS_USING_TEMPLATE", 1);
        SceneManager.LoadScene(2);
        // here is where you would call /create-drawing with the backgroundId of the template used
        // then also send the backgroundId to the drawing scene somehow so it knows to load in that template
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
