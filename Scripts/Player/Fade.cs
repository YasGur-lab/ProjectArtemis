using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*************************************
Class: Fade
Author: Charlie Weir
Created: 01/09/22
Last Modified: 17/01/23
Description: This class is in charge of the fader.

Change Log
**********
Date: 17/01/23
Author: Antoine Plouffe
Verified By: Alexander Achorn
Changes: Updating the script to remove garbage collection.

Date: 
Author: 
Verified By: 
Changes: 
*************************************/

enum State
{
    NotFaded,
    Faded,
    FadingIn,
    FadingOut,
    FadingInOut
}
public class Fade : MonoBehaviour
{

    public bool FadeOnStart = true;
    public bool FadeOnStop = false;
    [SerializeField] Color FadeColor;
    [SerializeField] GameObject Fader;
    float AlphaMin = 0.0f;
    float AlphaMax = 1.0f;
     float FadeDuration = 1.00f;
     Renderer rend;
    float timer = 0;
    State state;
    bool FadingIn = true;

    private Color newColor;

    // Start is called before the first frame update
    void Start()
    {
        if(Fader ==null)
        {
            Fader = GameObject.FindGameObjectWithTag("Fader");
        }
        rend = Fader.GetComponent<Renderer>();
        FadeColor = rend.materials[0].color;
        FadeDuration = 2.00f;

        //newColor = FadeColor;
    }

    void Update()
    {
        if(FadeOnStart)
        {
            FadeOut();
            FadeOnStart = false;
        }
        
        switch (state)
        {
            case State.NotFaded:
                FadeColor.a = AlphaMin;
                break;
            case State.Faded:
                FadeColor.a = AlphaMax;
                break;
            case State.FadingIn:
                FadeColor.a = Mathf.Lerp(AlphaMin, AlphaMax, timer / FadeDuration);

                if(FadeColor.a >= AlphaMax)
                {
                    state = State.Faded;
                    timer = 0.0f;
                }
                break;
            case State.FadingOut:
                FadeColor.a = Mathf.Lerp(AlphaMax, AlphaMin, timer / FadeDuration);
                if (FadeColor.a <= AlphaMin)
                {
                    state = State.NotFaded;
                    timer = 0.0f;
                }
                break;
            case State.FadingInOut:
                
                if(FadingIn)
                {
                    FadeColor.a = Mathf.Lerp(AlphaMin, AlphaMax, timer / FadeDuration);

                    if (FadeColor.a >= AlphaMax)
                    {
                      
                          FadingIn = false;
                      timer = 0.0f;
                    }
                }
                else
                {
                    FadeColor.a = Mathf.Lerp(AlphaMax, AlphaMin, timer / FadeDuration);
                    if (FadeColor.a <= AlphaMin)
                    {
                        state = State.NotFaded;
                        timer = 0.0f;
                        FadingIn = true;
                    }
                }
                break;
            default:
                break;
        }

        if(state == State.FadingIn || state == State.FadingOut || state == State.FadingInOut)
        {

            timer += Time.deltaTime;
            if(rend) rend.material.color = FadeColor;
        }
    }

    public void FadeIn()
    {
        state = State.FadingIn;
    }
    public void FadeOut()
    {
        state = State.FadingOut;

    }
    
    public float GetFadeDuration()
    {
        return FadeDuration;
    }
}
