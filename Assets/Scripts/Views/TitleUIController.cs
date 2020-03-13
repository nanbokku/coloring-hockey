using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TitleUIController : MonoBehaviour
{
    [SerializeField]
    private Button startBtn = null;
    [SerializeField]
    private Button howtoBtn = null;
    [SerializeField]
    private Button startBtnInHowTo = null;
    [SerializeField]
    private GameObject howto = null;

    /// <summary>
    /// スタートボタンを押下したときのイベント
    /// </summary>
    /// <value></value>
    public UnityAction OnStartBtnClicked { get; set; } = null;

    void Start()
    {
        startBtn.onClick.AddListener(() =>
        {
            OnStartBtnClicked();
        });

        startBtnInHowTo.onClick.AddListener(() =>
        {
            OnStartBtnClicked();
        });

        howtoBtn.onClick.AddListener(() =>
        {
            ShowHowToView();
        });

        ShowTitleView();
    }

    void ShowTitleView()
    {
        howto.SetActive(false);
        startBtn.gameObject.SetActive(true);
        howtoBtn.gameObject.SetActive(true);
    }

    void ShowHowToView()
    {
        howto.SetActive(true);
        startBtn.gameObject.SetActive(false);
        howtoBtn.gameObject.SetActive(false);
    }
}
