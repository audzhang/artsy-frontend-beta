using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using System.Linq;
using System.IO;

public class ShopScreen : MonoBehaviour
{
    // To create a new color Palette:
    // 1. In Unity, Click on the last "Palette (x)"
    // 2. In Unity, Copy and Paste to create a new "Palette (x + 1)" under "Palette (x)"
    // 3. In Unity, Click "PalettesContainer" and make the bottom bigger (pull down the bottom until the red x touches the bottom of the bar)
    //      This part is really finicky and strange. Unity was not built for dynamically sized scrollable lists
    //      Just do your best to make sure the spacing between Palette options looks good
    // 4. In this script, add a new list (size 5) into the end of rgbs with the each element being a list (size 3) or rgb values between (0,255)
    // 5. In this script, add the title of the palette in paletteTitles
    // 6. In this script, add the cost of the palette in paletteCosts
    private int[, ,] rgbs = new int[,,] {
        { {255, 0, 0}, {255, 255, 0}, {0, 0, 255}, {0, 255, 0}, {255, 0, 255} },
        { {38, 83, 43}, {57, 158, 90}, {90, 188, 185}, {99, 226, 198}, {110, 249, 245} },
        { {53, 80, 112}, {109, 89, 122}, {181, 101, 118}, {229, 107, 111}, {234, 172, 139} },
        { {78, 255, 239}, {115, 166, 173}, {155, 151, 178}, {216, 167, 202}, {199, 184, 234} },
        { {255, 231, 76}, {255, 89, 100}, {220, 220, 220}, {56, 97, 140}, {53, 167, 255} },
        { {239, 71, 111}, {255, 209, 102}, {6, 214, 160}, {17, 138, 178}, {7, 59, 76} },
        { {255, 190, 11}, {251, 86, 7}, {255, 0, 110}, {131, 56, 236}, {58, 134, 255} },
        { {0, 41, 107}, {0, 63, 136}, {0, 80, 157}, {253, 197, 0}, {255, 213, 0} },
        { {107, 63, 52}, {204, 156, 128}, {229, 175, 144}, {255, 195, 160}, {255, 217, 194} },
        { {0x04,0x68,0xBF}, {0x52,0xA8,0xF2}, {0xF2,0xB7,0x05}, {0xD9,0x3E,0x30}, {0xF2,0xF2,0xF2}},
        { {0x68,0x05,0xF2}, {0x05,0xF2,0xDB}, {0xF2,0xE2,0x05}, {0xF2,0xAC,0x29}, {0xD9,0xD9,0xD9}},
        { {0xD9,0x2B,0x4B}, {0x01,0x15,0x26}, {0x19,0xA6,0x14}, {0x3E,0xF2,0x38}, {0xF2,0xD6,0x49}},
        { {0xA2,0xBD,0xF2}, {0xB0,0xC1,0xD9}, {0x40,0x01,0x01}, {0x73,0x26,0x26}, {0x8C,0x58,0x58}},
        { {0x44,0x59,0x36}, {0x81,0x8C,0x58}, {0xBE,0xBF,0x99}, {0xBF,0xBF,0xBD}, {0xF2,0xF2,0xF2}},
        { {0x95,0x58,0xA6}, {0x37,0x4C,0x8C}, {0x19,0x59,0x53}, {0xD9,0x8E,0x04}, {0xD9,0x50,0x32}},
        { {0xF2,0x6B,0x76}, {0xF2,0x50,0x7B}, {0xF2,0x50,0x7B}, {0x35,0x8C,0x54}, {0xA1,0xBF,0x34}},
        { {0xF2,0xA6,0x63}, {0xF2,0x95,0x5E}, {0xF2,0x74,0x57}, {0xF2,0x60,0x52}, {0xA6,0x44,0x44}}
    };

    private string[] paletteTitles = {
        "Basic",
        "Mystic Sea",
        "Sunset",
        "Fairy Tale",
        "Skater Boy",
        "Summer",
        "Bold, Brave",
        "Go Blue",
        "Skin Tones",
        "Vintage",
        "Warm Glow",
        "Northern Lights",
        "Caspian Sea",
        "Midnight Forest",
        "Dark Rain",
        "Princess and the Frog",
        "Sunset",
    };


    private string[] paletteCosts = {
        "0",
        "200",
        "200",
        "200",
        "200",
        "200",
        "200",
        "50",
        "200",
        "200",
        "200",
        "200",
        "200",
        "200",
        "200",
        "200",
        "200"
    };

    private string[] brushTitles =
    {
        "Basic",
        "Acrylic",
        "Aurora",
        "Charcoal",
        "Faded",
        "Fresco",
        "Sharpened",
        "Snow",
        "Tarkine",
        "Thylacine",
        "Watercolor",
    };

    private string[] brushCosts =
    {
        "200",
        "200",
        "200",
        "200",
        "200",
        "200",
        "200",
        "200",
        "200",
        "200",
        "200",
    };

    public GameObject prefab; // our prefab object
    public Dictionary<string, string> allTemplates = new Dictionary<string, string>();


    // Don't touch
    private string[] colorLabels = {"Color1", "Color2", "Color3", "Color4", "Color5"};

    void Start()
    {
        this.StartCoroutine(fetchTemplates());
        setCoinBalance();
        setPalettes();
        setBrushes();
    }

    /*
    ############################################################
    ########################### COINS ##########################
    ############################################################
    */

    private void setCoinBalance ()
    {
        if (PlayerPrefs.HasKey("COIN_BALANCE"))
        {
            int coinBalance = PlayerPrefs.GetInt("COIN_BALANCE");
            GameObject coinBalanceObject = GameObject.Find("AddCoinsButton/CoinBalance");
            coinBalanceObject.GetComponent<Text>().text = coinBalance.ToString();
        }
    }

    /*
    ############################################################
    ######################### PALETTES #########################
    ############################################################
    */

    private void setPalettes ()
    {
        ArrayList ownedPalettes = new ArrayList();

        // // Add any palettes owned to the owned palettes array
        if (PlayerPrefs.HasKey("PALETTES"))
        {
            string paletteString = PlayerPrefs.GetString("PALETTES");
            // Palettes are stored as a string seperated by a : in PlayerPrefs
            string[] palettesOwned = paletteString.Split(':');
            foreach (string palette in palettesOwned)
            {
                int paletteNum = Int32.Parse(palette);
                ownedPalettes.Add(paletteNum);
            }
        }

        GameObject[] palettes = GameObject.FindGameObjectsWithTag("ColorPalette");

        for (int paletteIdx = 0; paletteIdx < palettes.Length; paletteIdx++)
        {
            // Set the title of the palette
            GameObject title = GameObject.Find(palettes[paletteIdx].name + "/Title");
            title.GetComponent<Text>().text = paletteTitles[paletteIdx];

            GameObject cost = GameObject.Find(palettes[paletteIdx].name + "/Cost");

            // If the palette is already owned
            if (ownedPalettes.Contains(paletteIdx))
            {
                // Update coin balance to show owned
                cost.GetComponent<Text>().text = "Owned";

                // Get the check mark png
                var checkMark = Resources.Load<Texture2D>("Images/CheckMark");
                
                // Update the coin icon to now show a check mark
                GameObject coinIcon = GameObject.Find(palettes[paletteIdx].name + "/CoinIcon");
                coinIcon.GetComponent<RawImage>().texture = checkMark;

                // Disable clickable palette
                palettes[paletteIdx].GetComponent<Button>().interactable = false;
            } else {
                // If the palette is not owned, set the price
                cost.GetComponent<Text>().text = paletteCosts[paletteIdx];
            }


            for (int j = 0; j < 5; j++)
            {
                GameObject color = GameObject.Find(palettes[paletteIdx].name + "/Colors/" + colorLabels[j]);
                Color32 c32 = new Color32(0,0,0,255);
                c32[0] = (byte)rgbs[paletteIdx, j, 0];
                c32[1] = (byte)rgbs[paletteIdx, j, 1];
                c32[2] = (byte)rgbs[paletteIdx, j, 2];
                color.GetComponent<Image>().color = c32;
            } 
        }
    }

    public void paletteClicked()
    {
        // Get the index of the palette that was just clicked
        // It will be the number between the parenthesis
        string name = EventSystem.current.currentSelectedGameObject.name;
        int from = name.IndexOf("(") + 1;
        int to = name.IndexOf(")");
        int idx = int.Parse(name.Substring(from, to - from));

        // Set the colors of the popup
        for (int i = 0; i < 5; ++i)
        {
            GameObject color = GameObject.Find("PopUpPanelPalettes/Panel/Colors/" + colorLabels[i]);
            Color32 c32 = new Color32(0,0,0,255);
            c32[0] = (byte)rgbs[idx, i, 0];
            c32[1] = (byte)rgbs[idx, i, 1];
            c32[2] = (byte)rgbs[idx, i, 2];
            color.GetComponent<Image>().color = c32;
        }

        // Set the price of the popup
        GameObject priceObject = GameObject.Find("PopUpPanelPalettes/Panel/Cost");
        priceObject.GetComponent<Text>().text = paletteCosts[idx];

        // Set the title of the popup
        GameObject title = GameObject.Find("PopUpPanelPalettes/Panel/Title");
        title.GetComponent<Text>().text = paletteTitles[idx];

        // If the user does not have enough coins, purchased button should be grey and not clickable
        GameObject coinBalanceObject = GameObject.Find("AddCoinsButton/CoinBalance");
        int coinBalance = Int32.Parse(coinBalanceObject.GetComponent<Text>().text);
        int price = Int32.Parse(priceObject.GetComponent<Text>().text);

        GameObject purchaseObject = GameObject.Find("PopUpPanelPalettes/Purchase");
        GameObject purchaseTextObject = GameObject.Find("PopUpPanelPalettes/Purchase/Text");
        
        if (coinBalance < price)
        {
            var cantPurchaseImage = Resources.Load<Sprite>("Images/CantPurchase");
            purchaseObject.GetComponent<Image>().sprite = cantPurchaseImage;
            purchaseObject.GetComponent<Button>().interactable = false;
            purchaseTextObject.GetComponent<Text>().text = "Need Coins";
        } 
        else
        {
            var confirmPurchaseImage = Resources.Load<Sprite>("Images/Confirm");
            purchaseObject.GetComponent<Image>().sprite = confirmPurchaseImage;
            purchaseObject.GetComponent<Button>().interactable = true;
            purchaseTextObject.GetComponent<Text>().text = "Purchase";
        }

        // Need Palette Index saved in player prefs so purchasePalette knows which palette is purchased
        PlayerPrefs.SetInt("PURCHASED_PALETTE_IDX", idx);
    }

    // Cannot be called unless the user has enough coins
    public void PurchasePalette ()
    {
        StartCoroutine(SendPaletteToBackend());
        int paletteIdx = PlayerPrefs.GetInt("PURCHASED_PALETTE_IDX");

        // Add palette to owned palettes
        string ownedPalettesString = PlayerPrefs.GetString("PALETTES");
        ownedPalettesString += ":" + paletteIdx.ToString();
        PlayerPrefs.SetString("PALETTES", ownedPalettesString);
        setPalettes();

        // Update Coin Balance
        int coinBalance = PlayerPrefs.GetInt("COIN_BALANCE");
        coinBalance -= Int32.Parse(paletteCosts[paletteIdx]);
        PlayerPrefs.SetInt("COIN_BALANCE", coinBalance);
        setCoinBalance();
    }

    IEnumerator SendPaletteToBackend ()
    {
        int paletteIdx = PlayerPrefs.GetInt("PURCHASED_PALETTE_IDX");
        string url = "https://uql53bqfta.execute-api.us-east-1.amazonaws.com/Artsy/purchase-palette";

        // TODO: Waiting on backend to change Headers to Form Data
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        
        UnityWebRequest request = UnityWebRequest.Post(url, formData);
        request.SetRequestHeader("userId", "testUser-1605385650936673515");
        request.SetRequestHeader("paintId", paletteIdx.ToString());
        request.SetRequestHeader("cost", paletteCosts[paletteIdx]);
        
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }       
    }

    public void CancelPurchasePalette ()
    {
        PlayerPrefs.DeleteKey("PURCHASED_PALETTE_IDX");
    }

    /*
    ############################################################
    ######################### BRUSHES #########################
    ############################################################
    */

    private void setBrushes ()
    {
        ArrayList ownedBrushes = new ArrayList();

        // // Add any palettes owned to the owned palettes array
        if (PlayerPrefs.HasKey("BRUSHES"))
        {
            string brushesString = PlayerPrefs.GetString("BRUSHES");
            // Palettes are stored as a string seperated by a : in PlayerPrefs
            string[] brushesOwned = brushesString.Split(':');
            foreach (string brush in brushesOwned)
            {
                ownedBrushes.Add(brush);
            }
        }

        GameObject[] brushPanels = GameObject.FindGameObjectsWithTag("BrushPanel");

        for (int brushIdx = 0; brushIdx < brushPanels.Length; brushIdx++)
        {
            // Set the title of the palette
            GameObject title = GameObject.Find(brushPanels[brushIdx].name + "/Title");
            title.GetComponent<Text>().text = brushTitles[brushIdx];

            GameObject cost = GameObject.Find(brushPanels[brushIdx].name + "/Cost");

            // If the brush is already owned
            if (ownedBrushes.Contains(brushTitles[brushIdx])) // TODO hmm
            {
                // Update coin balance to show owned
                cost.GetComponent<Text>().text = "Owned";

                // Get the check mark png
                var checkMark = Resources.Load<Texture2D>("Images/CheckMark");
                
                // Update the coin icon to now show a check mark
                GameObject coinIcon = GameObject.Find(brushPanels[brushIdx].name + "/CoinIcon");
                coinIcon.GetComponent<RawImage>().texture = checkMark;

                // Disable clickable palette
                brushPanels[brushIdx].GetComponent<Button>().interactable = false;
            } else {
                // If the palette is not owned, set the price
                cost.GetComponent<Text>().text = brushCosts[brushIdx];
            }

            GameObject brushImageObject = GameObject.Find(brushPanels[brushIdx].name + "/Brush");
            var brushImage = Resources.Load<Sprite>("TextureExamples/" + brushTitles[brushIdx]);
            brushImageObject.GetComponent<Image>().sprite = brushImage;
        }
    }

    public void brushClicked()
    {
        // Get the index of the palette that was just clicked
        // It will be the number between the parenthesis
        string name = EventSystem.current.currentSelectedGameObject.name;
        int from = name.IndexOf("(") + 1;
        int to = name.IndexOf(")");
        int idx = int.Parse(name.Substring(from, to - from));

        GameObject brushImageObject = GameObject.Find("PopUpPanelBrushes/Panel/Brush");
        var brushImage = Resources.Load<Sprite>("TextureExamples/" + brushTitles[idx]);
        brushImageObject.GetComponent<Image>().sprite = brushImage;

        // Set the price of the popup
        GameObject priceObject = GameObject.Find("PopUpPanelBrushes/Panel/Cost");
        priceObject.GetComponent<Text>().text = brushCosts[idx];

        // Set the title of the popup
        GameObject title = GameObject.Find("PopUpPanelBrushes/Panel/Title");
        title.GetComponent<Text>().text = brushTitles[idx];

        // If the user does not have enough coins, purchased button should be grey and not clickable
        GameObject coinBalanceObject = GameObject.Find("AddCoinsButton/CoinBalance");
        int coinBalance = Int32.Parse(coinBalanceObject.GetComponent<Text>().text);
        int price = Int32.Parse(priceObject.GetComponent<Text>().text);

        GameObject purchaseObject = GameObject.Find("PopUpPanelBrushes/Purchase");
        GameObject purchaseTextObject = GameObject.Find("PopUpPanelBrushes/Purchase/Text");
        
        if (coinBalance < price)
        {
            var cantPurchaseImage = Resources.Load<Sprite>("Images/CantPurchase");
            purchaseObject.GetComponent<Image>().sprite = cantPurchaseImage;
            purchaseObject.GetComponent<Button>().interactable = false;
            purchaseTextObject.GetComponent<Text>().text = "Need Coins";
        } 
        else
        {
            var confirmPurchaseImage = Resources.Load<Sprite>("Images/Confirm");
            purchaseObject.GetComponent<Image>().sprite = confirmPurchaseImage;
            purchaseObject.GetComponent<Button>().interactable = true;
            purchaseTextObject.GetComponent<Text>().text = "Purchase";
        }

        // Need Palette Index saved in player prefs so purchasePalette knows which palette is purchased
        PlayerPrefs.SetInt("PURCHASED_BRUSH_IDX", idx);
    }

    // Cannot be called unless the user has enough coins
    public void PurchaseBrush ()
    {
        StartCoroutine(SendBrushToBackend());
        int brushIdx = PlayerPrefs.GetInt("PURCHASED_BRUSH_IDX");

        // Add palette to owned palettes
        string ownedBrushesString = PlayerPrefs.GetString("BRUSHES");
        ownedBrushesString += ":" + brushTitles[brushIdx];
        PlayerPrefs.SetString("BRUSHES", ownedBrushesString);
        setBrushes();

        // Update Coin Balance
        int coinBalance = PlayerPrefs.GetInt("COIN_BALANCE");
        coinBalance -= Int32.Parse(brushCosts[brushIdx]);
        PlayerPrefs.SetInt("COIN_BALANCE", coinBalance);
        setCoinBalance(); 
    }

    IEnumerator SendBrushToBackend ()
    {
        int brushIdx = PlayerPrefs.GetInt("PURCHASED_BRUSH_IDX");
        string url = "https://uql53bqfta.execute-api.us-east-1.amazonaws.com/Artsy/purchase-brush";

        // TODO: Waiting on backend to change Headers to Form Data
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        
        UnityWebRequest request = UnityWebRequest.Post(url, formData);
        request.SetRequestHeader("userId", "testUser-1605385650936673515");
        request.SetRequestHeader("brushId", brushTitles[brushIdx]);
        request.SetRequestHeader("cost", brushCosts[brushIdx]);
        
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }       
    }

    public void CancelPurchaseBrush ()
    {
        PlayerPrefs.DeleteKey("PURCHASED_BRUSH_IDX");
    }

    /*
    ############################################################
    ######################## TEMPLATES #########################
    ############################################################
    */

     // gets all templates
    private IEnumerator fetchTemplates ()
    {
        Debug.Log("FETCHING TEMPLATES");
        string url = "https://uql53bqfta.execute-api.us-east-1.amazonaws.com/Artsy/get-templates";

        // Using the static constructor
        var request = UnityWebRequest.Get(url);

        // Wait for the response and then get our data
        yield return request.SendWebRequest();
        var data = request.downloadHandler.text;

        TemplateCollection collection = TemplateCollection.CreateFromJSON(data);

        foreach(TemplateItem templateItem in collection.templates)
        {
            // templateTitles.Add(templateItem.title);
            // templates.Add(templateItem.url);
            allTemplates[templateItem.title] = templateItem.url;
        }
        setTemplates();
    }

    private void setTemplates ()
    {
        // creating UI grid elements
        GameObject newObj; // create gameobject instance
        GameObject contentObj = GameObject.Find("TemplateContainer");
        foreach(KeyValuePair<string,string> template in allTemplates)
        {
            newObj = (GameObject)Instantiate(prefab, transform); // create new instances of prefab
            newObj.transform.SetParent(contentObj.transform);
            // setting the name of the button so we have some way to reference the url of the button that was pressed :')
            newObj.transform.GetChild(0).name = template.Key;

            // Add onClick to button
            // Button button = newObj.transform.GetChild(0).GetComponent<Button>();
            // button.onClick.AddListener(delegate {
            //     string templateURL = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
            //     LoadCanvasSceneWithTemplate(templateURL);
            // });

            // Set the title of the template
            Text title = newObj.transform.GetChild(1).GetComponent<Text>();
            title.text = template.Key;

            // Set the image of the template
            RawImage image = newObj.transform.GetChild(0).GetChild(0).GetComponent<RawImage>();
            string url = template.Value;
            if(image!=null)
            {
                StartCoroutine(FetchImages(template.Value, image));
            }
        }
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

    /*
    ############################################################
    ####################### NAVIGATION #########################
    ############################################################
    */

    public void GoToHome ()
    {
        SceneManager.LoadScene(0);
    }

    public void GoToSpirometer ()
    {
        SceneManager.LoadScene("SpirometerChallenge");
    }

    public void OnApplicationQuit()
    {
        Debug.Log("QUITTING APPLICATION");
        PlayerPrefs.DeleteAll();
    }
}
