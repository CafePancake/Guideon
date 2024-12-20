using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// un vendor est une classe qui sert a echanger une ressource pour une autre
/// </summary>
public class Vendor : MonoBehaviour
{
    public string[] reqMatsNames; //nom des materiaux demandes
    public int[] reqMatsNumbers; //quantites de materiaux demandes
    public Dictionary<string, int> cost = new Dictionary<string, int> //dictionaire qui contient le cout (nom du mat, nombre)
    {

    };
    private InventoryManager inventory; //ref a inventaire
    private AudioManager audioManager; //ref a inventaire
    public Button craftButton; //bouoton pour activer l<echange
    public bool isBought; //est ce que lobjet a deja ete echange/crafte
    public string itemName = "amulette"; //nom du item qui sera donne au joueur

    public AudioClip buttonSound;

    void Awake()
    {
        craftButton.interactable = false; //par defaut, ne peut pas crafter/echanger
        InitializeCost(); //genere un cout pour l<objet
    }

    void Start()
    {
        inventory = InventoryManager.Instance;
        audioManager = AudioManager.Instance;

        inventory.updateInventory.AddListener(SetAvailable); //ecoute quand l<inventaire change
    }

    public void CraftItem()
    {
        if (CheckHasReqMats() && !isBought) Trade();
    }

    public void Trade()
    {
        foreach (KeyValuePair<string, int> entry in cost) //pour chaque entree dans cout
        {
            string resource = entry.Key; //nom de ressource
            int cost = entry.Value;  //nb

            inventory.SpendResources(resource, cost); //depense nom de ressource, nombre de ressource
            isBought = true;
            craftButton.interactable = false;
        }
        audioManager.Play(buttonSound);
        inventory.AddItem(itemName);
    }

    public bool CheckHasReqMats()
    {
        foreach (KeyValuePair<string, int> entry in cost) //pour chaque entree dans cout
        {
            string resource = entry.Key; //donne moi le nom du item
            int cost = entry.Value; //et le nombre demande

            if (!inventory.contents.ContainsKey(resource) || inventory.contents[resource].nbHeld < cost) //si pas l<objet ou pas assez
            {
                return false; //retourne n<a pas les mats
            }
        }
        return true; //a passe tous les checks, retourne a les mats
    }

    public void SetAvailable()
    {
        if (!isBought)
        {
            craftButton.interactable = CheckHasReqMats();
        }
    }
    public void InitializeCost()
    {
        for (int i = 0; i < reqMatsNames.Length; i++)
        {
            cost[reqMatsNames[i]] = reqMatsNumbers[i];
        }
    }
}
