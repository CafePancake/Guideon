using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// classe qui sert uniquement a toggle le canvas de l,inventaire
/// </summary>
public class ToggleInventory : MonoBehaviour
{
    Canvas inventoryMenu;
    AudioManager _audio;
    public AudioClip sonToggleOn;
    public AudioClip sonToggleOff;

    void Start()
    {
        inventoryMenu = gameObject.GetComponent<Canvas>();
        _audio = AudioManager.Instance;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryMenu.enabled = inventoryMenu.enabled?false:true; //inverse le enabled.    si oui, non.  /  si non, oui.
            _audio.Play(inventoryMenu.enabled? sonToggleOn : sonToggleOff); //joue son approprie selon inventaire affiche ou disparait
        }
    }
}
