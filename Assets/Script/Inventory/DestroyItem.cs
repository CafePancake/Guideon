using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyItem : MonoBehaviour
{
    [SerializeField] int _colliderCount = 0;
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {

            StartCoroutine(DestructionItem());
        } 
    }

    
    private IEnumerator DestructionItem()
    {
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;
        float elapsedTime = 0;
        float duration = 0.5f; // Duration of scale down animation

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progression = elapsedTime / duration;
            transform.localScale = Vector3.Lerp(startScale, endScale, progression);
            yield return null;
        }

        Destroy(gameObject);
        yield break;
    }
}
