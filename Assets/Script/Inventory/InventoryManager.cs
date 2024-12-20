using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    private AudioManager audioManager; //manager audio
    public AudioClip sonPickup; //son qui joue quand on ajoute des ressources a l<inventaire

    public GameObject panelItem; //pannel qui affiche une ressource que le joue possede
    public GameObject panelNotif; //pannel de base pour notification de pickup d<un item
    public GameObject canvasNotif; //canvas that will contain the notifications
    public Dictionary<string, Item> contents = new Dictionary<string, Item>(); //liste des ressources dans l<inventaire, key nom de ressource, contient infos (nom, icone, nbheld)
    public Dictionary<string, GameObject> panels = new Dictionary<string, GameObject>(); //liste des pannels de l,inventaire, ont un key corrspondant au nom de la ressource qu<ils representent
    public UnityEvent updateInventory = new UnityEvent();


    void Awake()
    {
        if (Instance != null && Instance != this) //fait un singleton
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // DontDestroyOnLoad(gameObject); // Optional, justte une scene pas besoin
    }

    void Start()
    {
        audioManager = AudioManager.Instance;        
    }

    /// <summary>
    /// fonction qui ajoute un pannel de ressource a l,inventaire du joueur
    /// </summary>
    /// <param name="ressourceName"> nom de la ressource reliee a ce panel </param>
    /// <param name="icon"> icone de la ressource pour afficher dans l,ui</param>
    void AddPannel(string ressourceName, Sprite icon)
    {
        panels[ressourceName] = Instantiate(panelItem); //pannel de la ressource = nouv pannel
        panels[ressourceName].transform.SetParent(transform, false); //le gameobject est e parent d ece panel
        panels[ressourceName].GetComponentInChildren<Image>().sprite = icon; //image = icone de ressource
    }

    /// <summary>
    /// ajoute un nouvel item dans invenaire selon nom recu
    /// </summary>
    /// <param name="name">nom de la ressource aujoute</param>
    public void AddItem(string name)
    {
        if (!contents.ContainsKey(name))
        { //si a pas deja la ressource dans invenatire
            contents[name] = new Item { itemName = name, icon = Resources.Load<Sprite>("icones/" + name), nbHeld = 0 }; //nouvelle ressource dans inventaire, nom de ressource en parametre, cherche une icone selon le nom rece dans resources
        }
        Notify(name);

        // contents[key].nbHeld+=quantity;
        Mathf.Clamp(contents[name].nbHeld++, 0, 99); //tentative de clamp qui me marche pas
        Updatecount(name); //update le pannel ui de la ressource
    }

    /// <summary>
    /// fonction qui depense les ressources
    /// </summary>
    /// <param name="name">nom de la ressource</param>
    /// <param name="cost">nombre a depenser</param>
    public void SpendResources(string name, int cost)
    {
        contents[name].nbHeld -= cost;
        Updatecount(name); //update ui pannel de la ressource
    }


    /// <summary>
    /// fonction qui update les pannels des ressources
    /// </summary>
    /// <param name="name">nom de la ressource concernee</param>
    void Updatecount(string name)
    {
        if (!panels.ContainsKey(name))
        {
            AddPannel(name, contents[name].icon); //si on update le nombre d<une ressource qui n,a pas de pannel on cree un pannel pour la ressource
        }

        panels[name].gameObject.SetActive(true); //temp pour empecher que le panel disparaisse a jamais apres 0 held
        panels[name].GetComponentInChildren<TextMeshProUGUI>().text = "" + contents[name].nbHeld; //le texte update selon le nombre tenu

        updateInventory.Invoke(); //annonce que l,inventaire a ete change

        if (contents[name].nbHeld <= 0) //si le joeur n<a plus d<une ressource
        {
            panels[name].gameObject.SetActive(false); //pannel de ressource disparait
        }
    }

    public void Notify(string name)
    {
        audioManager.Play(sonPickup);

        GameObject newPanelNotif = Instantiate(panelNotif); //pannel de la ressource = nouv pannel
        newPanelNotif.GetComponentInChildren<Image>().sprite = contents[name].icon; //image = icone de ressource
        newPanelNotif.transform.SetParent(canvasNotif.transform, false); //le gameobject est e parent d ece panel
    }
}