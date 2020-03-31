using System;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private HeroController[] _heroes;    
    private Map _map;

    public event Action<HeroController> OnHeroSelected;
    public event Action OnHeroDeselected;
   
    // Start is called before the first frame update
    void Start()
    {        
        _map = FindObjectOfType<Map>();
        _map.OnMapCreated += Map_OnMapCreated;        
    }

    private void Map_OnMapCreated()
    {
        _heroes = FindObjectsOfType<HeroController>();
        foreach (var hero in _heroes)
        {
            hero.MoveTo(_map.CheckPoints.First());
        }
        foreach (var checkpoint in _map.CheckPoints)
        {
            checkpoint.OnCheckpointSelected += Checkpoint_OnClick;
        }
    }

    private void Checkpoint_OnClick(CheckPointController checkpoint)
    {
        if (checkpoint.Hero != null)
        {
            OnHeroSelected?.Invoke(checkpoint.Hero);
        }
        else
        {
            OnHeroDeselected?.Invoke();
        }

        var previouslySelectedCheckpoint = _map.CheckPoints.SingleOrDefault(c => c.IsSelected);    
        if(previouslySelectedCheckpoint == null)
        {
            return;
        }

        if (previouslySelectedCheckpoint.Hero != null &&
            _map.HasDirectPath(previouslySelectedCheckpoint.Hero.CurrentCheckpoint, checkpoint))
        {
            previouslySelectedCheckpoint.Hero.MoveTo(checkpoint);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ClearSelectionIfClickedOutsideOfSelectableObjects();
    }

    void ClearSelectionIfClickedOutsideOfSelectableObjects()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var worldTouch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (!Physics2D.Raycast(worldTouch, Vector2.zero, Mathf.Infinity))
            {
                OnHeroDeselected?.Invoke();
                foreach (var hero in _heroes)
                {
                    hero.Deselect();
                }
            }
        }
    }
}
