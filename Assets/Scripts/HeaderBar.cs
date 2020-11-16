using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class HeaderBar : MonoBehaviour
{
    private int userStatus = 1;

    // Color schemes from: https://color.adobe.com/zh/trends/Illustration
    private int[, ,] palletes = 
    {
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

    public GameObject RenamePanel;
    public GameObject ColorPanel;
    public GameObject BrushPanel;

    public GameObject Pallete;
    public GameObject Brush;

    public GameObject ColorSection;
    public GameObject TextureSection;

    public DrawManager drawManager;

	void Start()
	{
        RenamePanel.SetActive(false);
		ColorPanel.SetActive(false);
        BrushPanel.SetActive(false);

        userStatus = PlayerPrefs.GetInt("UNLIMITED_STATUS");
        GenerateBrushPanel();
        GenerateColorPanel();
	}

    void Update()
    {
        HideIfClickedOutside(RenamePanel);
        HideIfClickedOutside(ColorPanel);
        HideIfClickedOutside(BrushPanel);

        // if(PlayerPrefs.GetInt("UNLIMITED_STATUS") != userStatus)
        // {
        //     userStatus = PlayerPrefs.GetInt("UNLIMITED_STATUS");
        //     GenerateColorPanel();
        //     GenerateBrushPanel();
        // }
    }
    
    public void GoToHome() 
    {
        SceneManager.LoadScene(0);
    }

    public void OpenRenamePanel() 
    {
        if (RenamePanel != null)
        {
            bool isActive = RenamePanel.activeSelf;
            RenamePanel.SetActive(!isActive);
            ColorPanel.SetActive(false);
            BrushPanel.SetActive(false);
        }
    }

    public void OpenColorPanel()
    {
        if (ColorPanel != null)
        {
            bool isActive = ColorPanel.activeSelf;
            ColorPanel.SetActive(!isActive);
            RenamePanel.SetActive(false);
            BrushPanel.SetActive(false);
        }
     }
 
    public void OpenBrushPanel()
    {
        if (BrushPanel != null)
        {
            bool isActive = BrushPanel.activeSelf;
            BrushPanel.SetActive(!isActive);
            RenamePanel.SetActive(false);
            ColorPanel.SetActive(false);
        }
     }

    public void HideIfClickedOutside(GameObject panel) 
    {
        if (Input.GetMouseButton(0) && panel.activeSelf && 
            !RectTransformUtility.RectangleContainsScreenPoint(
                panel.GetComponent<RectTransform>(), 
                Input.mousePosition, 
                Camera.main)) {
            panel.SetActive(false);
        }
    }

    public void GenerateBrushPanel()
    {
        GameObject newObj;

        if(userStatus == 1)
        {
            Debug.Log("USER STATUS IS 1");
            for (int i = 0; i < brushTitles.Length; i++)
            {
                newObj = (GameObject)Instantiate(Brush, transform);
                newObj.transform.SetParent(TextureSection.transform);

                Text title = newObj.transform.GetChild(0).GetComponent<Text>();
                title.text = brushTitles[i];
                
                // Add onClick to button
                Button button = newObj.transform.GetChild(1).GetComponent<Button>();
                string path = "TextureExamples/" + brushTitles[i];
                string para = brushTitles[i];
                button.GetComponent<Image>().sprite = Resources.Load<Sprite>(path);
                button.onClick.AddListener(()=>HandleBrushSelect(para));
            }
        }
        else
        {
            Debug.Log("USER STATUS IS 0");
            // BRUSHES: string (brush names separated by “:”) example: “Basic:Fresco”
            string[] brushesOwned = PlayerPrefs.GetString("BRUSHES").Split(':');
            foreach (string brush in brushesOwned)
            {
                newObj = (GameObject)Instantiate(Brush, transform);
                newObj.transform.parent = TextureSection.transform;

                Text title = newObj.transform.GetChild(0).GetComponent<Text>();
                title.text = brush;

                // Add onClick to button
                Button button = newObj.transform.GetChild(1).GetComponent<Button>();
                string path = "TextureExamples/" + brush;
                string para = brush;
                button.GetComponent<Image>().sprite = Resources.Load<Sprite>(path);
                button.onClick.AddListener(()=>HandleBrushSelect(brush));
            }
        }
    }

    public void HandleBrushSelect(string brushTitle)
    {
        drawManager.SelectTexture(brushTitle);
        // Debug.Log("Brush Selection Clicked");
    }

    public void GenerateColorPanel()
    {
        GameObject newObj;
        /*
            Pallete 
              MaskCircle
                Button
              MaskCircle
                Button
              MaskCircle
                Button
              MaskCircle
                Button
              MaskCircle
                Button
        */
        if(userStatus == 1)
        {   
            // Debug.Log(palletes.Length);
            for (int i = 0; i < palletes.GetLength(0); i++)
            {
                newObj = (GameObject)Instantiate(Pallete, transform);
                newObj.transform.parent = ColorSection.transform;

                for(int j = 0; j < 5; j++)
                {
                    Color32 color = new Color32(255, 0, 0, 255);
                    color[0] = (byte)palletes[i, j, 0];
                    color[1] = (byte)palletes[i, j, 1];
                    color[2] = (byte)palletes[i, j, 2];
                    
                    Button button = newObj.transform.GetChild(j).GetChild(0).GetComponent<Button>();
                    button.GetComponent<Image>().color = color;
                    button.onClick.AddListener(()=>HandleColorSelect(color));
                }
            }
        }
        else
        {
            // PALETTES: string (ints separated by “:”) example: “2:3:6”
            // string[] palletesOwned = {"2", "3", "6"};
            string[] palletesOwned = PlayerPrefs.GetString("PALETTES").Split(':');
            foreach (string pallete in palletesOwned)
            {
                int palleteNum = Int32.Parse(pallete);

                newObj = (GameObject)Instantiate(Pallete, transform);
                newObj.transform.parent = ColorSection.transform;

                for(int j = 0; j < 5; j++)
                {
                    Color32 color = new Color32(255, 0, 0, 255);
                    color[0] = (byte)palletes[palleteNum, j, 0];
                    color[1] = (byte)palletes[palleteNum, j, 1];
                    color[2] = (byte)palletes[palleteNum, j, 2];
                    
                    Button button = newObj.transform.GetChild(j).GetChild(0).GetComponent<Button>();
                    button.GetComponent<Image>().color = color;
                    button.onClick.AddListener(()=>HandleColorSelect(color));
                }
            }
        }
    }

    public void HandleColorSelect(Color color)
    {
        drawManager.SelectColor(color);
    }
}