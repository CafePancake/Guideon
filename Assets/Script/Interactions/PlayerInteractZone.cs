using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// class qui s<occupe des interactions entre joueur et interactables 
/// </summary>
public class InteractSphere : MonoBehaviour
{
    public bool wantsInteract;
    public TextMeshProUGUI interactPrompt; //un prompt dans le ui qui sert a indiquer qu<une interaction est possible

    void Update()
    {
        wantsInteract = Input.GetKey(KeyCode.F); //set un bool si le joueur appui pour interagir
    }


    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Interactable"))
        {
            interactPrompt.text = other.GetComponent<Interactable>().message; //set le prompt d<interaction au message du interactable
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Interactable")&&wantsInteract) //si dans la zone d<interaction et interagis
        {
            other.GetComponent<Interactable>().Interact(); //appelle fonction interact de l<interactable
            interactPrompt.text = other.GetComponent<Interactable>().message; //update prompt pour afficher le message (indication) du interactable
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Interactable"))
        {
            interactPrompt.text=""; //vide le promt quand s<eloigne du interactable
        }
    }
}

