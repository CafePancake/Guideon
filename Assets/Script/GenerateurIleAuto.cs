using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateurIleAuto : MonoBehaviour
{
    //Référence d'objet nécessaire au Script
    [Header("Reference")]
    [SerializeField] GameObject _prefabCube; // Prefab du cube de l'île pour l'instancier.
    // [SerializeField] GameObject _prefabPerso; // Prefab du personnage pour l'instantier.
    [SerializeField] Renderer _texturePlane; // Affiche sur un plane la Texturation de la fonction Sigmoide + Perlin Noise.
    [SerializeField] Transform _parentCube; // Permet d'instancier mes cubes de l'île à l'intérieur d'un objet vide comme parent
    [SerializeField] List<GameObject> _listeCube = new List<GameObject>();

    //Valeur qui permet de déterminer la hauteur et la largeur de l'île à l'intérieur du Tableau.
    [Header("Variable Modifiable")]
    [SerializeField, Range(10, 350)] int _ileLargeur; // Nombre de cube de largeur de l'île
    [SerializeField, Range(10, 350)] int _ileProfondeur; // Nombre de cube de profondeur de l'île
    [SerializeField, Range(0, 10)] float _ileCoefHauteur;  //Coefficient de multiplication de la hauteur pour générer une carte + ou - haute.

    //Valeur d'atenuation, lorsque l'on divise, il a une influence sur l'ile qui sera creer.
    //Plus la valeur est haute plus l'ile sera flat.
    [SerializeField, Range(0, 50)] float _forcePerlinCarte; // Force Perlin pour créer l'île
    [SerializeField, Range(0, 50)] float _forcePerlinVariants; // Force Perlin pour générer les différents variants d'item sur la carte.
    [SerializeField, Range(0, 50)] float _forcePerlinBiomes; // Force Perlin pour générer les zones de biomes sur la carte.

    //Champ qui permet d'impacter le rapport d'eau et de terre de l'ile
    [Header("Fonction Sigmoide")]
    [SerializeField, Range(0, 100)] float k; //Valeur entre 0 et 1
    [SerializeField, Range(0, 1)] float c; //Contraste de l'ile (Pourcentage)

    [Header("Resources")]
    private List<List<Material>> biomesMats = new List<List<Material>>(); // Double boucle qui contient les différents matériaux et objets de l'île.
    [SerializeField, Range(0,100)] int _fertile; // Pourcentage de chance de faire apparaitre une ressource sur le cube.
    void Start()
    {
        _listeCube.Clear();
        LoadResources();
        GenererCarte();
    }

    /// <summary>
    /// Fonction publique qui permet de générer plusieurs île de façon procédurale.
    /// </summary>
    /// <param name="il"></param>
    /// <param name="ip"></param>
    /// <param name="ch"></param>
    /// <param name="zp"></param>
    /// <param name="zv"></param>
    /// <param name="pei"></param>
    /// <param name="phe"></param>
    /// <param name="carteBiome"></param>
    /// <param name="circulaire"></param>
    public void CreerIle(int il, int ip, int ch, int zp, int zb, int zv, int pei, int phe, bool carteBiome, bool circulaire)
    {
        _ileLargeur = il;
        _ileProfondeur = ip;
        _ileCoefHauteur = ch;
        _forcePerlinCarte = zp;
        _forcePerlinBiomes = zb;
        _forcePerlinVariants = zv;
        k = pei;
        c = (float)phe / 100;
        GenererMap(carteBiome, circulaire);
    }

    private void GenererMap (bool carteBiome, bool circulaire)
    {
        LoadResources();
        float[,] uneCarte = Terraformer(_ileLargeur, _ileProfondeur, _forcePerlinCarte);

        // Si le booléen est True créer une île Circulaire sinon créer une île Rectangulaire
        uneCarte = circulaire ? AquaformeCirculaire(uneCarte) : Aquaforme(uneCarte);

        // Si le booléen de CarteBiome est vrai Terraformer une nouvelle texture Perlin et créer une carte sinon redonne la même carte qui a générer l'île
        float[,] uneCarteBiomes = carteBiome ? Terraformer(_ileLargeur, _ileProfondeur, _forcePerlinCarte * _forcePerlinBiomes) : uneCarte;

        float[,] uneCarteVariants = Terraformer(_ileLargeur, _ileProfondeur, _forcePerlinCarte * _forcePerlinVariants);

        AfficherIle(uneCarte, uneCarteBiomes, uneCarteVariants);
    }

    /// <summary>
    /// Fonction qui permet d'aller chercher des Objects dans un fichier du projet, peut importe son Type
    /// Le type est générer par Casting lorsque l'on créer une nouvelle Liste, ce qui va créer un tableau 2D
    /// </summary>
    private void LoadResources()
    {
        int nbBiomes = 1; // Nombre du biomes actuel
        int nbVariants = 1; // Nombre du variants actuel
        bool resteDesMats = true; // Vérifie si il reste des matériaux dans le fichier Resources.
        List<Material> tpBiome = new List<Material>(); // Liste qui contient les matériaux d'un biome.
        do
        {
            Object mats = Resources.Load("Mats/b" + nbBiomes + "_" + nbVariants); // Charge le fichier Resources avec les valeurs de Biomes et de Variants
            if (mats)
            {
                tpBiome.Add((Material)mats); // Ajoute le matériaux à la liste de matériaux du biome.
                nbVariants++; // Incrémente le nombre de variantes
            }
            else
            {
                if(nbVariants == 1)  // Si il n'y a plus de variantes
                {
                    resteDesMats = false; // On arrête la boucle
                }
                else
                {
                    biomesMats.Add(tpBiome); // Ajoute la liste de matériaux du biome à la liste de matériaux.
                    tpBiome = new List<Material>(); // Créer une nouvelle liste de matériaux.
                    nbBiomes++; // Incrémente le nombre de biomes
                    nbVariants = 1; // Réinitialise le nombre de variantes à 1
                }
            }
        } while(resteDesMats); // S'il reste des matériaux dans le fichier Resources, continue la boucle.
    }

    private void GenererCarte()
    {
        //Comme une création d'une liste, Création d'une mémoire qui peut stocker une valeur
        //dans chacun des objets du tableau (qui sera le Perlin -> _ileHauteur) (y)
        float[,] uneCarte = Terraformer(_ileLargeur, _ileProfondeur, _forcePerlinCarte);
        // En Changeant la forcePerlin de mes cartes Variantes il est possible d'ajouter de la plus grande variété dans une carte. (Variante Item)
        float[,] uneCarteVariante = Terraformer(_ileLargeur, _ileProfondeur, _forcePerlinVariants);
        // Création d'une map Perlin pour gérer les biomes
        float[,] uneCarteBiomes = Terraformer(_ileLargeur, _ileProfondeur, _forcePerlinBiomes*10);
        // uneCarte = Aquaforme(uneCarte);
        uneCarte = AquaformeCirculaire(uneCarte);
        AfficherIle(uneCarte, uneCarteVariante, uneCarteBiomes);
    }

    /// <summary>
    /// Doit envoyer une largeur, profondeur et une force perlin
    /// Perlin Noise prend les y près de lui pour conceptualiser une carte.
    /// </summary>
    /// <param name="largeur"></param>
    /// <param name="profondeur"></param>
    /// <param name="fP"></param>
    /// <returns></returns>
    private float[,] Terraformer(int largeur, int profondeur, float fP)
    {
        // Création d'un tableau de float contenant la largeur et la profondeur de la carte avec une valeur perlin à l'intérieur
        float[,] nouveauTerrain = new float[largeur, profondeur]; 
        int nouveauBruit = Random.Range(0, 100000); //Random.Range pour changer le Seeding de Unity
        for (int x = 0; x < largeur; x++)
        {
            for (int z = 0; z < profondeur; z++)
            {
                //PerlinNoise permet de générer un bruit de Perlin qui permet d'afficher la texture sur un Plain.
                //PerlinNoise peut retourner une valeur négative, donc on doit Clamp01, pour s'assurer de ne pas avoir d'erreur Index.
                float y = Mathf.Clamp01(Mathf.PerlinNoise((x / fP) + nouveauBruit, (z / fP) + nouveauBruit));
                //Ajoute la hauteur Perlin à la carte
                nouveauTerrain[x, z] = y;
            }
        }

        //Retourne le terrain
        return nouveauTerrain;
    }

    /// <summary>
    /// Fonction qui permet de créer une île rectangulaire avec une zone d'eau à l'extrémité.
    /// Aquaforme prend le tableau complet du terrain et fonction la hauteur de l'ile en 
    /// fonction de la distance entre le centre et l'extrémité.
    /// </summary>
    /// <param name="unTerrain"></param>
    /// <returns></returns>
    private float[,] Aquaforme(float[,] unTerrain)
    {
        //GetLenght permet de déterminer la largeur(x) et la profondeur(z) de l'île à l'intérieur du Tableau.
        int largeur = unTerrain.GetLength(0); // Va chercher la largeur de l'île.
        int profondeur = unTerrain.GetLength(1); // Va chercher la profondeur de l'île.
        //Double boucle qui permet de parcourir le terrain
        for (int x = 0; x < largeur; x++)
        {
            for (int z = 0; z < profondeur; z++)
            {
                //Calcul de la distance entre le centre et l'extrémité
                float distanceX = x / (float)largeur * 2 - 1; // Selon la largeur de l'île, la distance va être de 0 à 1.
                float distanceZ = z / (float)profondeur * 2 - 1; // Selon la profondeur de l'île, la distance va être de 0 à 1.

                float valeurMax = Mathf.Max(Mathf.Abs(distanceX), Mathf.Abs(distanceZ));//Calcul de la valeur max entre la distance X et la distance Z (0 à 1)
                valeurMax = FonctionSigmoide(valeurMax); // Instaure la valeur maximum à l'intérieur de la fonction pour trouver son endroit dans la courbe en S.
                unTerrain[x, z] = Mathf.Clamp01(unTerrain[x, z] - valeurMax); // Enlève la valeur Y du terrain 
            }
        }

        //Retourne le terrain
        return unTerrain;
    }

    /// <summary>
    /// Fonction qui permet de créer une île circulaire avec une zone d'eau à l'extrémité.
    /// Aquaforme prend le tableau complet du terrain et fonction la hauteur de l'ile en 
    /// fonction de la distance entre le centre et l'extrémité.
    /// </summary>
    /// <param name="unTerrain"></param>
    /// <returns></returns>
    private float[,] AquaformeCirculaire(float[,] unTerrain)
    {
        //GetLenght permet de déterminer la largeur(x) et la profondeur(z) de l'île à l'intérieur du Tableau.
        int largeur = unTerrain.GetLength(0); // Va chercher la largeur de l'île.
        int profondeur = unTerrain.GetLength(1); // Va chercher la profondeur de l'île.
        //Double boucle qui permet de parcourir le terrain
        for (int x = 0; x < largeur; x++)
        {
            for (int z = 0; z < profondeur; z++)
            {
                //Calcul de la distance entre le centre et l'extrémité
                float distanceX = x / (float)largeur * 2 - 1; // Sur la largeur
                float distanceZ = z / (float)profondeur * 2 - 1; // Sur la profondeur
                //Calcul avec Pythagore pour calculer la distance entre le centre et l'extrémité sur X et Z
                float valeurMax = Mathf.Sqrt(distanceX * distanceX + distanceZ * distanceZ);
                // Prend la valeur max précedement calculée et l'utilise pour la fonction sigmoide pour trouver son endroit dans la courbe en S.
                valeurMax = FonctionSigmoide(valeurMax);
                //Clamp la valeur entre 0 et 1 pour la valeur qui sera donné à l'emplacement dans le tableau.
                unTerrain[x, z] = Mathf.Clamp01(unTerrain[x, z] - valeurMax);
            }
        }

        //Retourne le terrain
        return unTerrain;
    }
    /// <summary>
    /// Fonction avec une double boucle qui permet de générer la texture d'une île en X et en Z
    /// </summary>
    private void AfficherIle(float[,] uneCarte, float[,] uneCarteVariante, float[,] uneCarteMeteo)
    {
        //GetLenght permet de déterminer la largeur(x) et la profondeur(z) de l'île à l'intérieur du Tableau.
        int largeur = uneCarte.GetLength(0); // Va chercher la largeur de l'île.
        int profondeur = uneCarte.GetLength(1); // Va chercher la profondeur de l'île.

        //Création d'une texture 2D à partir de la largeur et de la profondeur de l'île.
        Texture2D texture = new Texture2D(largeur, profondeur);

        //Double boucle qui permet de parcourir le terrain
        for (int x = 0; x < largeur; x++)
        {
            for (int z = 0; z < profondeur; z++)
            {
                float y = uneCarte[x, z];

                //Création d'une nouvelle couleur à partir de la valeur de l'île (noir jusqu'a blanc)
                Color nouvelleCouleur = new Color(y, y, y);
                // Applique la couleur au plane qui détient la texture créer de l'île.
                texture.SetPixel(x, z, nouvelleCouleur);

                if (y > 0)
                {
                    // Instantier un cube à un endroit en fonction du tableau (leTerrain) qui aurait une hauteur selon la valeur contenu dans la coordonnées ainsi que le Coefficient de la valeur Y pour la hauteur.
                    GameObject leCube = Instantiate(_prefabCube, transform.position + new Vector3(x, y * _ileCoefHauteur, z) - new Vector3(largeur / 2, 0f, profondeur / 2), Quaternion.identity);

                    int quelBiome = Mathf.RoundToInt(uneCarteMeteo[x,z] * (biomesMats.Count-1)); //Prend la valeur y de la carte et la multiplie par le nombre de Biomes dans la liste.
                    int quelVariant = Mathf.RoundToInt(uneCarteVariante[x, z] * (biomesMats[quelBiome].Count-1)); //Prend la valeur d'une carte variante, qui est une deuxième map perlin pour créer une deuxième variété dans l'île, il est possible de le faire avec une multitude d'autre paramètre
                    leCube.GetComponent<Renderer>().material = biomesMats[quelBiome][quelVariant]; // Prend le component qui fait le rendu du Cube pour instaurer la couleur.
                    leCube.AddComponent<BoxCollider>(); //Ajoute un collider au cube
                    leCube.transform.parent = transform; // Ajoute les cubes instantier au Empty GameObject du parent
                    leCube.gameObject.name = "Cube (" + x + ", " + y + ", " + z + ")"; //Nomme le cube par sa position.
                    _listeCube.Add(leCube); //Ajoute le cube dans une liste

                    if(Random.value*100 <= _fertile) //Si la valeur aléatoire est inférieur ou égal à fertile, instantie un objet
                    {
                        string itemRandom = Random.value>0.5 ? "c" : "s"; // 50/50 d'aller chercher un cube ou une sphère
                        float taille = Random.Range(0.5f, 1.5f); // Créer une taille aléatoire à l'objet que l'on va instantier

                        // Référence l'item Instantier en allant chercher avec le ressources les items à l'intérieur de mon Dossier Item dans Ressources, Ajouter +1 au variable (quelBiome et quelVariant) pour avoir accès au bon Item (Doit ajouter 1 vu que le résultat commencera à 0)
                        GameObject unItem = Instantiate((GameObject)Resources.Load("Items/" + itemRandom +  "" + (quelBiome+1) + "_" + (quelVariant+1)), new Vector3(leCube.transform.position.x, leCube.transform.position.y + taille, leCube.transform.position.z), Quaternion.identity);

                        unItem.transform.localScale = Vector3.one * taille; //Transforme le localScale de l'objet en fonction de la taille générer aléatoirement
                        unItem.transform.rotation = Random.rotation; //Génère une rotation aléatoire sur 360 avec la fonction Random.rotation
                        unItem.transform.parent = leCube.transform; //Met l'objet dans la hiérarchie en dessous du Cube sur lequel l'objet est positionner.
                    }
                }
            }
        }

        //Applique la texture sur le plain
        texture.Apply();
        _texturePlane.sharedMaterial.mainTexture = texture; // Va chercher la matériel du plane et lui donne la texture créer.
    } 

    /// <summary>
    /// Fonction Mathématique qui permet d'avoir une valeur qui se situe entre 0 et 1 pour donnée le placement du cube s'il fait parti de l'île ou l'océan.
    /// </summary>
    /// <param name="valeur"></param>
    /// <returns></returns>
     private float FonctionSigmoide(float valeur)
    {
        //k = Pente / (Degré d'immersion de l'île)
        //C = Centre de l'effet du dégradé (Plus c'est haut plus on se rapproche de l'île) / (Pourcentage de l'île hors de l'eau)
        //Mathf.Exp = e (Base naturel = 2.72)
        return 1 / (1 + Mathf.Exp(-k * (valeur - c)));
    }
}
