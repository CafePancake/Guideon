using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class EnnemiEtatManager : MonoBehaviour
{
    // Passer à travers la liste des biomes pour trouver un chemin à X distance du personnage, et lui faire suivre un chemin ``Aleatoire``, faire des chemins cours pour éviter l'overload de donner.
    // Rien sérialisez dans EnnemiEtatsManager (Peut sérialiser sur l'ile le perso)
    // Rien ajouter dans l'état Manager, le Manager parle seulement à un état à la fois
    // On ne peut pas parler à un objet instantier localement, donc créer un référence dans BiomesEtatsManager à la place que localement
    // Machine à état (GOAP)

    private EnnemiEtatsBase etatActuel; // Variable qui va contenir l'état actuel du Biome.

    // Instance des classe Concrète, donc des différents États, doivent être accessible à l'extérieur, doit faire comme une liste pour créer l'instance.
    [Header("Référence Publique des États de l'ennemi")]
    public EnnemiEtatRepos repos = new EnnemiEtatRepos(); // Instance de l'état repos, qui permet à l'ennemi d'être en idle.
    public EnnemiEtatChasse chasse = new EnnemiEtatChasse(); // Instance de l'état Cultivable, qui permet de cultiver (récolter ressource) un Biome.
    public EnnemiEtatAttaque attaque = new EnnemiEtatAttaque(); // Instance de l'état Recoltable, qui permet de cultiver (récolter ressource) un Biome.
    public EnnemiEtatMort mort = new EnnemiEtatMort(); // Instance de l'état Recoltable, qui permet de cultiver (récolter ressource) un Biome.
    public UnityEngine.AI.NavMeshAgent agent {get; set;} // Instance qui permet l'accès au NavMeshAgent pour les déplacements de l'ennemi vers le joueur.
    public Animator anim {get; set;} // Instance qui permet l'accès à l'animator pour l'animation de l'ennemi.
    public Ennemi ennemiScript {get; set;} // Instance qui donne accès au Script qui donne accès au point de l'ennemi.
    public Dictionary<string, dynamic> infos {get ; set; } = new Dictionary<string, dynamic>(); // Dynamic permet de stocker n'importe quel valeur

    // Prend les références des composants nécessaires pour l'ennemi et les stockes dans le dictionnaire infos.
    void Awake()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();
        ennemiScript = GetComponent<Ennemi>();
        infos.Add("sonAttaque", ennemiScript._sonAttaqueEnnemi);
        infos.Add("ennemiLife", ennemiScript.vieActuelle);
        transform.Rotate(new Vector3(0, Random.Range(0, 360), 0));
    }

    // Met en référence l'état de base de l'ennemi et les stock dans le dictionnaire infos.
    void Start()
    {
        Vector3 spawnPoint = this.transform.position;
        float vitesseDeplacement = 1.5f;
        infos.Add("vitesseDeplacement", vitesseDeplacement);
        infos.Add("spawnPointOfEnemy", spawnPoint);
        ChangerEtat(repos); // Doit aller chercher un Type de class Abstraint pour effectuer le changement d'état.
    }

    // Fonction qui sera appeler par les différents état pour faire le changement de méthode.
    public void ChangerEtat(EnnemiEtatsBase etat)
    {
        etatActuel = etat;
        etatActuel.InitEtat(this); 
    }
    
    // Update is called once per frame
    void Update()
    {
        // Donne une référence au État du Manager de façon constente avec l'Update de façon à pouvoir appeler les différentes méthodes et états, This est le Biomes, donc le cube.
        etatActuel.UpdateEtat(this); 
        // Debug.Log( etatActuel.GetType().Name );
    }

    // Fonction OnTriggerEnter qui permet de déclencher une action lorsque le Collider du joueur est entré dans le Trigger d'un Biome.
    // Trigger enter à utiliser en priorité, 
    private void OnTriggerEnter(Collider other)
    {
        // Donne une référence au État du Manager lorsque le joueur entre en contact avec le biome de façon à pouvoir appeler les différentes méthodes et états, This est le Biomes, donc le cube.
        etatActuel.TriggerEnterEtat(this, other); 
    }
}
