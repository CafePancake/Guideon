using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupNotif : MonoBehaviour
{
    public float moveDistance = 0.5f; // Distance the panel moves up
    public float duration = 1f;    // Duration of the animation

    private CanvasGroup canvasGroup; // To handle opacity
    private Vector3 startPosition;  // Starting position of the panel

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        startPosition = transform.position;
        StartCoroutine(FadeAndMove()); //des le debut commence a bouger et fade
    }

    private IEnumerator FadeAndMove()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            // bouge panel vers haut
            transform.position = Vector3.Lerp(startPosition, startPosition + Vector3.up * moveDistance, t);

            // reduit opacite
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //verifie que les valeurs soient comme voulu a la fin
        transform.position = startPosition + Vector3.up * moveDistance;
        canvasGroup.alpha = 0f;

        // detruit a la fin
        Destroy(gameObject);
    }
}
