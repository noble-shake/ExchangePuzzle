using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class BackgroundMoveEffect : MonoBehaviour
{
    [SerializeField] float sliderSpeed = 10f;
    float totalSpeed;
    [SerializeField] Transform FrontPoint;
    [SerializeField] Transform BackPoint;
    [SerializeField] bool SliderCheck;
    [SerializeField] bool FrontFinish;
    [SerializeField] bool BackFinish;

    private void Update()
    {
        FrontSlide();
        BackSlide();
    }


    public void FrontSlide()
    {
        if (SliderCheck && !FrontFinish)
        {
            if (Vector3.Distance(transform.position, FrontPoint.position) < 0.001f)
            {
                FrontFinish = true;
                return;
            }

            totalSpeed += sliderSpeed * Time.timeScale;
            transform.position = Vector3.MoveTowards(transform.position, FrontPoint.position, totalSpeed);
        }
    }

    public void BackSlide()
    {
        if (!SliderCheck && !BackFinish)
        {
            if (Vector3.Distance(transform.position, BackPoint.position) < 0.001f)
            {
                BackFinish = true;
                return;
            }

            totalSpeed += sliderSpeed * Time.timeScale;
            transform.position = Vector3.MoveTowards(transform.position, BackPoint.position, totalSpeed);
        }
    }

    public void SlideChange()
    {
        FrontFinish = false;
        BackFinish = false;
        totalSpeed = 0f;
        SliderCheck = !SliderCheck;
    }

    public void SlideReset()
    {
        FrontFinish = false;
        BackFinish = false;
        totalSpeed = 0f;
        SliderCheck = true;
    }
}
