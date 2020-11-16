using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

// reference for downloading texture to sprite: https://dev.to/cemuka/download-images-from-a-url-endpoint-in-runtime-with-unity-4f2a

public class ChangeTemplate : MonoBehaviour
{
    private SpriteRenderer rend;
    private Sprite sprite;

    // Start is called before the first frame update
    void OnEnable()
    {
        int isUsingTemplate = PlayerPrefs.GetInt("IS_USING_TEMPLATE", 0);
        string url = PlayerPrefs.GetString("CURRENT_TEMPLATE_URL", "");

        if(isUsingTemplate == 1)
        {
            rend = GetComponent<SpriteRenderer>();
            StartCoroutine(FetchSprite(url, (response) => {
                sprite = response;
                rend.sprite = sprite;
            }));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FetchSprite(string url, System.Action<Sprite> callback)
    {
        var www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            if (www.isDone)
            {
                var texture = DownloadHandlerTexture.GetContent(www);
                var rect = new Rect(0, 0, texture.width, texture.height);
                var sprite = Sprite.Create(texture,rect,new Vector2(0.5f,0.5f));
                callback(sprite);
            }
        }
    }

}
