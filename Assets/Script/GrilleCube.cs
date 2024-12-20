using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrilleCube : MonoBehaviour
{
    [Header("Instantiate Cube")]
    [SerializeField] GameObject _prefab;
    [SerializeField] Transform _parent;
    [SerializeField, Range(0, 100)] int _nbX;
    [SerializeField, Range(0, 100)] int _nbY;
    [SerializeField, Range(0, 100)] int _nbZ;
    [SerializeField, Range(1, 10)] float _espace;
    [SerializeField] List<GameObject> _listeCube = new List<GameObject>();

    [Header("Rotation")]
    [SerializeField] float _vitesse;

    void Start()
    {
        _listeCube.Clear();

        for (int x = -_nbX/ 2; x < _nbX/ 2; x++)
        {
            for (int y = -_nbY/ 2; y < _nbY/ 2; y++)
            {
                for (int z = -_nbZ / 2; z < _nbZ/ 2; z++)
                {
                    GameObject Cube = Instantiate(_prefab, new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z + z) * _espace, Quaternion.identity, _parent);
                    _listeCube.Add(Cube);
                }
            }
        }

        //Shuffle
        for (int i = 0; i < _listeCube.Count; i++)
        {
            GameObject temporaire = _listeCube[i];
            int randomIndex = Random.Range(i, _listeCube.Count);
            _listeCube[i] = _listeCube[randomIndex];
            _listeCube[randomIndex] = temporaire;
        }

        StartCoroutine(DetruirePrefab());
    }

    void Update()
    {
        //Rotation
        if (_listeCube.Count == 0)
        {
            StopAllCoroutines();
            return;
        }
        this.transform.Rotate(_vitesse, 0f, _vitesse, Space.Self);
    }

    IEnumerator DetruirePrefab()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            int alea = Random.Range(0, _listeCube.Count);
            Destroy(_listeCube[alea]);
            _listeCube.RemoveAt(alea);
        }
    }
}