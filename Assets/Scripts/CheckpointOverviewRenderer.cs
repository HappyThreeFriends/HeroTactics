using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointOverviewRenderer : MonoBehaviour
{
    GameController _gameController;

    public GameObject HeroOverviewPanelPrefab;

    private List<GameObject> _heroOverviewPanels = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        _gameController = FindObjectOfType<GameController>();
        _gameController.OnCheckpointSelected += GameController_OnCheckpointSelected;
        _gameController.OnCheckpointDeselected += GameController_OnCheckpointDeselected;
    }

    private void GameController_OnCheckpointDeselected()
    {
        foreach (var item in _heroOverviewPanels)
        {
            Destroy(item);
        }
        _heroOverviewPanels.Clear();
    }

    private void GameController_OnCheckpointSelected(CheckPointController checkpoint)
    {
        foreach (var item in _heroOverviewPanels)
        {
            Destroy(item);
        }
        foreach (var hero in checkpoint.Heroes)
        {
            var heroOverview = Instantiate(HeroOverviewPanelPrefab, this.transform);
            heroOverview.transform.localScale = new Vector2(1.0f / this.transform.localScale.x, 1.0f / this.transform.localScale.y);
            
            heroOverview.transform.localPosition = new Vector2(0, 0);
            heroOverview.GetComponent<HeroOverviewPanel>().SetHero(hero);
            _heroOverviewPanels.Add(heroOverview);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
