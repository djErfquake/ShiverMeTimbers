using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class ScoreBanner : MonoBehaviour
{

    #region singleton
    // singleton
    public static ScoreBanner instance;
    private void Awake()
    {
        if (instance && instance != this) { Destroy(gameObject); return; }
        instance = this;
    }
    #endregion

    public Color flashColor = Color.red;

    private RectTransform scoreHUD;
    public RectTransform[] banners;
    private TextMeshProUGUI[] bannerText;

    private float upPosition, downPosition = 30f;


    private void Start()
    {
        scoreHUD = transform as RectTransform;

        upPosition = banners[0].sizeDelta.y;

        bannerText = new TextMeshProUGUI[banners.Length];
        for (int i = 0; i < banners.Length; i++)
        {
            bannerText[i] = banners[i].GetComponentInChildren<TextMeshProUGUI>();
            bannerText[i].text = "";
        }

        Reset();
    }



    public void Reset()
    {
        scoreHUD.anchoredPosition = new Vector2(scoreHUD.anchoredPosition.x, upPosition);
        for (int i = 0; i < banners.Length; i++)
        {
            banners[i].gameObject.SetActive(false);
        }
    }

    public void Show()
    {
        scoreHUD.DOAnchorPosY(downPosition, 1f).SetEase(Ease.OutCubic);
    }

    public void Hide()
    {
        scoreHUD.DOAnchorPosY(upPosition, 1f).SetEase(Ease.InCubic);
    }


    public void SetScore(int playerIndex, string score)
    {
        bannerText[playerIndex].color = flashColor;
        bannerText[playerIndex].text = score;
        bannerText[playerIndex].transform.DOPunchScale(Vector2.one * 0.6f, 1f).OnComplete(() =>
        {
            bannerText[playerIndex].color = Color.white;
        });
    }

    public void SetPlayerActive(int playerIndex, bool active)
    {
        banners[playerIndex].gameObject.SetActive(active);
    }
}
