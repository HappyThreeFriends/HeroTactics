using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointController : MonoBehaviour
{
    public Sprite Sprite;
    public Sprite SpriteOnSelected;

    public bool IsSelected { get; private set; }
    public List<HeroController> Heroes { get; private set; } = new List<HeroController>();
    public HeroController Hero { get; private set; }
    public event Action<CheckPointController> OnCheckpointSelected;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void PlaceHero(HeroController hero)
    {
        Hero = hero;        
    }

    public void RemoveHero(HeroController hero)
    {
        Hero = null;
        Deselect();
    }
    // Update is called once per frame
    void Update()
    {
        ClearSelectionIfClickedOutsideOfCurrentCheckpoit();
    }

    private void OnMouseUp()
    {   
        OnCheckpointSelected?.Invoke(this);
        Select();
    }

    void ClearSelectionIfClickedOutsideOfCurrentCheckpoit()
    {      
        if (Input.GetMouseButtonUp(0))
        {
            var worldTouch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var collider = this.GetComponent<Collider2D>();
            if (!collider.OverlapPoint(worldTouch))
            {
                Deselect();
            }
        }
    }

    void Select()
    {
        IsSelected = true;
        this.GetComponent<SpriteRenderer>().sprite = SpriteOnSelected;
    }

    void Deselect()
    {
        IsSelected = false;
        this.GetComponent<SpriteRenderer>().sprite = Sprite;
    }
}
