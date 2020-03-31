using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroOverviewPanel : MonoBehaviour
{
    private HeroController _hero;
    private Text _hpTb;
    private Text _attackTb;

    void Awake()
    {   
        _hpTb = GameObject.Find("HP").GetComponent<Text>();
        _attackTb = GameObject.Find("Attack").GetComponent<Text>();
    }

    public void SetHero(HeroController hero)
    {
        _hero = hero;
        _hpTb.text = $"HP: {_hero.Hp}";
        _attackTb.text = $"Attack: {_hero.Attack}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
