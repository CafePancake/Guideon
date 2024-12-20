using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PNJ : Interactable
{
    AudioManager audioManager; //manager qui s<occupe du audio
    AudioSource _audio; //audiosource du pnj
    public AudioClip[] talkSouds; //array de sons a jouer quand le pnj parle
    public AudioClip completionSound; //son qui joue quand on parle au perso apres que tous les emblemes ont ete deposes a la stele
    public TextMeshPro dialogText; //zone de texte pour le dialogue
    public GameObject stele; //gameobject stele pour le suivi de progression
    public GameObject portal; //gameobject portail qui sert a finir la partie
    public bool isTalking = false; //est-ce que le pnj est deja en train d<afficher un message
    public string completedSteleText = "Enfin! Je vais pouvoir sortir d'ici et détruire le monde!"; //texte si le joueur a complete tous les objctif de ls stele
    public string imcompleteSteleText = "Tu dois offrir 3 amulettes à la stèle avant que je ne puisse ouvrir le portail"; //texte si reste des objectifs de stele


    void Start()
    {
        audioManager = AudioManager.Instance;
        _audio = GetComponent<AudioSource>();
    }

    public override void Interact()
    {
        if (!isTalking) //si pas deja en train de parler
        {
            GetComponent<Animator>().SetTrigger("talk");
            bool steleCompleted = stele.GetComponent<Stele>().isComplete; //regarde si stele est complete
            if (steleCompleted)
            {
                GiveReward(); //si oui recompense le joueur
                audioManager.Play(completionSound);
            }
            StartCoroutine(DisplayMessage(steleCompleted)); //donne le message correspondant au state de completion de la stele
            StartCoroutine(MakeTalkingSounds());
        }
    }

    /// <summary>
    /// recompense le joueur quand stele complete
    /// </summary>
    public void GiveReward()
    {
        portal.SetActive(true); //ouvre le portail
    }

/// <summary>
/// fais apparaitre un message durant un certain temps
/// </summary>
/// <param name="isSteleComplete"> bool state de completeion de la stele </param>
/// <returns></returns>
    private IEnumerator DisplayMessage(bool isSteleComplete)
    {
        isTalking = true; //commence a parler
        if (isSteleComplete)
        {
            dialogText.text = completedSteleText; //message correspondant au state de completion de stele
        }
        else{
            dialogText.text = imcompleteSteleText;
        }

        yield return new WaitForSeconds(6f); //attend 8 sec

        dialogText.text = ""; //vide texte dialogue
        isTalking = false; //arrete de parler
    }

    /// <summary>
    /// coroutine qui produit des sons tant que le pnj parle, permet d<avoir de la variation dans les 'voicelines'
    /// </summary>
    private IEnumerator MakeTalkingSounds()
    {
        while(isTalking)
        {
            _audio.PlayOneShot(talkSouds[Random.Range(0, talkSouds.Length)]); //fait jouer un des clips de sons aleatoire
            yield return new WaitForSeconds(2f); //attends 2 sec avant de jouer un autre clip
        }
    }
}
