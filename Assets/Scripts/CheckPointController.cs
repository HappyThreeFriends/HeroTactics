using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckPointController : MonoBehaviour
{
    public Sprite Sprite;
    public Sprite SpriteOnSelected;

    public bool IsSelected { get; private set; }
    public List<HeroController> Heroes { get; private set; } = new List<HeroController>();
    private List<HeroController> _selectedHeroes = new List<HeroController>();
    public event Action<CheckPointController> OnCheckpointSelected;

    // Start is called before the first frame update
    void Start()
    {   
    }

    public void PlaceHero(HeroController hero)
    {
        Heroes.Add(hero);
    }

    public void RemoveHero(HeroController hero)
    {
        Heroes.Remove(hero);
        Deselect();
    }

    public void MoveHeroesTo(CheckPointController other)
    {
        foreach(var selectedHero in _selectedHeroes)
        {
            selectedHero.MoveTo(other);
        }

        Heroes = Heroes.Except(_selectedHeroes).ToList();
        _selectedHeroes.Clear();
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
        _selectedHeroes = Heroes.ToList();
        this.GetComponent<SpriteRenderer>().sprite = SpriteOnSelected;
    }

    void Deselect()
    {
        IsSelected = false;
        this.GetComponent<SpriteRenderer>().sprite = Sprite;
    }
}
