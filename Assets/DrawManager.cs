using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DrawManager : MonoBehaviour
{
    // reference to the line prefab
    public GameObject linePrefab;

    Line activeLine;
    bool EraserUsed = false;

    // Canvas
    [SerializeField] private Text m_PaintName;
    // Color
    [SerializeField] private Color m_CurrentColor;
    [SerializeField] private Image m_CurrentColorButtonImg;
    // Brush
    [SerializeField] private float m_BrushSize;
    [SerializeField] private float m_BrushOpacity;
    [SerializeField] private string m_BrushTexture;
    // Eraser
    [SerializeField] private Color m_SavedColorBeforeEraser;
    [SerializeField] private float m_SavedSizeBeforeEraser;
    [SerializeField] private float m_SavedBrushOpacityBeforeEraser;
    [SerializeField] private string m_SavedBrushTextureBeforeEraser;
    [SerializeField] private float m_EraserSize;

    void Start()
    {
        m_PaintName.text = "New Painting";
        m_CurrentColorButtonImg.color = Color.red;
        m_CurrentColor = Color.red;
        m_BrushSize = 0.008F;
        m_BrushTexture = "Basic";
        m_BrushOpacity = 1.00F;
        
        m_SavedColorBeforeEraser = m_CurrentColor;
        m_SavedSizeBeforeEraser = m_BrushSize;
        m_SavedBrushTextureBeforeEraser = m_BrushTexture;
        m_SavedBrushOpacityBeforeEraser = m_BrushOpacity;

    }

    void Update () 
    {
        if (Input.GetMouseButtonDown(0)){
            // Lines don't draw when cliking on UIs
            if (EventSystem.current.IsPointerOverGameObject()) 
                return; 
            
            GameObject lineGameObj = Instantiate(linePrefab);
            activeLine = lineGameObj.GetComponent<Line>();

            activeLine.SetColor(m_CurrentColor);
            activeLine.SetWidth(m_BrushSize);
            activeLine.SetTexture(m_BrushTexture);
            // Debug.Log(m_CurrentColor);
        }

        if (Input.GetMouseButtonUp(0)) {
            activeLine = null;
        }

        if(activeLine != null) {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            activeLine.UpdateLine(mousePos);
        }
    }

    #region Private Methods
    private void SetBrushColor(Color color)
    {
        m_CurrentColor = color;
        m_CurrentColorButtonImg.color = m_CurrentColor;
    }

    private void SetBrushSize(float size)
    {
        //Remapping size values
        // Slider has a value of 0-1, LineRenderer uses percents as width units
        // size = (size - minValue) / (maxValue - minValue);
        // size = (m_MaxBrushSize - m_MinBrushSize) * size + m_MinBrushSize;
        m_BrushSize = size;
    }

    private void SetBrushOpacity(float opacity)
    {
        //Remapping opacity values
        // Slider: 0-1 range, LineRenderer color alpha: 0-1 range
        m_BrushOpacity = opacity;
        // Alpha component of the color (0 is transparent, 1 is opaque).
        m_CurrentColor.a = opacity;
    }

    private void SetBrushTexture(string tex) {
        m_BrushTexture = tex; 
    }
    #endregion

    #region Public Methods

    public void RenamePainting(Text name)
    {
        m_PaintName.text = name.text;
    }

    public string GetPaintingName()
    {
        return m_PaintName.text;
    }

    public void SelectColor(Color color)
    {
        SetBrushColor(color);
    }

    public Color GetColor()
    {
        return m_CurrentColor;
    }

    public void ChangeBrushSize(Slider slider)
    {   
        // slider.minValue, slider.maxValue = (0, 1)
        // which is consistent with Linerenderer's width range
        SetBrushSize(slider.value);
    }

    public float GetBrushSize()
    {
        return m_BrushSize;
    }

    public void SelectTexture(string BrushTitle) {
        SetBrushTexture(BrushTitle);
    }

    public string GetTexture()
    {
        return m_BrushTexture;
    }

    public void ChangeBrushOpacity(Slider slider)
    {
        SetBrushOpacity(slider.value);
    }

    public float GetBrushOpacity()
    {
        return m_BrushOpacity;
    }

    public void SelectEraser ()
    {
        // save previously used color,size,opacity,texture to go back to if user toggles set pencil after
        m_SavedColorBeforeEraser = m_CurrentColor;
        m_SavedSizeBeforeEraser = m_BrushSize;
        m_SavedBrushTextureBeforeEraser = m_BrushTexture;
        m_SavedBrushOpacityBeforeEraser = m_BrushOpacity;
        Debug.Log("size" + m_SavedSizeBeforeEraser);

        // set color to white (or same as canvas)
        m_CurrentColor = Color.white;
        SetBrushSize(1);
        SetBrushOpacity(1);
        SetBrushTexture("Basic");
        EraserUsed = true;
    }

    public void ChangeEraserSize ()
    {
    
    }

    public void SelectPencilOrColorAfterEraser ()
    {
        if(EraserUsed) {
            SetBrushColor(m_SavedColorBeforeEraser);
            SetBrushSize(m_SavedSizeBeforeEraser);
            SetBrushTexture(m_SavedBrushTextureBeforeEraser);
            SetBrushOpacity(m_SavedBrushOpacityBeforeEraser);
        }
        EraserUsed = false;
    }

    #endregion
}
