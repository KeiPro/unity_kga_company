using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTime : MonoBehaviour
{
    public Text gameTime;

    private DateTime currentTime;

    //public Image panelImage;

    void Start()
    {
        currentTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);
        StartCoroutine(UpdateTime());

    }

    private IEnumerator UpdateTime()
    {
        while (true)
        {
            // 현재 시간에 4초 추가
            currentTime = currentTime.AddSeconds(240);
            //Debug.Log(currentTime.ToString("HH:mm"));

            gameTime.text = currentTime.ToString("tt h:mm");

            //Color color = panelImage.color;

            //if(color.a <= 0.7f)
            //{
            //    color.a += 0.1f;

            //    panelImage.color = color;
            //}

            // 실제 시간에서는 1초마다 반복
            yield return new WaitForSeconds(1);
        }
    }
}
