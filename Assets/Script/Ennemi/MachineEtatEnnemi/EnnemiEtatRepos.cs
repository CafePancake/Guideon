using System.Collections;
using System.IO.Ports;
using UnityEngine;

public class EnnemiEtatRepos : EnnemiEtatsBase
{
    // Classe qui est de type Abstrait à cause du lien avec le BiomesEtatsBase, ce qui lui permet de reprendre les différentes
    // méthodes de la classe BiomesEtatsBase, pour les modifier selon les besoins.
    private float maxDistanceToPlayer = 7f;
    private Transform playerTransform;

    /// <summary>
    /// Fonction qui sera appeler lorsque le Biome est créer qui aura pour but d'initialiser les différentes variables du Biome.
    /// </summary>
    /// <param name="ennemi"></param>
    public override void InitEtat(EnnemiEtatManager ennemi)
    {
        playerTransform = GameObject.FindGameObjectWithTag("Perso").transform;
        
        Debug.Log("InitEtat Repos");
        ennemi.anim.SetBool("enCourse", false);
        ennemi.anim.SetBool("enAttaque", false);
    }

    /// <summary>
    /// État qui sera appeler à chaque frame (60x Secondes).
    /// </summary>
    /// <param name="ennemi"></param>
    public override void UpdateEtat(EnnemiEtatManager ennemi)
    {
        // Prend en float la distance entre l'ennemi et le joueur
        float distanceToPlayer = Vector3.Distance(ennemi.transform.position, playerTransform.position);

        // Si le joueur est assez près de l'ennemi, il passe en mode attaque
        if (distanceToPlayer < maxDistanceToPlayer)
        {
            Debug.Log("ennemi chase");
            ennemi.anim.SetBool("enCourse", true);
            ennemi.anim.SetBool("enAttaque", false);
            ennemi.StopAllCoroutines();
            ennemi.ChangerEtat(ennemi.chasse);
        }
    }

    /// <summary>
    /// État qui sera appeler lorsque le Collider du joueur est entré dans le Trigger d'un Biome.
    /// </summary>
    /// <param name="ennemi"></param>
    public override void TriggerEnterEtat(EnnemiEtatManager ennemi, Collider other)
    {
        if (other.CompareTag("Perso")) ChangerPerso(ennemi);
    }

    private void ChangerPerso(EnnemiEtatManager ennemi)
    {
        ennemi.anim.SetBool("enCourse", true);
        ennemi.StopAllCoroutines();
        ennemi.ChangerEtat(ennemi.chasse);
    }
}
