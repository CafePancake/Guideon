using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Stele : Interactable
{
    InventoryManager inventory; //inventaire manager
    AudioManager audioManager; //manager audio
    public AudioClip insertSound; //son quand on va porter un embleme a la stele
    public AudioClip completionSound; //
    public Perso _persoInfo; //script perso
    public string[] specialItemReq; //tableau qui contient le nom des items demandes pour chaque stage

    //les quatres elements du modeles de la stele qui correspondent aux emblemes
    public GameObject fireEmblem;
    public GameObject airEmblem;
    public GameObject earthEmblem;
    public GameObject waterEmblem;
    public int stage = 0; //stage actuel
    public bool isComplete = false; //si tous les objets demandes ont ete apportes

    void Start()
    {
        inventory = InventoryManager.Instance;
        audioManager = AudioManager.Instance;
    }

    public override void Interact()
    {
        if (!isComplete) //si pas deja complet
        {
            InsertEmblems(); //si as les mats necessaires, faire l<echange
        }
    }

    public void InsertEmblems()
    {
        for (int i = 0; i < specialItemReq.Length; i++)
        {
            if (!inventory.contents.ContainsKey(specialItemReq[i])) //si n<as pas l<embleme recherche
            {
                continue; //passe et cherche pour le prochain
            }
            else if (inventory.contents[specialItemReq[i]].nbHeld > 0) //sinon si en a au moins un
            {
                inventory.SpendResources(specialItemReq[i], 1); //'depense' le item de l<inventaire
                audioManager.Play(insertSound);
                GrantReward(specialItemReq[i]); //donne une recompense en fonction du item donne
                stage++;
            }
        }
        if (stage >= specialItemReq.Length) //si le stage de progression de stele est egal aux nombre d<items speciaux demand/e
        {
            isComplete = true; //considere que complet
            audioManager.Play(completionSound);
            message = "La stèle est complète"; //change prompt pour ne pas donner une instruction et indiquer complet
        }
    }

/// <summary>
/// s<occupe de recompenser le joueur quand il amene un embleme a la stele
/// </summary>
/// <param name="specialItemName">nom du item donne a la stele</param>
    public void GrantReward(string specialItemName)
    {
        if (specialItemName == "Emblème de feu") // ici unlock le pouvoir relie a l<embleme donne
        {
            fireEmblem.SetActive(true); //rends visible l<embleme sur modele de stele
            _persoInfo._isFireActive = true; //unlolck pouvoir
        }
        else if (specialItemName == "Emblème d'air")
        {
            airEmblem.SetActive(true);
            _persoInfo._isAirActive = true;
        }
        else if (specialItemName == "Emblème de terre") 
        {
            earthEmblem.SetActive(true);
            _persoInfo._isEarthActive = true;
        }
        else if (specialItemName == "Emblème d'eau")
        {
            waterEmblem.SetActive(true);
            _persoInfo._isWaterActive = true;
        }
    }
}
