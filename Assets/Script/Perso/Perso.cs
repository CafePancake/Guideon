using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Perso : MonoBehaviour
{
    [Header("Référence UI")]
    [SerializeField] HealthBar _healthBar; // Référence de la barre de vie du joueur
    [SerializeField] GameObject _FireImage; // Référence de l'image du sort feu, qui sera affichée lorsque le sort feu est actif
    [SerializeField] GameObject _AirImage; // Référence de l'image du sort air, qui sera affichée lorsque le sort air est actif
    [SerializeField] GameObject _EarthImage; // Référence de l'image du sort terre, qui sera affichée lorsque le sort terre est actif
    [SerializeField] GameObject _WaterImage; // Référence de l'image du sort d'eau, qui sera affichée lorsque le sort d'eau est actif
    [SerializeField] TextMeshProUGUI _textFireCD; // Référence du texte qui affichera le temps restant avant de pouvoir utiliser le sort feu
    [SerializeField] TextMeshProUGUI _textAirCD; // Référence du texte qui affichera le temps restant avant de pouvoir utiliser le sort air
    [SerializeField] TextMeshProUGUI _textEarthCD; // Référence du texte qui affichera le temps restant avant de pouvoir utiliser le sort terre
    [SerializeField] TextMeshProUGUI _textWaterCD; // Référence du texte qui affichera le temps restant avant de pouvoir utiliser le sort eau

    [Header("Référence De Jeu")]
    [SerializeField, Range(0, 100)] int _currentHealth; // Stock la vie actuelle du joueur
    [SerializeField, Range(0, 100)] int _maxHealth; // Stock la vie maximum du joueur
    [SerializeField, Range(0, 50)] int _currentShield; // Stock le bouclier actuel du joueur
    [SerializeField, Range(0, 50)] int _maxShield; // Stock le bouclier maximum du joueur

    [Header("Référence Sort Joueur Code")]
    [SerializeField] public bool _isFireActive; // Garde en référence si le sort de feu est actif, donc si le joueur possède le sort
    [SerializeField, Range(0,15)] float _cooldownFire; // Garde en stock le temps de chargement du sort de feu 
    [SerializeField] public bool _isAirActive; // Garde en référence si le sort d'air est actif, donc si le joueur possède le sort
    [SerializeField, Range(0,15)] float _cooldownAir; // Garde en stock le temps de chargement du sort d'air
    [SerializeField] public bool _isEarthActive; // Garde en référence si le sort de terre est actif, donc si le joueur possède le sort
    [SerializeField, Range(0,15)] float _cooldownEarth; // Garde en stock le temps de chargement du sort de terre
    [SerializeField] public bool _isWaterActive; // Garde en référence si le sort d'eau est actif, donc si le joueur possède le sort
    [SerializeField, Range(0,15)] float _cooldownWater; // Garde en stock le temps de chargement du sort 
    [SerializeField] Navigation _nav;

    [Header("Référence Sort Joueur InGame")]
    [SerializeField] GameObject _fireSpell; // Garde en préfab le sort de feu pour l'instancier dans la scène, donc un sort de zone autour du joueur
    [SerializeField] GameObject _earthSpell; // Garde en préfab le sort de terre pour l'instancier dans la scène, donc un bouclier pour le joueur
    [SerializeField] GameObject _airSpell; // Garde en préfab le sort d'air pour l'instancier dans la scène, donc un dash pour le joueur
    [SerializeField] GameObject _waterSpell; // Garde en préfab le sort d'eau pour l'instancier dans la scène, donc un sort de zone qui effectue des dégats en continue

    [Header("Référence Son")]
    [SerializeField] AudioSource _audioSource; // Source audio du joueur pour jouer ces sons
    [SerializeField] AudioClip _gameOverSound; // Son de game over
    [SerializeField] AudioClip _hitSound; // Son de game over
    [SerializeField] AudioClip _fireSpellSound; // Son du sort feu
    [SerializeField] AudioClip _earthSpellSound; // Son du sort terre
    [SerializeField] AudioClip _airSpellSound; // Son du sort air
    [SerializeField] AudioClip _waterSpellSound; // Son du sort eau

    Animator _animator; // Référence de l'animator du joueur pour gérer les animations

    void Awake()
    {
        _animator = GetComponent<Animator>(); // Récupère l'animator du joueur

        _currentHealth = _maxHealth; // Initialise la vie du joueur à sa valeur maximum
        _healthBar.SetMaxHealth(_maxHealth); // Initialise la barre de vie du joueur

        _currentShield = 0; // Initialise le bouclier du joueur à 0
        _healthBar.SetShield(_currentShield); // Initialise la barre de bouclier du joueur

        ResetUI(); // Réinitialise l'interface utilisateur
    }

    /// <summary>
    /// Fonction pour réinitialiser l'interface utilisateur complet lorsque le joueur débute le jeu
    /// </summary>
    void ResetUI()
    {

        _EarthImage.GetComponent<Image>().color = new Color(1, 1, 1, 0.1f);
        _FireImage.GetComponent<Image>().color = new Color(1, 1, 1, 0.1f);
        _AirImage.GetComponent<Image>().color = new Color(1, 1, 1, 0.1f);
        _WaterImage.GetComponent<Image>().color = new Color(1, 1, 1, 0.1f);
        _cooldownEarth = 0;
        _cooldownFire = 0;
        _cooldownAir = 0;
        _cooldownWater = 0;
        _textEarthCD.text = _cooldownEarth.ToString();
        _textFireCD.text = _cooldownFire.ToString();
        _textAirCD.text = _cooldownAir.ToString();
        _textWaterCD.text = _cooldownWater.ToString();
        _textEarthCD.gameObject.SetActive(false);
        _textFireCD.gameObject.SetActive(false);
        _textAirCD.gameObject.SetActive(false);
        _textWaterCD.gameObject.SetActive(false);
    }

    void Start()
    {
        // Appel la fonction SpellCooldown toutes les secondes pour réduire le CD des sorts si le joueur à débloquer le sort.
        _audioSource = GetComponent<AudioSource>();
        InvokeRepeating("SpellCooldown", 0, 1);
    }

    void Update()
    {
        // Si le joueur appuie sur la touche Q et que le sort est débloqué et que le CD est à 0, alors le sort de feu est lancé
        if (Input.GetKeyDown(KeyCode.Q) && _isFireActive && _cooldownFire <= 0)
        {
            FireSpell();
        }
        // Si le joueur appuie sur la touche Q et que le sort est débloqué et que le CD est  à 0, alors le sort d'air est lancé
        if (Input.GetKeyDown(KeyCode.E) && _isAirActive && _cooldownAir <= 0)
        {
            AirSpell();
        }
        // Si le joueur appuie sur la touche Q et que le sort est débloqué et que le CD est à 0, alors le sort de terre est lancé
        if (Input.GetKeyDown(KeyCode.R) && _isEarthActive && _cooldownEarth <= 0)
        {
            EarthSpell();
        }
        if (Input.GetKeyDown(KeyCode.T) && _isWaterActive && _cooldownWater <= 0)
        {
            WaterSpell();
        }	
    }

    /// <summary>
    /// Fonction qui lance un Sort de feu autour du joueur pour infliger des dégats aux ennemis,
    /// ceci ce lance uniquement si le joueur à débloquer le sort et que celui-ci peut être lancer (CD à 0)
    /// </summary>
    void FireSpell()
    {
        _audioSource.PlayOneShot(_fireSpellSound);

        // Définir les variables pour le sort de feu
        float spellRadius = 5f;
        int fireDamage = 20;

        // Get all colliders within a sphere around the player
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, spellRadius);

        // Instantie le fireSpell à la position du joueur
        GameObject fireSpell = Instantiate(_fireSpell, transform.position, Quaternion.identity);
        fireSpell.transform.rotation = Quaternion.Euler(-90, 0, 0);

        // Check each collider
        foreach (Collider hitCollider in hitColliders)
        {
            // If the collider has an Ennemi component
            Ennemi ennemi = hitCollider.GetComponent<Ennemi>();
            if (ennemi != null)
            {
                // Deal damage to the enemy
                ennemi.PriseDegatEnnemi(fireDamage);
            }
        }

        Destroy(fireSpell, 1.5f);

        // Set the cooldown of the fire spell
        _cooldownFire = 5;
    }


    /// <summary>
    /// Fonction qui lance un Dash vers l'avant pour le joueur, ceci ce lance
    /// uniquement si le joueur à débloquer le sort et que celui-ci peut être lancer (CD à 0) 
    /// </summary>
    void AirSpell()
    {
        // Lance le Dash du joueur vers l'avant
        StartCoroutine(CoroutineDash());
        // Set le cooldown du sort d'air à 10 secondes pour empêcher le joueur de l'utiliser le dash trop souvent
        _cooldownAir = 10;
    }

    /// <summary>
    /// Fonction qui donne un bouclier au joueur pour le protéger des dégats, 
    /// ceci ce lance uniquement si le joueur à débloquer le sort et que celui-ci peut être lancer (CD à 0)
    /// </summary>
    void EarthSpell()
    {
        // Set le bouclier du joueur à la valeur maximale
        _currentShield = _maxShield;
        _healthBar.SetShield(_currentShield);
        // Démontrer au joueur que le sort à été lancé.
        _earthSpell.SetActive(true);
        _audioSource.PlayOneShot(_earthSpellSound);
        // Set le cooldown du sort de terre à 10 secondes pour empêcher le joueur de l'utiliser trop souvent
        _cooldownEarth = 15;
    }

    void WaterSpell()
    {
        // Aucune ligne pour l'instant
        GameObject waterSpell = Instantiate(_waterSpell, transform.position, Quaternion.identity);
        Destroy(waterSpell, 5f);
        _cooldownWater = 20;
        _audioSource.PlayOneShot(_waterSpellSound);
    }

    /// <summary>
    /// Fonction qui sera utiliser par InvokeRepeating pour réduire le CD des sorts si le joueur à débloquer le sort.
    /// </summary>
    void SpellCooldown()
    {
        // Si le joueur à débloquer le sort de feu, alors on réduit le CD du sort de feu
        if (_isFireActive)
        {
            // Montre le texte de cooldown du sort de feu
            _textFireCD.gameObject.SetActive(true);
            _cooldownFire--;
            if (_cooldownFire < 0)
            {
                // Si le CD est en dessous de 0, alors on le remet à 0 et on cache le texte de cooldown
                _cooldownFire = 0;
                _textFireCD.gameObject.SetActive(false);
                _FireImage.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
            }
            else{
                // Sinon on affiche le texte de cooldown restant au sort
                _FireImage.GetComponent<Image>().color = new Color(1, 1, 1, 0.1f);
            }
            // Mettre à jour le texte de cooldown
            _textFireCD.text = _cooldownFire.ToString();
        }
        // Si le joueur à débloquer le sort d'air, alors on réduit le CD du sort d'air
        if (_isAirActive)
        {
            // Montre le texte de cooldown du sort d'air
            _textAirCD.gameObject.SetActive(true);
            _cooldownAir--;
            if (_cooldownAir < 0)
            {
                // Si le CD est en dessous de 0, alors on le remet à 0 et on cache le texte de cooldown
                _cooldownAir = 0;
                _textAirCD.gameObject.SetActive(false);
                _AirImage.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
            }
            else{
                // Sinon on affiche le texte de cooldown restant au sort
                _AirImage.GetComponent<Image>().color = new Color(1, 1, 1, 0.1f);
            }
            // Mettre à jour le texte de cooldown
            _textAirCD.text = _cooldownAir.ToString();
        }
        // Si le joueur à débloquer le sort de terre, alors on réduit le CD du sort de terre
        if (_isEarthActive)
        {
            // Montre le texte de cooldown du sort de terre
            _textEarthCD.gameObject.SetActive(true);
            _cooldownEarth--;
            if (_cooldownEarth < 0)
            {
                // Si le CD est en dessous de 0, alors on le remet à 0 et on cache le texte de cooldown
                _cooldownEarth = 0;
                _textEarthCD.gameObject.SetActive(false);
                _EarthImage.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
            }
            else
            {
                // Sinon on affiche le texte de cooldown restant au sort
                _EarthImage.GetComponent<Image>().color = new Color(1, 1, 1, 0.1f);
            }
            // Mettre à jour le texte de cooldown
            _textEarthCD.text = _cooldownEarth.ToString();
        }
        // Si le joueur à débloquer le sort d'eau, alors on réduit le CD du sort d'eau
        if (_isWaterActive)
        {
            // Montre le texte de cooldown du sort d'eau
            _textWaterCD.gameObject.SetActive(true);
            _cooldownWater--;
            if (_cooldownWater < 0)
            {
                // Si le CD est en dessous de 0, alors on le remet à 0 et on cache le texte de cooldown
                _cooldownWater = 0;
                _textWaterCD.gameObject.SetActive(false);
                _WaterImage.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
            }
            else
            {
                // Sinon on affiche le texte de cooldown restant au sort
                _WaterImage.GetComponent<Image>().color = new Color(1, 1, 1, 0.1f);
            }
            // Mettre à jour le texte de cooldown
            _textWaterCD.text = _cooldownWater.ToString();
        }
    }

    public void PriseDegat(int degat)
    {
        // Si le joueur à un bouclier, celui-ci sera utiliser pour réduire les dégats avant de déduire les dégats au joueur.
        if (_currentShield > 0)
        {
            // On réduit les dégats du bouclier
            _currentShield -= degat;
            if (_currentShield < 0)
            {
                // Si le bouclier est en dessous de 0, alors on le remet à 0 et on ajoute les dégats au joueur
                _currentHealth += _currentShield;
                _currentShield = 0;
                _earthSpell.SetActive(false);
            }
            // On remet à jour les barres de vie et de bouclier
            _healthBar.SetShield(_currentShield);
            _healthBar.SetHealth(_currentHealth);
        }
        // Sinon le joueur perdra des PV
        else
        {
            _audioSource.PlayOneShot(_hitSound);
            _currentHealth -= degat;
            _healthBar.SetHealth(_currentHealth);
        }

        // Si le joueur à 0 PV, la partie est terminée
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            _animator.SetBool("mort", true);
            _audioSource.PlayOneShot(_gameOverSound);
            Invoke("GameOver", 2f);
        }
    }

    private void GameOver()
    {
        _nav.DefaiteScene();
    }

    /// <summary>
    /// Coroutine qui sera utiliser par AirSpell pour faire un Dash vers l'avant pour le joueur à Usage unique,
    /// donc la coroutine ne peut être lancer qu'une seule fois ensuite le cooldown commence.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoroutineDash()
    {
        GameObject dashEffect = Instantiate(_airSpell, transform.position, Quaternion.identity);
        _audioSource.PlayOneShot(_airSpellSound);

        CharacterController controller = GetComponent<CharacterController>();

        if (controller == null)
        {
            Debug.LogError("CharacterController is missing!");
            yield break;
        }

        float dashSpeed = 20f;        // Vitesse du dash
        float dashDuration = 0.2f;   // Durée du dash
        float elapsedTime = 0f;      // Temps écoulé pendant le dash

        Vector3 dashDirection = transform.forward; // Direction du dash

        while (elapsedTime < dashDuration)
        {
            // Applique le déplacement dans la direction du dash
            controller.Move(dashDirection * dashSpeed * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return null; // Attendre le prochain frame
        }

        Destroy(dashEffect);
        yield break;
    }

}
