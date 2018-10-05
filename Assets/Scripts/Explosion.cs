using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public Sprite[] sprites;
    private SpriteRenderer spriteRenderer;
    private int spriteIndex = 0;
    public float delay = 0.2f;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[0];
        StartCoroutine(NextSprite());
    }


    private IEnumerator NextSprite()
    {
        bool running = true;
        while(running)
        {
            yield return new WaitForSeconds(delay);
            spriteIndex++;
            if (spriteIndex < sprites.Length) { spriteRenderer.sprite = sprites[spriteIndex]; }
            else { running = false; }
        }
        Destroy(gameObject);
    }

}
