using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuitPiece : MonoBehaviour
{
    public SuitUp suitUp;
    [SerializeField] GameObject ObjectToReap;

    private void Start()
    {
        if(!ObjectToReap)
        {
            ObjectToReap = this.gameObject;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(this.tag == other.tag)
        {
            if(other.tag == "Helmet")
            {
                suitUp.PutOnHelmet();
                ObjectToReap.SetActive(false);
            }
            if (other.tag == "Suit")
            {
                suitUp.PutOnSuit();
                ObjectToReap.SetActive(false);
                this.gameObject.SetActive(false);
            }
            if (other.tag == "LeftHand")
            {
                suitUp.PutOnLeftGlove();
                ObjectToReap.SetActive(false);
            }
            if (other.tag == "RightHand")
            {
                suitUp.PutOnRightGlove();
                ObjectToReap.SetActive(false);
            }
        }
    }
}
