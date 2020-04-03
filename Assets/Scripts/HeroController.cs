using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    public bool IsSelected { get; private set; }
    public CheckPointController CurrentCheckpoint { get; private set; }
    public Player Player { get; private set; }

    public int Hp;
    public int Attack;


    void OnMouseDown()
    {
        IsSelected = true;
    }

    public void SetPlayer(Player player)
    {
        Player = player;
    }

    public void MoveTo(CheckPointController checkpoint)
    {
        this.transform.position = checkpoint.transform.position;        
        CurrentCheckpoint = checkpoint;
        CurrentCheckpoint.PlaceHero(this);        
    }

    public void Deselect()
    {
        IsSelected = false;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
