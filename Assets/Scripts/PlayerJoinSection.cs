using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerJoinSection : MonoBehaviour
{
    public RectTransform colorSection; 
    public RectTransform ship;

    [HideInInspector]
    public bool playerActive = false;



    public void AddPlayer(Player p, float x, float width)
    {
        ship.GetComponent<ShipRotate>().Setup(p);

        gameObject.SetActive(true);

        playerActive = true;

        colorSection.sizeDelta = new Vector2(width, 1082f);
        colorSection.anchoredPosition = new Vector2(x, 1082f);
        ship.anchoredPosition = new Vector2(0, 1080f);

        colorSection.DOAnchorPosY(0f, 0.3f).OnComplete(() =>
        {
            ship.DOAnchorPosY(0f, 1f);
        });  
    }


    public void SetNewSize(float x, float width, float delay = 0)
    {
        colorSection.DOAnchorPosX(x, 0.3f).SetDelay(delay);
        colorSection.DOSizeDelta(new Vector2(width, 1082f), 0.3f).SetDelay(delay);
    }


    public void RemovePlayer()
    {
        ship.GetComponent<ShipRotate>().Reset();

        playerActive = false;
        ship.DOAnchorPosY(-1080f, 1f).OnComplete(() =>
        {
            colorSection.DOAnchorPosY(-1082f, 0.3f).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        });
    }


    public void Reset()
    {
        gameObject.SetActive(false);

        playerActive = false;

        colorSection.anchoredPosition = new Vector2(0, 1082f);
        ship.anchoredPosition = new Vector2(0, 1080f);
    }
}
