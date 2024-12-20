using System.Collections;
using UnityEngine;

public class BiomesEtatMort : BiomesEtatsBase
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
        // Debug.Log("Etat Mort");
        // Va chercher le matériaux de sable fissuré pour montrer que le biome est éteint.
        Object MateriauxMort = Resources.Load("Biomes/MateriauxEffet/m6_1");
        // Prend le Renderer du biome pour appliquer des matériaux.
        Renderer RendererBiome = biome.GetComponent<Renderer>();
        // Applique le matériaux de base qui cache les biomes sur le biome.
        RendererBiome.material = (Material)MateriauxMort;
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
}
