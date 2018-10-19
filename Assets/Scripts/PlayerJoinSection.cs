﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerJoinSection : MonoBehaviour
{
    public RectTransform colorSection; 
    public RectTransform ship;

    [HideInInspector]
    public bool playerActive = false;



    public void AddPlayer(float x, float width)
    {
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


    public void SetNewSize(float x, float width)
    {
        colorSection.DOAnchorPosX(x, 0.3f);
        colorSection.DOSizeDelta(new Vector2(width, 1082f), 0.3f);
    }


    public void Reset()
    {
        playerActive = false;
        ship.DOAnchorPosY(-1080f, 1f).OnComplete(() =>
        {
            colorSection.DOAnchorPosY(-1082f, 0.3f).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        });
    }
}