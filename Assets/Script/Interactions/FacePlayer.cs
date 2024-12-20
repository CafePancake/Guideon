using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// classe donne a un texte de dialogue pour que lel texte fasse face au joueur (ou camera)
/// </summary>
public class FacePlayer : MonoBehaviour
{

    public Transform target; //ce a quoi on veut que le texte fasse face

    void Update()
    {
        Vector3 direction = (target.position - transform.position).normalized; //direction est la diff entre la pos et pos target
        direction.y = 0; // ne pas rotate verticalement
        transform.rotation = Quaternion.LookRotation(direction * -1); // rotation en fonction de la diff/rence des deux positions
    }
}
