using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using static UnityEngine.UI.Button;
using UnityEngine.Events;

public class PopupDisplayUI : MonoBehaviour
{
    public enum PopupPosition
    {
        Top = 1470,
        Middle = 0,
        Bottom = -1470
    }
    public static PopupDisplayUI instance;

    [SerializeField]
    private GameObject popupUi;

    [SerializeField]
    private TMP_Text popupText;

    [SerializeField]
    private Button confirmButton, cancelButton;

    public TMP_Text PopupText { get => popupText; set => popupText = value; }
    public Button ConfirmButton { get => confirmButton; set => confirmButton = value; }
    public Button CancelButton { get => cancelButton; set => cancelButton = value; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void ShowPopup(string text, PopupPosition position = PopupPosition.Middle, UnityAction confirmAction = null, UnityAction cancelAction = null)
    {
        var yPos = (int)position;
        var rect = popupUi.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(0, yPos);
        popupUi.SetActive(true);
        popupText.text = text;
        if(confirmAction != null)
        {
            confirmButton.onClick.AddListener(confirmAction);
            confirmButton.gameObject.SetActive(true);
        }
        else
        {
            confirmButton.gameObject.SetActive(false);
        }
        if (cancelAction != null)
        {
            cancelButton.onClick.AddListener(cancelAction);
            cancelButton.gameObject.SetActive(true);
        }
        else
        {
            cancelButton.gameObject.SetActive(false);
        }
    }
}
