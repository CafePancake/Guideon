using System.Collections;
using UnityEngine;

public class BiomesEtatApparition : BiomesEtatsBase
{
    /*--------------------------------------------------------------------------------------------//

    Le BiomeEtatApparition va tout d'abord faire apparaître un ParticleSystem de type Portail (ou autre)

    Ensuite l'ennemi va apparaître par un Coroutine qui augmentera avec le temps, lorsque l'ennemi sera ScaleUP en (1,1,1) celui-ci
    pourra débuter sa Patrouille ou sa Chasse, le biome ensuite sera en EtatEteint

    //--------------------------------------------------------------------------------------------*/

    // Classe qui est de type Abstrait à cause du lien avec le BiomesEtatsBase, ce qui lui permet de reprendre les différentes
    // méthodes de la classe BiomesEtatsBase, pour les modifier selon les besoins.

    /// <summary>
    /// Fonction qui sera appeler lorsque le Biome est créer qui aura pour but d'initialiser les différentes variables du Biome.
    /// </summary>
    /// <param name="biome"></param>
    public override void InitEtat(BiomesEtatsManager biome) 
    {
        // Debug.Log("Etat Apparition");
        // Lance la couroutine AnimationCultivable en lui passant la valeur biome.
        biome.StartCoroutine(AnimationApparition(biome));
    } 

    /// <summary>
    /// État qui sera appeler à chaque frame (60x Secondes).
    /// </summary>
    /// <param name="biome"></param>
    public override void UpdateEtat(BiomesEtatsManager biome)
    {
        // Aucune ligne nécessaire
    }

    /// <summary>
    /// État qui sera appeler lorsque le Collider du joueur est entré dans le Trigger d'un Biome.
    /// </summary>
    /// <param name="biome"></param>
    public override void TriggerEnterEtat(BiomesEtatsManager biome, Collider other)
    {
        if (other.CompareTag("Perso")) biome.StartCoroutine(AnimationApparition(biome));
    } 

    IEnumerator AnimationApparition(BiomesEtatsManager biome)
    {
        // Créer une valeur GameObject qui contiendra la particule pour l'instantier par la suite.
        GameObject particulePrefab;
        // Permet d'aller chercher avec la fonction Resources.Load la première particule.
        particulePrefab = (GameObject)Resources.Load("Particules/p1_2");
        // Permet de stocker la particule Prefab avec son Component ParticleSystem pour l'instancier.
        ParticleSystem particule = particulePrefab.GetComponent<ParticleSystem>();
        // Stocke une la particule instancier dans une valeur ParticleSystem qui permet d'appliquer des modifications au particule.
        ParticleSystem particuleInstance = Object.Instantiate(particule, biome.transform.position, Quaternion.identity, biome.parentParticule);
        // Permet d'ajouter la particule dans le parent des particules dans la hiérarchie.
        particuleInstance.transform.parent = biome.parentParticule;
        // Change la rotation de la particule instancier vers le haut.
        particuleInstance.transform.rotation = Quaternion.Euler(-90, 0, 0);
        // Permet de faire jouer la particule
        particuleInstance.Play();

        GameObject ennemiPrefab = (GameObject)Resources.Load("Ennemis/e1_1");
        GameObject ennemiInstance = Object.Instantiate(ennemiPrefab, new Vector3(biome.transform.position.x, biome.transform.position.y + 1, biome.transform.position.z), Quaternion.identity);
        ennemiInstance.transform.parent = biome.parentEnnemi;
        ennemiInstance.transform.localScale = Vector3.zero;

        biome.infos["ennemiSurBiome"] = ennemiInstance;

        // Garde une référence du nombre de temps actuel ainsi que la durée totale de l'animation avec temps et dureeAnim
        float temps = 0f;
        float dureeAnim = 2f;

        while (temps < dureeAnim)
        {
            // Permet d'ajouter du temps à la valeur temps en fonction du nombre de frame prise en compte avec Time.deltaTime (60fps)
            temps += Time.deltaTime;

            // Change de façon croissante la valeur du localScale de l'ennemi jusqu'à arriver à 1,1,1
            ennemiInstance.transform.localScale = Vector3.Lerp(ennemiInstance.transform.localScale, new Vector3(0.2f, 0.2f, 0.2f), temps / dureeAnim);

            yield return null; // Retourne au début de la Coroutine pour attendre à la prochaine frame.
        }

        // Permet d'attendre qu'il n'y ait plus de particule avant d'aller plus loin dans le code.
        while (particuleInstance.IsAlive(true))
        {
            yield return null;
        }

        // Lorsque les particules ont cessé d'apparaître et sont toutes mortes, détruit l'objet.
        Object.Destroy(particuleInstance.gameObject);

        biome.ChangerEtat(biome.mort);
    }
}
