using System;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private HeroController[] _heroes;    
    private Map _map;

    public event Action<CheckPointController> OnCheckpointSelected;
    public event Action OnCheckpointDeselected;
   
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
            checkpoint.OnCheckpointSelected += Checkpoint_OnSelected;
        }
    }

    private void Checkpoint_OnSelected(CheckPointController checkpoint)
    {     
        OnCheckpointSelected?.Invoke(checkpoint);

        var previouslySelectedCheckpoint = _map.CheckPoints.SingleOrDefault(c => c.IsSelected);    
        if(previouslySelectedCheckpoint == null)
        {
            return;
        }

        if(_map.HasDirectPath(previouslySelectedCheckpoint, checkpoint))
        {
            previouslySelectedCheckpoint.MoveHeroesTo(checkpoint);
        }
        OnCheckpointSelected?.Invoke(checkpoint);
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
                OnCheckpointDeselected?.Invoke();               
            }
        }
    }
}
