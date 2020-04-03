using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckPointController : MonoBehaviour
{
    [Serializable]
    public struct CapturedCheckpointSprite
    {
        public string Color;
        public Sprite Sprite;
    }

    public Sprite Sprite;
    public Sprite SpriteOnSelected;

    public bool IsSelected { get; private set; }
    public List<HeroController> Heroes { get; private set; } = new List<HeroController>();
    private List<HeroController> _selectedHeroes = new List<HeroController>();
    public event Action<CheckPointController> OnCheckpointSelected;
    public CapturedCheckpointSprite[] CapturedCheckpointSprites;
    private Dictionary<string, Sprite> _capturedCheckpointSprites => CapturedCheckpointSprites.ToDictionary(c => c.Color, c => c.Sprite);

    public Player CapturedBy { get; private set; }

    // Start is called before the first frame update
    void Start()
    {   
    }

    public void PlaceHero(HeroController hero)
    {
        CapturedBy = hero.Player;
        this.GetComponent<SpriteRenderer>().sprite = _capturedCheckpointSprites[CapturedBy.Color];
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
            if (!collider.OverlapPoint(worldTouch) && IsSelected)
            {
                Deselect();
            }
        }
    }

    private GameObject _checkpointSelection;
    void Select()
    {
        IsSelected = true;
        _selectedHeroes = Heroes.ToList();
        _checkpointSelection = new GameObject("CheckpointSelection");
        _checkpointSelection.transform.position = this.transform.position;
        _checkpointSelection.transform.SetParent(this.transform);       
        var spriteRenderer = _checkpointSelection.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = SpriteOnSelected;
    }

    void Deselect()
    {
        IsSelected = false;
        if(_checkpointSelection != null)
        {
            Destroy(_checkpointSelection);
        }
    }
}
