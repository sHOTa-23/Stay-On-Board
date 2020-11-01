﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Brain : MonoBehaviour
{
    public int DNALength = 2;
    public float timeAlive;
    public float timeWalking;
    public DNA dna;
    public GameObject eyes;
    bool alive = true;
    bool seeGround = true;
   

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("dead"))
        {
            alive = false;
            timeAlive = 0;
            timeWalking = 0;
        }
    }

    public void Init()
    {
        dna = new DNA(DNALength, 3);
        timeAlive = 0;
        alive = true;
    }

    public void Update()
    {
        if (!alive) return;
        RaycastHit hit;
        if(Physics.Raycast(eyes.transform.position,eyes.transform.forward*10,out hit))
        {
           seeGround = hit.collider.gameObject.CompareTag("platform");
        }
        timeAlive = PopulationManager.elapsed;
        float turn = 0;
        float move = 0;
        if(seeGround)
        {
            if (dna.GetGene(0) == 0) { 
                move = 1;
                timeWalking += Time.deltaTime;
            }
            else if (dna.GetGene(0) == 1) turn = -90;
            else if (dna.GetGene(0) == 2) turn = 90;
        }
        else
        {
            if (dna.GetGene(1) == 0)
            {
                move = 1;
                timeWalking += Time.deltaTime;
            }
            else if (dna.GetGene(1) == 1) turn = -90;
            else if (dna.GetGene(1) == 2) turn = 90;
        }
        transform.Translate(0, 0, move * 0.1f);
        transform.Rotate(0, turn, 0);
    }
}
