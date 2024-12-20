using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenererArchipel : MonoBehaviour
{
    //Sérialiser le Prefab Ile (gameObject prefab qui contient le script générateur d'ile auto)
    public GameObject _prefabIle;
    //Sérialiser la propriété int _nombreDIles dans un interval possible de 1 à 30 iles. Déclaré à 1 par défaut.
    [Range(1, 30)] public int _nombreDIles = 1;
    
    //Sérialiser les deux propriétés int _tailleIleMin et _variationMaxTaille pour avoir des iles de taille aléatoire comprises entre _tailleIleMin et _tailleIleMin Additionnée de _variationMaxTaille. Déclarés à 10 par défaut.
    public int _tailleIleMin = 10;
    public int _variationMaxTaille = 10;

    //Sérialiser un int _padding qui ajoute un espace entre deux iles (valeur int libre). Déclaré à 10 par défaut.
    public int _padding = 10;

    //Sérialiser un Booléen _carteBiome qui va dre aux iles créées si les biomes utilisent la map perlin de base (hauteur des cubes) ou une map perlin distincte. Déclaré à false par défaut.
    public bool _carteBiome = false;

    //Sérialiser un booléen _circulaire qui va dire aux iles créées si elles seront circulaires ou rectangulaires. Déclaré à false par défaut.
    public bool _circulaire = false;

    void Start() 
    {
        GenererIles(_nombreDIles);
    }

    void GenererIles(int nbIles)
    {
        // On déclare deux floats offSetX et offSetZ qu'on met à 0 par défaut pour que l'ile 1 soit générée à la position 0,0,0
        float offsetx = 0f;
        float offsetz = 0f;

        // On crée un int tailleIlePrecedente (defaut à 0) pour y stocker a taille de l'ile que l'on vient de créer et utiliser cette valeur lors de la création de l'ile suivante pour définir le offset entre les deux iles (évite les chevauchements entre deux iles)
        int tailleIlePrecedente = 0;

        // On loop entre 0 et notre nombre d'ile définie dans l'éditeur avec notre slider entre 1 et 30.
        for (int i = 0; i < nbIles; i++)
        {
            // Choix de la taille d'ile aléatoire on défini la valeur du int tailleIle entre _tailleIleMin et _tailleIleMin additionnée de _variationMaxTaille
            int tailleIle = Random.Range(_tailleIleMin, _tailleIleMin + _variationMaxTaille);

            // On défini un pourcentage aléatoire pour notre ile 'hors de l'eau' entre 70% et 90%
            int pourcent = Random.Range(70, 90);

            // On défini la taille réelle de l'ile en multipliant sa taille par son pourcentage hors de l'eau
            int tailleIleReelle = tailleIle * pourcent / 100 ; // Maybe

            // Le offset de l'ile à créer est l'addition des deux tailles d'iles (précédente et courante) divisé par 2
            int offset = (tailleIlePrecedente + tailleIleReelle) / 2;
            Debug.Log(offset + "offset ile");

            // Une chance sur 2 d'ajouter ce offset ainsi que le padding au x de l'ile sinon, ajout au z de l'ile
            if (Random.value > 0.5f)
            {
                offsetx += _padding + offset; // Maybe 
                Debug.Log(offsetx + "offset ileX");
            }
            else
            {
                offsetz += _padding + offset; // Maybe 
                Debug.Log(offsetz + "offset ileZ");
            }

            // On instancie le prefab de notre ile avec les coordonnées définies précédemment (l'ile reste à 0 en y)
            GameObject uneIle = Instantiate(_prefabIle, new Vector3( offsetx, 0, offsetz), Quaternion.identity);

            // On appelle la méthode publique CreerIle de notre ile instanciée en lui passant tous les paramètres requis.
            // CreerIle (largeur, profondeur, coeff hauteurs cubes, zoom perlin, zoom perlin biomes, zoom perlin variantes, pourcentage erosion ile, pourcentage hors eau, carteBiome, circulaire)
            uneIle.GetComponent<GenerateurIleAuto>().CreerIle(tailleIle, tailleIle, Mathf.Clamp((tailleIle / 8), 1 , 8), Random.Range(9,15), Random.Range(2,20), Random.Range(2, 20), Random.Range(10,30), pourcent , _carteBiome, _circulaire);

            // On stock a taille de l'ile actuelle = son pourcentage hors de l,eau dans tailleIlePrecedente avant de repartir la boucle et créer l'ile suivante.
            tailleIlePrecedente = tailleIleReelle ; // Maybe
            Debug.Log(tailleIlePrecedente);
        }
    }
}
