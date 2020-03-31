using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroOverviewRenderer : MonoBehaviour
{
    GameController _gameController;
   
    public GameObject HeroOverviewPanelPrefab;

    private GameObject _heroOverviewPanel;
    // Start is called before the first frame update
    void Start()
    {   
        _gameController = FindObjectOfType<GameController>();
        _gameController.OnHeroSelected += GameController_OnHeroSelected;
        _gameController.OnHeroDeselected += GameController_OnHeroDeselected;
    }

    private void GameController_OnHeroDeselected()
    {
        _heroOverviewPanel.SetActive(false);
    }

    private void GameController_OnHeroSelected(HeroController hero)
    {
        if (_heroOverviewPanel == null)
        {
            _heroOverviewPanel = Instantiate(HeroOverviewPanelPrefab, this.transform);
        }
        _heroOverviewPanel.SetActive(true);
        _heroOverviewPanel.GetComponent<HeroOverviewPanel>().SetHero(hero);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
