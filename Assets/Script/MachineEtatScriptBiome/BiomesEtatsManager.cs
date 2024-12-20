using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomesEtatsManager : MonoBehaviour
{
    // Rien sérialisez dans BiomesEtatsManager (Peut sérialiser sur l'ile le perso)
    // Rien ajouter dans l'état Manager, le Manager parle seulement à un état à la fois
    // On ne peut pas parler à un objet instantier localement, donc créer un référence dans BiomesEtatsManager à la place que localement
    // Machine à état (GOAP)
    private BiomesEtatsBase etatActuel; // Variable qui va contenir l'état actuel du Biome.

    // Instance des classe Concrète, donc des différents États, doivent être accessible à l'extérieur, doit faire comme une liste pour créer l'instance.
    [Header("Référence Publique des États de Biomes")]
    public BiomesEtatActivable activable = new BiomesEtatActivable(); // Instance de l'état Activable, qui permet d'activer un 
    public BiomesEtatCultivable cultivable = new BiomesEtatCultivable(); // Instance de l'état Cultivable, qui permet de cultiver (récolter ressource) un Biome.
    public BiomesEtatRecoltable recoltable = new BiomesEtatRecoltable(); // Instance de l'état Recoltable, qui permet de cultiver (récolter ressource) un Biome.
    public BiomesEtatApparition apparition = new BiomesEtatApparition(); // Instance de l'état Apparition, qui permet de faire apparaître un ennemi sur un Biome.
    public BiomesEtatEteint eteint = new BiomesEtatEteint(); // Instance de l'état Eteint, qui permet d'éteindre un Biome.
    public BiomesEtatMort mort = new BiomesEtatMort(); // Instance de l'état Eteint, qui permet d'éteindre un Biome.
    public BiomesEtatAmbiance ambiance = new BiomesEtatAmbiance(); // Instance de l'état Eteint, qui permet d'éteindre un Biome.

    public Dictionary<string, dynamic> infos {get ; set; } = new Dictionary<string, dynamic>(); // Dynamic permet de stocker n'importe quel valeur

    [Header("Reference GameObject + Variant")]
    public GameObject perso; // Va chercher la référence du personnage pour changer l'état des biomes
    public Transform parentItem; // Va chercher la référence du parent des items pour instancier les items à l'intérieur du parent Item dans la hiérarchie.
    public Transform parentParticule; // Va chercher la référence du parent des items pour instancier les particules à l'intérieur du parent Item dans la hiérarchie.
    public Transform parentEnnemi; // Va chercher la référence du parent des items pour instancier les ennemis à l'intérieur du parent Item dans la hiérarchie.
    public Transform parentAmbiance; 

    // Start is called before the first frame update
    void Start()
    {
        ChangerEtat(activable); // Doit aller chercher un Type de class Abstraint pour effectuer le changement d'état.
    }

    // Fonction qui sera appeler par les différents état pour faire le changement de méthode.
    public void ChangerEtat(BiomesEtatsBase etat)
    {
        etatActuel = etat;
        // Créer une entrée dans le dictionnaire qui contient va contenir l'état du biome, GetType me retourne l'état du Biome en String à l'aide de .Name (ex : BiomesEtatActivable), on utilise Replace pour garder seulement l'etat en enlevant "BiomesEtat"
        infos["etat"] = etatActuel.GetType().Name.Replace("BiomesEtat", "");
        // Initialise l'état qui vient d'être attribué au Manager, This est le Biomes, donc le cube.
        etatActuel.InitEtat(this); 
    }
    
    // Update is called once per frame
    void Update()
    {
        // Donne une référence au État du Manager de façon constente avec l'Update de façon à pouvoir appeler les différentes méthodes et états, This est le Biomes, donc le cube.
        etatActuel.UpdateEtat(this); 
    }

    // Fonction OnTriggerEnter qui permet de déclencher une action lorsque le Collider du joueur est entré dans le Trigger d'un Biome.
    // Trigger enter à utiliser en priorité, 
    private void OnTriggerEnter(Collider other)
    {
        // Donne une référence au État du Manager lorsque le joueur entre en contact avec le biome de façon à pouvoir appeler les différentes méthodes et états, This est le Biomes, donc le cube.
        etatActuel.TriggerEnterEtat(this, other); 
    }

}
