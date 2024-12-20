using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class MovePerso : MonoBehaviour
{
    [Header("Champ de force + Camera")]
    [SerializeField] private GameObject _champForce;
    [SerializeField] private Camera _camera;
    // [SerializeField] private int scaleActuel = 0;
    [SerializeField] private int multScaleVitesse = 2;

    [Header("Mouvement du personnage")]
    [SerializeField] private float _vitesse;
    [SerializeField] private float _vitesseMouvement = 20.0f;
    [SerializeField] private float _vitesseRotation = 3.0f;
    [SerializeField] private float _impulsionSaut = 30.0f;
    [SerializeField] private float _gravite = 0.2f;
    [SerializeField] private float _vitesseSaut;
    private Vector3 _directionsMouvement = Vector3.zero;

    [Header("Objectifs")]
    [SerializeField] private GameObject _astrid;

    [Header ("Référence Son")]
    [SerializeField] private AudioSource _son;
    [SerializeField] private AudioClip _sonMarche;
    [SerializeField] private AudioClip _sonSaut;


    Animator _animator;
    CharacterController _controller;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
        _son = GetComponent<AudioSource>();
        _camera.fieldOfView = 75;
        // _champForce.transform.localScale = new Vector3(scaleActuel, scaleActuel, scaleActuel);
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        StartCoroutine(ProduireSonsMarche());
    }

    /// <summary>
    /// Fonction qui permet d'augmenter le scale du champ de force du joueur lors de ses mouvements.
    /// Ainsi que de faire varier la position de la caméra en fonction du mouvement du personnage.
    /// </summary>
    void AjusterScaleChampForce()
    {
                _camera.fieldOfView = 90 + _vitesse*multScaleVitesse;
                // _champForce.transform.localScale = new Vector3(_vitesse*multScaleVitesse, _vitesse*multScaleVitesse, _vitesse*multScaleVitesse);
    }

    void Update()
    {
        AjusterScaleChampForce();

        //Permet au personnage d'effectuer une rotation sur l'axe Y en multipliant la valeur donner sur le joueur lorsqu'il appuie sur la touche (A ou D) par sa vitesse de rotation.
        transform.Rotate(0, Input.GetAxis("Horizontal") * _vitesseRotation, 0);
        // Permet d'avoir une vitesse au personnage à l'aide de la valeur donner par le joueur lorsqu'il appuie sur la touche (W ou S) ainsi que la valeur du mouvement du Personnage en les multipliants.
        _vitesse = Input.GetAxis("Vertical") * _vitesseMouvement;
        // Permet de mettre l'animation du joueur en course à l'aide de la valeur de vitesse en imposant une condition, si la vitesse est supérieur à 0, l'animation de course se met en marche.
        _animator.SetBool("enCourse", _vitesse > 0);
        // Permet de créer un Vector3 de position à l'aide de la vitesse du joueur en les mettants dans un Vector3 nommer _directionsMouvement.
        _directionsMouvement = new Vector3(0, 0, _vitesse);
        // Permet d'établir la direction du joueur à l'aide de la fonction TransformeDirection et la position de directionsMouvement en transformation leur position en direction à l'aide de la fonction.
        _directionsMouvement = transform.TransformDirection(_directionsMouvement);
        // Permet de faire sauté le personnage à l'aide des valeurs du CharacterController ainsi que la hauteur de Saut (vitesseSaut) et son Impulsion
        // en placant une condition qui dit que si le joueur est au Sol selon le Character controller et que le joueur à appuyer sur le bouton de Saut. Sa hauteur
        // sera remplacer par son impulsion de Saut.
        if (Input.GetButton("Jump") && _controller.isGrounded)
        {
            _vitesseSaut = _impulsionSaut;
            _son.PlayOneShot(_sonSaut);
        } 
        // Permet de mettre l'animation du joueur en mode Saut à l'aide des valeurs du Character controller ainsi que des valeurs de vitesse de saut et de l'impulsion
        // du personnage en placant une condition qui détecte si le joueur n'est pas au Sol grâce au Character controller et si la vitesse de saut est toujours 
        // supérieur à l'impulsion.
        _animator.SetBool("enSaut", !_controller.isGrounded && _vitesseSaut > -_impulsionSaut);
        // Permet d'ajouter la vitesse du Saut au direction du personnage à l'aide de vitesseSaut en Ajoutant progressivement sa vitesse au Vector3.
        _directionsMouvement.y += _vitesseSaut;
        // Permet d'affecter la hauteur du joueur à l'aide de la valeur de gravité ainsi que sa vitesse de Saut en placant une condition que si le joueur n'est pas
        // au sol réduit sa vitesse de saut par la gravité progressivement.
        if (!_controller.isGrounded) _vitesseSaut -= _gravite;
        // Premet au joueur de ce déplacer à l'aide de la fonction Move du Character controller ainsi que la direction de celui-ci ainsi que le temps en Frames
        // en Multipliant la direction du personnage par Time.deltaTime.
        _controller.Move(_directionsMouvement * Time.deltaTime);

         if (Input.GetMouseButton(1))
        {
            Vector3 directionObjectif = (_astrid.transform.position - transform.position).normalized;
            directionObjectif.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(directionObjectif);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _vitesseRotation * Time.deltaTime);
        }
    }
        private IEnumerator ProduireSonsMarche()
    {
        while (true)
        {
            if (_controller.isGrounded && _vitesse > 1)
            {
                AudioManager.Instance.Play(_sonMarche);
            }
            yield return new WaitForSeconds(0.3f);
        }
    }
}