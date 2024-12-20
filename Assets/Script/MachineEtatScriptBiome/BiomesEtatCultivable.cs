using System.Collections;
using UnityEngine;

public class BiomesEtatCultivable : BiomesEtatsBase
{
    /*--------------------------------------------------------------------------------------------//

Le BiomeEtatCultivable va tout d'abord avoir la TextureOriginal du Biome

Lorsque le joueur entrera en Collision avec le BiomeEtatCultivable, l'animation de cultivation débute ainsi
l'item qui sera attribuer en GénérationAléatoire va ScaleUp jusqu'a 1,1,1 de façon Smooth, lorsque l'objet aura terminer d'apparaître

Le biome changera à l'état BiomeEtatRecoltable.

//--------------------------------------------------------------------------------------------*/

    public override void InitEtat(BiomesEtatsManager biome)
    {
        // Reçoit un item aléatoire qui sera attribué au Biome lors de sa génération.
        ItemAlea(biome);
    }

    public override void UpdateEtat(BiomesEtatsManager biome)
    {
        // Aucune ligne nécessaire
    }

    public override void TriggerEnterEtat(BiomesEtatsManager biome, Collider other)
    {
        // Lance la couroutine AnimationCultivable en lui passant la valeur biome.
        if (other.CompareTag("Perso")) biome.StartCoroutine(AnimationCultivable(biome));
    }

    private void ItemAlea(BiomesEtatsManager biome)
    {
        // Contient la valeur max aléatoire de l'item à générer.
        int nombreItemAleatoire = 0;
        // Récupère la valeur du Biome et selon la valeur du Biome, on défini le nombre d'item que le biome contient
        switch (biome.infos["biome"])
        {
            case 1:
                nombreItemAleatoire = 4;
                break;
            case 2:
                nombreItemAleatoire = 4;
                break;
            case 3:
                nombreItemAleatoire = 6;
                break;
            default:
                nombreItemAleatoire = 0; // Valeur par défaut
                break;
        }

        // Full Random (à switch pour du SemiRandom, donc chaque item à un % de chance d'apparaître)
        int itemAleatoire = Random.Range(1, nombreItemAleatoire + 1);

        // Ajoute la valeur du nombre d'item aléatoire pour générer l'item dans Resources.Load
        biome.infos["itemAlea"] = itemAleatoire;
    }

    IEnumerator AnimationCultivable(BiomesEtatsManager biome)
    {
        // Créer une valeur GameObject qui contiendra la particule pour l'instantier par la suite.
        GameObject particulePrefab;
        // Permet d'aller chercher avec la fonction Resources.Load la première particule.
        particulePrefab = (GameObject)Resources.Load("Particules/P1_1");
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

        // Permet d'aller chercher avec la fonction Resources.Load l'item qui sera générer sur le biome.
        GameObject itemPrefab = (GameObject)Resources.Load("Biomes/Items/i" + biome.infos["biome"] + "_" + biome.infos["itemAlea"]);
        Debug.Log("Item Générer : " + itemPrefab.name);
        // Garde une référence du GameObject qui est instancier dans la scene, pour modifier son localScale.
        biome.infos["itemSurBiome"] = Object.Instantiate(itemPrefab, new Vector3(biome.transform.position.x, biome.transform.position.y + 1, biome.transform.position.z), Quaternion.identity);
        // Permet d'ajouter l'item dans le parent des items dans la hiérarchie.
         biome.infos["itemSurBiome"].transform.parent = biome.parentItem;
        // Permet d'ajuster le localScale de l'item à 0,0,0 pour l'animation.
         biome.infos["itemSurBiome"].transform.localScale = Vector3.zero;

        // Garde une référence du nombre de temps actuel ainsi que la durée totale de l'animation avec temps et dureeAnim
        float temps = 0f;
        float dureeAnim = 2f;

        // Tant que le temps qui est générer est inférieur à la durée de l'animation, continue à Instancier l'objet sur la scène en le montant son scale.
        while (temps < dureeAnim)
        {
            // Permet d'ajouter du temps à la valeur temps en fonction du nombre de frame prise en compte avec Time.deltaTime (60fps)
            temps += Time.deltaTime;

            // Change de façon croissante la valeur du localScale de l'item jusqu'à arriver à 1,1,1
             if(biome.infos["itemSurBiome"])biome.infos["itemSurBiome"].transform.localScale = Vector3.Lerp( biome.infos["itemSurBiome"].transform.localScale, Vector3.one, temps / dureeAnim);

            yield return null; // Retourne au début de la Coroutine pour attendre à la prochaine frame.
        }

        // Lorsque les particules ont cessé d'apparaître et sont toutes mortes, détruit l'objet.
        Object.Destroy(particuleInstance.gameObject);

        // Permet de changer le biome vers l'état Recoltable, pour permettre au joueur de récolter l'objet qui sera générer.
        biome.ChangerEtat(biome.recoltable); // A CHANGER !!!!
    }
}