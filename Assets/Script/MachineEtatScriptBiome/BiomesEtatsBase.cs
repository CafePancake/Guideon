using UnityEngine;

/// <summary>
/// Script servant à déclarer les différentes méthodes des Biomes
/// qui seront utilisées et héritées par les États Concret des Biomes.
/// </summary>
public abstract class BiomesEtatsBase
{
    // Doit déclarer les méthodes qui seront utilisées par les États Concret des Biomes.
    // Stocker la référence du Biome dans la variable biomes, pour y accéder
    public abstract void InitEtat(BiomesEtatsManager biome); // État qui sera appeler lorsque le Biome est créer.
    public abstract void UpdateEtat(BiomesEtatsManager biome); // État qui sera appeler à chaque frame (60x Secondes).
    public abstract void TriggerEnterEtat(BiomesEtatsManager biome, Collider other); // État qui sera appeler lorsque le Collider du joueur est entré dans le Trigger d'un Biome.
}
