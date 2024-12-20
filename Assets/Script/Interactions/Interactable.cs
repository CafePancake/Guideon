using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe mere des objects interractables, 
/// quand le joueur appuie sur f (ou touche interaction) en étant proche d'un interactable, declanche sa fonction Interact()
/// </summary>
public class Interactable : MonoBehaviour
{
    public string message= ""; //tous les interactables ont un message pour prompt d<interaction
    public virtual void Interact(){
        
    }
}
