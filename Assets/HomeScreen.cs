using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HomeScreen : MonoBehaviour
{
    public GameObject TemplatePanel;
    public GameObject BlurPanel;
    public Dictionary<string, string> allTemplates = new Dictionary<string, string>();

    // void Awake ()
    // {
    //     this.StartCoroutine(fetchUserInfo());
    // }

   	void Start ()
	{
        DestroyPanels();
        TemplatePanel.SetActive(false);
        BlurPanel.SetActive(false);

        // We only want to call the API once when the application starts
        // Not every time the scene is started
        // PlayerPrefs will be deleted every time the application is quit
        if (!PlayerPrefs.HasKey("fetchedUserInfo"))
        {
            this.StartCoroutine(fetchUserInfo());
            PlayerPrefs.SetInt("fetchedUserInfo", 1);
        } else
        {
            if (PlayerPrefs.GetInt("BREATH_COUNT") >= 10)
            {
                setUnlimitedText();
            }
            else
            {
                setBreathsLeft();
            }
            
        }
        this.StartCoroutine(fetchAllTemplates());
	}

    public void GoToShop ()
    {
        SceneManager.LoadScene("ShopScene");
    }

    public void GoToGallery ()
    {
         SceneManager.LoadScene("GalleryScene");
    }

    public void GoToDraw ()
    {
        PlayerPrefs.SetInt("isUsingTemplate", 0);
        SceneManager.LoadScene(2);
    }

    public void GoToSpirometer ()
    {
        SceneManager.LoadScene("SpirometerChallenge");
    }

    public void GoToDrawWithTemplate ()
    {
        PlayerPrefs.SetInt("isUsingTemplate", 1);
        SceneManager.LoadScene(2);
    }

    public void OpenTemplatesPanel ()
    {
        if (TemplatePanel != null)
        {
            TemplatePanel.SetActive(true);
            BlurPanel.SetActive(true);
        }
    }

    public void CloseTemplatesPanel ()
    {
        DestroyPanels();
        if (TemplatePanel != null)
        {
            TemplatePanel.SetActive(false);
            BlurPanel.SetActive(false);
        }
    }

    public void DestroyPanels ()
    {
        // destroy the child components
        GameObject[] templatePanels = GameObject.FindGameObjectsWithTag("TemplatePanel");
        foreach(GameObject panel in templatePanels)
        {
            GameObject.Destroy(panel);
        }
    }

    public IEnumerator fetchUserInfo ()
    {
        string url = "https://uql53bqfta.execute-api.us-east-1.amazonaws.com/Artsy/get-user-info";

        // Using the static constructor
        var request = UnityWebRequest.Get(url);
        request.SetRequestHeader("userId", "testUser-1605385650936673515");

        // Wait for the response and then get our data
        yield return request.SendWebRequest();
        var data = request.downloadHandler.text;

        UserInfo user = UserInfo.CreateFromJSON(data);

        // Create a string to store the list of palettes seperated by :
        string paletteString = "";
        foreach (string palette in user.paints)
        {
            if (paletteString != "")
            {
                paletteString += ":";
            }
            paletteString += palette;
        }
        PlayerPrefs.SetString("PALETTES", paletteString);

        PlayerPrefs.SetInt("COIN_BALANCE", user.coins);

        string brushString = "";
        foreach (string brush in user.brushes)
        {
            if (brushString != "")
            {
                brushString += ":";
            }
            brushString += brush;
        }
        PlayerPrefs.SetString("BRUSHES", brushString);

        PlayerPrefs.SetString("USER_ID", user.userId);

        PlayerPrefs.SetInt("UNLIMITED_STATUS", user.unlimitedExpiration);

        PlayerPrefs.SetInt("BREATH_COUNT", user.breathCount);
        setBreathsLeft();

        string ownedTemplatesString = "";
        foreach (string background in user.backgrounds)
        {
            if (ownedTemplatesString != "")
            {
                ownedTemplatesString += ":";
            }
            ownedTemplatesString += background;
        }
        PlayerPrefs.SetString("OWNED_TEMPLATES", ownedTemplatesString);
    }

    // gets all templates
    IEnumerator fetchAllTemplates ()
    {
        string url = "https://uql53bqfta.execute-api.us-east-1.amazonaws.com/Artsy/get-templates";

        // Using the static constructor
        var request = UnityWebRequest.Get(url);

        // Wait for the response and then get our data
        yield return request.SendWebRequest();
        var data = request.downloadHandler.text;

        TemplateCollection collection = TemplateCollection.CreateFromJSON(data);

        foreach(TemplateItem templateItem in collection.templates)
        {
            allTemplates[templateItem.title] = templateItem.url;
        }
        Debug.Log("Done fetching templates :)");
    }
    // sarah
    IEnumerator CreateDrawing () {
      string urlCreate = "https://uql53bqfta.execute-api.us-east-1.amazonaws.com/Artsy/create-drawing";
      List<IMultipartFormSection> formData = new List<IMultipartFormSection>();

      UnityWebRequest request = UnityWebRequest.Post(urlCreate, formData);
      request.SetRequestHeader("userId", "testUser-1605385650936673515");

      yield return request.SendWebRequest();

      var drawingId = request.downloadHandler.text;
      PlayerPrefs.SetString("DRAWING_ID", drawingId);

      if (request.isNetworkError || request.isHttpError)
      {
          Debug.Log(request.error);
      }
    }


    public void PostDrawing ()
    {
      this.StartCoroutine(CreateDrawing());
      Debug.Log("create drawing sent to back end");
    }

    public void setBreathsLeft()
    {
        GameObject unlimitedTextObject = GameObject.Find("UnlimitedText");
        unlimitedTextObject.GetComponent<Text>().text = "Breaths left to unlock unlimited Brushes and Colors:  ";
        GameObject breathsLeftObject = GameObject.Find("BreathsLeftText");
        breathsLeftObject.GetComponent<Text>().text = (10-PlayerPrefs.GetInt("BREATH_COUNT")).ToString();

    }

    public void setUnlimitedText ()
    {
        GameObject breathsLeftObject = GameObject.Find("UnlimitedText");
        breathsLeftObject.GetComponent<Text>().text = "You can use all Colors and Brushes for the Next Hour!";

        GameObject unlimitedTextObject = GameObject.Find("BreathsLeftText");
        unlimitedTextObject.GetComponent<Text>().text = "";
    }


    void OnApplicationQuit()
    {
        Debug.Log("QUITTING APPLICATION");
        PlayerPrefs.DeleteAll();
    }

}

[System.Serializable]
public class TemplateItem
{
    public string title;
    public string url;
}

[System.Serializable]
public class TemplateCollection
{
    public TemplateItem[] templates;

    public static TemplateCollection CreateFromJSON(string jsonString = "helloWorld")
    {
        return JsonUtility.FromJson<TemplateCollection>(jsonString);
    }

}
[System.Serializable]
public class UserInfo
{
    public string[] drawings;
    public int baseline;
    public string userId;
    public int coins;
    public string[] paints;
    public string[] brushes;
    public int unlimitedExpiration;
    public int breathCount;
    public string[] backgrounds;

    public static UserInfo CreateFromJSON(string jsonString = "helloWorld")
    {
        return JsonUtility.FromJson<UserInfo>(jsonString);
    }
}
