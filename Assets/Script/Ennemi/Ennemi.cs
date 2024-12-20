using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ennemi : MonoBehaviour
{
    // Référence sur Ennemi concernant ces points de vie
    [Header("Référence sur Ennemi")]
    [SerializeField, Range(0, 100)] public float vieActuelle; // Stock de façon publique la vie actuelle de l'ennemi, pour activer certain état
    [SerializeField, Range(0, 100)] public float vieMaximum; // Stock de façon publique la vie maximum de l'ennemi

    // Référence Sonore de l'ennemi lors de l'attaque ou de la mort de celui-ci
    [Header("Référence Son")]
    [SerializeField] AudioSource _audioSource; // Source audio de l'ennemi pour jouer ces propres son
    [SerializeField] AudioClip _sonPriseDegatEnnemi; // Son de l'ennemi lorsqu'il subit un coup
    [SerializeField] AudioClip _sonMortEnnemi; // Son de l'ennemi lorsqu'il meurt
    [SerializeField] public AudioClip _sonAttaqueEnnemi; // Son de l'ennemi lorsqu'il attaque

    [Header("Référence UI")]
    [SerializeField] Image _barreDeVie;

    Animator _anim;

    void Awake()
    {
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        vieActuelle = vieMaximum;
        AjusterVieEnnemi(vieActuelle, vieMaximum);
    }

    public void AjusterVieEnnemi(float currentHealth, float maxHealth)
    {
        _barreDeVie.fillAmount = (float)currentHealth / (float)maxHealth;
    }

    public void PriseDegatEnnemi(int damage)
    {
        vieActuelle -= damage;
        _audioSource.PlayOneShot(_sonPriseDegatEnnemi, .1f);
        AjusterVieEnnemi(vieActuelle, vieMaximum);
    }

    void Update()
    {
        if (vieActuelle <= 0)
        {
            _anim.SetBool("mort", true);
            StartCoroutine(AnimationDeMortEnnemi());
        }
    }


    private IEnumerator AnimationDeMortEnnemi()
    {
        yield return new WaitForSeconds(2f);
        _audioSource.PlayOneShot(_sonMortEnnemi, .2f);
        Vector3 scaleDeDepart = transform.localScale;
        Vector3 scaleFinal = Vector3.zero;
        float temps = 0;
        float duration = 2f; // Duration of scale down animation
        while (temps < duration)
        {
            temps += Time.deltaTime;
            float progress = temps / duration;
            transform.localScale = Vector3.Lerp(scaleDeDepart, scaleFinal, progress);
            yield return null;
        }

        Destroy(gameObject);
        yield break;
    }

}
