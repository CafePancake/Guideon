using System.Collections;
using UnityEngine;

public class BiomesEtatAmbiance : BiomesEtatsBase
{
    /*--------------------------------------------------------------------------------------------//

    Le BiomeEtatMort va tout simplement être un Biome qui sera désactivé et ne sera plus réutilisable, donc ne changera pas d'état.

    //--------------------------------------------------------------------------------------------*/

    // Classe qui est de type Abstrait à cause du lien avec le BiomesEtatsBase, ce qui lui permet de reprendre les différentes
    // méthodes de la classe BiomesEtatsBase, pour les modifier selon les besoins.

    /// <summary>
    /// Fonction qui sera appeler lorsque le Biome est créer qui aura pour but d'initialiser les différentes variables du Biome.
    /// </summary>
    /// <param name="biome"></param>
    public override void InitEtat(BiomesEtatsManager biome) 
    {
        Debug.Log("AMBIANCE");
        AleaAmbiance(biome);
        biome.StartCoroutine(AnimationAmbiance(biome));
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
        // Aucune ligne nécessaire
    } 

    private void AleaAmbiance(BiomesEtatsManager biome)
    {
        biome.infos["AleaAmbiance"] = Random.Range(1, 5);
        Debug.Log("AleaAmbiance" + biome.infos["AleaAmbiance"]);
    }


    public IEnumerator AnimationAmbiance(BiomesEtatsManager biome)
    {
        Debug.Log("JE DEBUTE L'AMBIANCE");
        // Permet d'aller chercher avec la fonction Resources.Load le préfab d'ambiance qui sera générer sur le biome.
        GameObject ambiancePrefab = (GameObject)Resources.Load("Biomes/Ambiance/a1_" + biome.infos["AleaAmbiance"]);
        // Garde une référence du GameObject qui est instancier dans la scene, pour modifier son localScale.
        biome.infos["AmbianceSurBiome"] = Object.Instantiate(ambiancePrefab, new Vector3(biome.transform.position.x, biome.transform.position.y + .8f, biome.transform.position.z), Quaternion.identity);
        biome.infos["AmbianceSurBiome"].transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        // Permet d'ajouter l'objet de décor sur le parent Ambiance dans la hiérarchie.
         biome.infos["AmbianceSurBiome"].transform.parent = biome.parentAmbiance;
        // Permet d'ajuster le localScale de l'item à 0,0,0 pour l'animation.
         biome.infos["AmbianceSurBiome"].transform.localScale = Vector3.zero;

        // Garde une référence du nombre de temps actuel ainsi que la durée totale de l'animation avec temps et dureeAnim
        float temps = 0f;
        float dureeAnim = 2f;

        // Tant que le temps qui est générer est inférieur à la durée de l'animation, continue à Instancier l'objet sur la scène en le montant son scale.
        while (temps < dureeAnim)
        {
            // Permet d'ajouter du temps à la valeur temps en fonction du nombre de frame prise en compte avec Time.deltaTime (60fps)
            temps += Time.deltaTime;

            // Change de façon croissante la valeur du localScale de l'item jusqu'à arriver à 1,1,1
             if(biome.infos["AmbianceSurBiome"])biome.infos["AmbianceSurBiome"].transform.localScale = Vector3.Lerp( biome.infos["AmbianceSurBiome"].transform.localScale, Vector3.one, temps / dureeAnim);

            yield return null; // Retourne au début de la Coroutine pour attendre à la prochaine frame.
        }

        // Permet de changer le biome vers l'état Mort, ce qui gardera le biome actif pour garder seulement un objet de décor sur le biome.
        biome.ChangerEtat(biome.mort);
        yield break;
    }
}
