using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// classe du portail
/// </summary>
public class Portail : MonoBehaviour
{
    public AudioClip _sonTraverse; //son quand le perso traverse le portail a la fin
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Guidon"))
        {
            SceneManager.LoadScene("Victoire"); //retroune au main menu, changer plus tard pour endscene
            AudioManager.Instance.Play(_sonTraverse);
        }
    }

}
