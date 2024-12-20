using System.Collections;
using UnityEngine;

// Classe qui est de type Abstrait à cause du lien avec le EnnemiEtatsBase, ce qui lui permet de reprendre les différentes
// méthodes de la classe EnnemiEtatsBase, pour les modifier selon les besoins.
public class EnnemiEtatAttaque : EnnemiEtatsBase
{
    // Variables qui permettent de définir les différentes caractéristiques de l'attaque de l'ennemi
    private float attackRange = 2f;        // Distance à laquelle l'ennemi peut attaquer le joueur
    private int attackDamage = 20;      // Quantité de dégâts que l'ennemi inflige au joueur
    private float attackDuration = 2f;   // Durée de l'animation d'attaque
    private Transform playerTransform;      // Référence vers la position du joueur
    private Perso playerScript;

    /// <summary>
    /// Fonction qui sera appeler lorsque l'ennemi entre dans l'état d'attaque.
    /// </summary>
    /// <param name="ennemi">Référence vers le gestionnaire d'état de l'ennemi</param>
    public override void InitEtat(EnnemiEtatManager ennemi)
    {
        Debug.Log("InitEtat Attaque");
        // Recherche et stocke la référence du joueur
        playerTransform = GameObject.FindGameObjectWithTag("Guidon").transform;
        playerScript = GameObject.FindGameObjectWithTag("Guidon").GetComponent<Perso>();
        // Démarre la séquence d'attaque
        ennemi.StartCoroutine(SequenceAttaque(ennemi));
    }

    /// <summary>
    /// État qui sera appeler à chaque frame pour vérifier la distance avec le joueur
    /// </summary>
    /// <param name="ennemi">Référence vers le gestionnaire d'état de l'ennemi</param>
    public override void UpdateEtat(EnnemiEtatManager ennemi)
    {
        // Calcule la distance entre l'ennemi et le joueur
        float distanceToPlayer = Vector3.Distance(ennemi.transform.position, playerTransform.position);
        
        // Si le joueur s'éloigne trop, l'ennemi retourne en mode poursuite
        if (distanceToPlayer > 2.5f)
        {
            ennemi.anim.SetBool("enAttaque", false);
            ennemi.anim.SetBool("enCourse", true);
            ennemi.ChangerEtat(ennemi.chasse);
        }
    }

    public override void TriggerEnterEtat(EnnemiEtatManager ennemi, Collider other)
    {
        // Ne rien faire ici, car l'attaque ne peut être déclenchée que lorsque l'ennemi est dans l'état d'attaque
    }

    /// <summary>
    /// Coroutine qui gère la séquence d'attaque de l'ennemi
    /// </summary>
    /// <param name="ennemi">Référence vers le gestionnaire d'état de l'ennemi</param>
    private IEnumerator SequenceAttaque(EnnemiEtatManager ennemi)
    {
        Debug.Log("SequenceAttaque");
        while (true)
        {
            if (ennemi.infos["ennemiLife"] <= 0)
            {
                yield break;
            }
            // Fait tourner l'ennemi vers le joueur pendant l'attaque
            Vector3 lookDirection = playerTransform.position - ennemi.transform.position;
            lookDirection.y = 0; // Garde la rotation sur l'axe Y uniquement
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            ennemi.transform.rotation = targetRotation;

            // Attend la durée de l'animation d'attaque
            yield return new WaitForSeconds(attackDuration);

            // Vérifie si le joueur est toujours à portée d'attaque
            float distanceToPlayer = Vector3.Distance(ennemi.transform.position, playerTransform.position);
            if (distanceToPlayer <= attackRange)
            {
                if (playerScript != null)
                {
                    Debug.Log("Attaque ennemi");
                    playerScript.PriseDegat(attackDamage);
                }
            }

            // Attend un court délai avant la prochaine attaque
            yield return new WaitForSeconds(0.5f);
        }
    }
}
