// ﻿using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
//
// public class FlowGraph : MonoBehaviour
// {
//     [SerializeField] private Sprite circleSprite;
//     private RectTransform flowContainer;
//
//     private void Awake() {
//       flowContainer = transform.Find("Flow_Graph_Container").GetComponent<RectTransform>();
//
//       createCircle(new Vector2(200, 200));
//     }
//
//     private void createCircle(Vector2 anchoredPosition) {
//       GameObject gameObject = new GameObject("circle", typeof(Image));
//       gameObject.transform.SetParent(flowContainer, false);
//       gameObject.GetComponent<Image>().sprite = circleSprite;
//       RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
//       rectTransform.anchoredPosition = anchoredPosition;
//       rectTransform.sizeDelta = new Vector2(11, 11);
//       rectTransform.anchorMin = new Vector2(0, 0);
//       rectTransform.anchorMax = new Vector2(0, 0);
//     }
// }
