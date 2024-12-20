using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class BougeAgent : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] UnityEngine.AI.NavMeshAgent _agent; // Référence du NavMeshSurface de l'agent ce qui nous permet de lui attribuer des valeurs à certaines fonctions.

    [Header("Variable")]
    [SerializeField] Transform _perso; // On prend le transform du personnage pour lui attribuer une cible.
    [SerializeField] Transform _base; // On prend le transform de base de l'île donc 0,0,0
    [SerializeField] Transform _goal; // Répertorie le transform du personnage dans une variable privée, donc la cible de l'ennemi.

    void Start()
    {
        _goal = _perso; // Le premier but de l'agent est de ce diriger vers le personnage.
        InvokeRepeating("EnChasse", 3f, 0.2f); // Appel la fonction en chasse après 3 secondes et rappel cette fonction chaque 0,2 secondes.
    }

    /// <summary>
    /// Fonction qui défini l'ÉtatDeChasse de l'ennemi.
    /// </summary>
    public void EnChasse()
    {
        _agent.destination = _goal.position;
        if(_agent.remainingDistance > 1f || _agent.pathPending) // Si l'agent est à une distance plus grande qu'un unité Unity et qu'il n'est pas entrain d'analyser une nouvelle trajectoire
        {
            _agent.destination = _goal.position; // Continue de ce diriger vers son but, donc vers le personnage ou vers le centre de l'île
        }
        else if (_agent.remainingDistance < 1f && !_agent.pathPending) // Si l'agent est à une distance plus basse qu'un unité Unity et qu'il n'est pas entrain de faire une recherche de trajet.
        {
            if(_goal == _perso) // Si le but est d'atteindre le personnage, celui retourne au milieu de l'ile
            {
                _goal = _base; // Se dirige désormais vers le centre de l'île
            }
            else if (_goal == _base) // Si le but était d'atteindre le milieu de l'île, retourne vers le joueur.
            {
                _goal = _perso; // Se dirige désormais vers le personnage
            }
        }
    }
}
