using System.Collections;
using UnityEngine;

public class BiomesEtatRecoltable : BiomesEtatsBase
{
    /*--------------------------------------------------------------------------------------------//

    Le BiomeEtatApparition va tout d'abord avoir la TextureOriginale du biome ainsi que l'item pour le joueur à amasser.

    Une animation va se lancer directement sur l'objet pour démontrer au joueur qu'il peut amasser l'objet en Changeant le Scale
    entre 0.9 jusqu'a 1.1, lorsque le joueur passe sur le biome retourne lance une valeur aléatoire, si la valeur se situe entre 
    0.0 à .85, retourne à cultivable sinon éteint le biome.

    //--------------------------------------------------------------------------------------------*/

    // Classe qui est de type Abstrait à cause du lien avec le BiomesEtatsBase, ce qui lui permet de reprendre les différentes
    // méthodes de la classe BiomesEtatsBase, pour les modifier selon les besoins.

    /// <summary>
    /// Fonction qui sera appeler lorsque le Biome est créer qui aura pour but d'initialiser les différentes variables du Biome.
    /// </summary>
    /// <param name="biome"></param>
    public override void InitEtat(BiomesEtatsManager biome)
    {
        // Debug.Log("Etat Recoltable");
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
        if (other.CompareTag("Perso")) biome.StartCoroutine(AnimationRecoltable(biome));
    }

    IEnumerator AnimationRecoltable(BiomesEtatsManager biome)
    {
        GameObject item = biome.infos["itemSurBiome"];
        if (biome.infos["itemSurBiome"]) biome.infos["itemName"] = biome.infos["itemSurBiome"].GetComponent<CraftingMaterial>().matName;
        InventoryManager.Instance.AddItem(biome.infos["itemName"]);
        // Permet d'avoir un float qui contient le Temps à l'aide de Time.deltaTime.
        float t = 0.0f;
        // Permet d'avoir un float fixe qui sera la longueur de l'animation qui sera effectuer au biome.
        float duration = 2f;
        // Pendant que le temps actuel est inférieur à la durée de l'animation, celui-ci provoque une animation.
        Debug.Log("Debute l'animation de scale down");
        while (t < duration)
        {
            t += Time.deltaTime;
            float rotation = t / duration;
            if (t > 0 && t < duration)
            {
                biome.transform.Rotate(3f * rotation, 3f * rotation, 3f * rotation, Space.Self);
            }
            if (item) item.transform.localScale = Vector3.Lerp(item.transform.localScale, Vector3.zero, t / duration);
            yield return null;
        }

        biome.transform.rotation = Quaternion.identity;

        // State change logic should be here, after the animation completes

        // Debug.Log(item.transform.position);
        if (biome.infos["itemSurBiome"]) Object.Destroy(biome.infos["itemSurBiome"]);

        if (Random.value < 0.85f)
        {
            Debug.Log("Change vers etat cultivable");
            biome.ChangerEtat(biome.cultivable);
        }
        else
        {
            biome.ChangerEtat(biome.eteint);
        }
    }
}