using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Geyser : MonoBehaviour
{
    public float lifetime;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Fade());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Ennemi"))
        {
            other.GetComponent<Ennemi>().PriseDegatEnnemi(1);
        }
    }

    private IEnumerator Fade()
    {
        float time=0;
        while(true)
        {
            if(time>=lifetime)
            {
                Destroy(gameObject);
            }
            time++;
            yield return new WaitForSeconds(1f);
        }
    }
}
