using System.Collections;
using UnityEngine;

public class EnnemiEtatChasse : EnnemiEtatsBase
{
    // Distance maximale à laquelle l'ennemi poursuivra le joueur
    private float maxChaseDistance = 7f;
    // Référence à la position du joueur
    private Transform playerTransform;

    // Initialisation de l'état
    public override void InitEtat(EnnemiEtatManager ennemi)
    {
        Debug.Log("InitEtat Chasse");
        // Trouve le joueur dans la scène
        playerTransform = GameObject.FindGameObjectWithTag("Perso").transform;
        // Démarre la coroutine de poursuite
        ennemi.StartCoroutine(ChassePlayer(ennemi));
    }

    // Mise à jour de l'état
    public override void UpdateEtat(EnnemiEtatManager ennemi)
    {
        // Calcule la distance entre l'ennemi et le joueur
        float distanceToPlayer = Vector3.Distance(ennemi.transform.position, playerTransform.position);
        
        // Si le joueur est trop loin, retourne à l'état de repos
        if (distanceToPlayer > maxChaseDistance)
        {
            Debug.Log("ennemi chase");
            ennemi.anim.SetBool("enCourse", false);
            ennemi.StopAllCoroutines();
            ennemi.ChangerEtat(ennemi.repos);
        }
        // Si le joueur est trop proche, passe à l'état d'attaque
        if (distanceToPlayer < 2f)
        {
            ennemi.anim.SetBool("enAttaque", true);
            ennemi.anim.SetBool("enCourse", false);
            ennemi.StopAllCoroutines();
            ennemi.ChangerEtat(ennemi.attaque);
        }
    }

    // Gestion des collisions
    public override void TriggerEnterEtat(EnnemiEtatManager ennemi, Collider other)
    {
        // Ne rien faire ici, car la poursuite ne peut être déclenchée que lorsque l'ennemi est dans l'état de chasse
    }

    // Coroutine gérant la poursuite du joueur
    private IEnumerator ChassePlayer(EnnemiEtatManager ennemi)
    {
        while (true)
        {
            if (playerTransform != null)
            {
                // Calcul de la direction vers le joueur
                Vector3 lookDirection = playerTransform.position - ennemi.transform.position;
                lookDirection.y = 0; // Ignore la différence de hauteur
                
                // Rotation progressive vers le joueur
                if (lookDirection != Vector3.zero)
                {
                    Quaternion rotation = Quaternion.LookRotation(lookDirection);
                    ennemi.transform.rotation = Quaternion.Slerp(ennemi.transform.rotation, rotation, 0.1f);
                }

                // Déplacement vers le joueur avec une vitesse augmentée de 50%
                
                ennemi.transform.position = Vector3.MoveTowards(
                    ennemi.transform.position,
                    playerTransform.position,
                    ennemi.infos["vitesseDeplacement"] * 1.5f * Time.deltaTime
                );
            }
            yield return null; // Attend la prochaine frame
        }
    }
}

