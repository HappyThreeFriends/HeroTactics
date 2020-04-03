using System;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private HeroController[] _heroes;    
    private Map _map;

    public event Action<CheckPointController> OnCheckpointSelected;
    public event Action OnCheckpointDeselected;

    private Player CurrentPlayer = new Player { Id = 1, Color = "Blue" };

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
            hero.SetPlayer(CurrentPlayer);

            hero.MoveTo(_map.CheckPoints.First());
        }
        foreach (var checkpoint in _map.CheckPoints)
        {
            checkpoint.OnCheckpointSelected += Checkpoint_OnSelected;            
        }
    }

    private void Checkpoint_OnSelected(CheckPointController checkpoint)
    {   
        var previouslySelectedCheckpoint = _map.CheckPoints.SingleOrDefault(c => c.IsSelected);

        if(previouslySelectedCheckpoint != null && _map.HasDirectPath(previouslySelectedCheckpoint, checkpoint))
        {
            previouslySelectedCheckpoint.MoveHeroesTo(checkpoint);
        }
        OnCheckpointSelected?.Invoke(checkpoint);
    }    
}
