using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRandomSkin : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites;
    private SpriteRenderer currentSprite;

    private void Awake()
    {
        currentSprite = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        currentSprite.sprite = sprites[Random.Range(0, sprites.Count)];
    }
}
