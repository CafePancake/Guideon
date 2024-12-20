using System.Collections;
using UnityEngine;

public class BiomesEtatEteint : BiomesEtatsBase
{
    /*--------------------------------------------------------------------------------------------//

    Le BiomeEtatEteint va tout d'abord être complètement éteint, donc une texture noirci qui indique au joueur son ÉtatEteint.

    Ensuite une coroutine ce lancera avec un Timer qui va permettre au Biome de retourner à l'EtatActivable. Le délai sera à ajuster,
    mais peut se situer entre Une à Deux minutes.

    //--------------------------------------------------------------------------------------------*/

    // Classe qui est de type Abstrait à cause du lien avec le BiomesEtatsBase, ce qui lui permet de reprendre les différentes
    // méthodes de la classe BiomesEtatsBase, pour les modifier selon les besoins.

    /// <summary>
    /// Fonction qui sera appeler lorsque le Biome est créer qui aura pour but d'initialiser les différentes variables du Biome.
    /// </summary>
    /// <param name="biome"></param>
    public override void InitEtat(BiomesEtatsManager biome) 
    {
        // Debug.Log("Etat Activable");
        // Va chercher le matériaux de sable fissuré pour montrer que le biome est éteint.
        Object MateriauxEteint = Resources.Load("Biomes/MateriauxEffet/m5_1");
        // Prend le Renderer du biome pour appliquer des matériaux.
        Renderer RendererBiome = biome.GetComponent<Renderer>();
        // Applique le matériaux de base qui cache les biomes sur le biome.
        RendererBiome.material = (Material)MateriauxEteint;
        // Lance la coroutine qui va permettre au Biome de retourner à l'EtatActivable.
        biome.StartCoroutine(AnimationEteint(biome));
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

    IEnumerator AnimationEteint(BiomesEtatsManager biome)
    {
        yield return new WaitForSeconds(30f);

        // Permet d'aller chercher le matériaux du Biomes.
        Object MaterialLoad = Resources.Load("Biomes/MateriauxEffet/m1_1");
        // Permet d'aller chercher le matériaux transparent qui sera appliquer au biome pour une animation.
        Object MaterialTransparentLoad = Resources.Load("Biomes/MateriauxEffet/m2_1");
        // Permet d'aller chercher le Renderer du biome qui permet d'appliquer les différents matériaux chercher précédement
        Renderer RendererBiome = biome.GetComponent<Renderer>();
        // Permet d'avoir un float qui contient le Temps à l'aide de Time.deltaTime.
        float t = 0.0f;
        // Permet d'avoir un float fixe qui sera la longueur de l'animation qui sera effectuer au biome.
        float duration = 2f;
        // Pendant que le temps actuel est inférieur à la durée de l'animation, celui-ci provoque une animation.
        while (t < duration)
        {
            // Permet d'avoir la valeur du temps en frame avec Time.deltaTime qui sera contenu dans la valeur t.
            t += Time.deltaTime;

            // Permet d'avoir une valeur de rotation qui augmentera plus le temps va avancer.
            float rotation = t / duration;
            // Si le temps est entre 0 et la durée totale, appliquer une Rotation au biome selon son centre.
            if (t > 0f && t < duration)
            {
                // Permet d'avoir une rotation sur le biome qui donne une animation en prenant les valeur de rotation et une valeur de 3 prédéterminer.
                biome.transform.Rotate(3f * rotation, 3f * rotation, 3f * rotation, Space.Self);
                // Prend le matériaux transparent récupérer précédement et l'applique sur le biome.
                RendererBiome.material = (Material)MaterialTransparentLoad;

            }
            // Si le temps est entre 1.5 secondes et la durée totale de l'animation
            if (t > 1.5f && t < duration)
            {
                // Prend le matériaux qui correspond aux biomes et l'applique sur le biome.
                RendererBiome.material = (Material)MaterialLoad;
            }
            yield return null;
        }
        // Lorsque l'animation est terminer retourne la Transform.Rotation en 0,0,0.
        biome.transform.rotation = Quaternion.identity;
        biome.ChangerEtat(biome.activable);
    }
}
