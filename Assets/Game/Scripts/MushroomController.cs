using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomController : MonoBehaviour
{
    [SerializeField]
    private string message;
    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void Activate()
    {
        anim.SetTrigger("Activate");
        PopupDisplayUI.instance.ShowPopup(message, PopupDisplayUI.PopupPosition.Middle, () => { Debug.Log("Okay Pressed"); } );
    }
}
