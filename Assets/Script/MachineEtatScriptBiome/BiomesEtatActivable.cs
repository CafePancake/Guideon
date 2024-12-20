using System.Collections;
using UnityEngine;

public class BiomesEtatActivable : BiomesEtatsBase
{
    /*--------------------------------------------------------------------------------------------//

    Le BiomeEtatActivable va tout d'abord afficher la TextureDeBase de l'objet.

    Lorsque le joueur va toucher le Biome qui possède cet ÉtatActivable, l'objet va Tourner sur lui même à une certaine vitesse,
    celui-ci va changer sa Texture vers une Texture transparente pour à la suite d'un certain délai prendre la TextureOriginale du Biome.

    Aussi, le prochain état du Biome sera générer aléatoirement avec largement plus de Chance d'être EtatCultivable qu'EtatApparition.

    //--------------------------------------------------------------------------------------------*/
    // Classe qui est de type Abstrait à cause du lien avec le BiomesEtatsBase, ce qui lui permet de reprendre les différentes
    // méthodes de la classe BiomesEtatsBase, pour les modifier selon les besoins.

    /// <summary>
    /// Fonction qui sera appeler lorsque le Biome est créer qui aura pour but d'initialiser les différentes variables du Biome.
    /// </summary>
    /// <param name="biome"></param>
    public override void InitEtat(BiomesEtatsManager biome)
    {
        // Va chercher le matériaux de sable fissuré pour montrer que le biome est éteint.
        Object MateriauxMort = Resources.Load("Biomes/MateriauxEffet/m1_1");
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
        //Lorsque la Sphere de force intéragis avec le biome celui-ci déclanche une coroutine.
        if (other.CompareTag("Perso")) biome.StartCoroutine(Animation(biome));
    }

    public IEnumerator Animation(BiomesEtatsManager biome)
    {
        // Permet d'avoir une valeur aléatoire qui se situe entre 0 et 1.
        float random = Random.value;
        // Permet d'avoir une valeur qui contiendra un string  
        string etatBiome;
        // Si la valeur aléatoire est inférieur ou égal à .95f alors le Biome va être cultivable.
        if (random <= .95f)
        {
            etatBiome = "cultivable";
        }
        // Si la valeur aléatoire est supérieur ou égal à .95f et inférieur ou égal à .995f alors le Biome va être en ambiance, donc ajouter du décor.
        else if(random >= .99f && random <= .995f)
        {
            etatBiome = "ambiance";
            Debug.Log("Ambiance");
        }
        // Si la valeur aléatoire est supérieur ou égal à .995f alors le Biome va être en Apparition, donc fait apparaître des monstres sur le biome
        else
        {
            // Permet d'exprimer sur le string l'état Apparition, vers lequel le changement d'état sera effectuer.
            etatBiome = "apparition";
        }
        // Permet d'aller chercher le matériaux du Biomes.
        Object MaterialLoad = Resources.Load("Biomes/Materiaux/b" + biome.infos["biome"] + "_" + biome.infos["variant"]);
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
                biome.transform.Rotate(5f * rotation, 5f * rotation, 5f * rotation, Space.Self);
            }
            if (t > 1.5f && t < duration)
            {
                // À parti de 1.5s, applique le matériaux du biome sur le biome.
                RendererBiome.material = (Material)MaterialLoad;
            }
            yield return null;
        }
        // Lorsque l'animation est terminer retourne la Transform.Rotation en 0,0,0.
        biome.transform.rotation = Quaternion.identity;
        // Si le temps est supérieur à 2, change l'état du biome pour passer à la prochaine étape.
        if (t >= 2f)
        {
            if (etatBiome == "cultivable")
            {
                // Fonction qui permet de changer l'état et qui l'amène à l'état cultivable, donc met des ressources sur le biome.
                biome.ChangerEtat(biome.cultivable);
            }
            else if (etatBiome == "ambiance")
            {
                // Fonction qui permet de changer l'état et qui l'amène à l'état d'ambiance, donc mets des objets de décorations sur le biome.
                biome.ChangerEtat(biome.ambiance);
            }
            else
            {
                // Fonction qui permet de changer l'état et qui l'amène à l'état apparition, donc fait apparaître des ennemis sur le biome.
                biome.ChangerEtat(biome.apparition);
            }
            // Permet d'arrêter la coroutine après avoir changer d'état.
            yield break;
        }
    }

}
