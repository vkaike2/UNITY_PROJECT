using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SecondMapStepParent : MapStepParent
{
    [Header("CONFIGURATIONS")]
    [SerializeField]
    private Transform _tilesParent;
    [SerializeField]
    private Transform _platformsParent;
    [SerializeField]
    private Transform _trapsParent;
    [SerializeField]
    private Transform _objectsParent;
    [SerializeField]
    private Transform _triggersParent;


    //TRAPS
    private List<Stalactite> _stalactites;
    private List<Spike> _spikes;
    private List<StoneBallSpawner> _stoneBallsSpawners;


    private void Awake()
    {
        _stalactites = _trapsParent == null ? new List<Stalactite>() : _trapsParent.GetComponentsInChildren<Stalactite>().ToList();
        _spikes = _trapsParent == null ? new List<Spike>() : _trapsParent.GetComponentsInChildren<Spike>().ToList();
        _stoneBallsSpawners = _trapsParent == null ? new List<StoneBallSpawner>() : _trapsParent.GetComponentsInChildren<StoneBallSpawner>().ToList();
    }


    public override void Show()
    {
        ToggleShowAndHide(true);
        
        if (_spikes != null)
        {
            foreach (var spike in _spikes)
            {
                spike.gameObject.SetActive(true);
                spike.Activate(true);
            }
        }
    }


    public override void Hide()
    {
        ToggleShowAndHide(false);
        
        if (_spikes != null)
        {
            foreach (var spike in _spikes)
            {
                spike.Activate(false);
                spike.gameObject.SetActive(false);
            }
        }
    }

    private void ToggleShowAndHide(bool value)
    {
        if (_stalactites != null)
        {
            foreach (var stalactite in _stalactites)
            {
                stalactite.gameObject.SetActive(value);
            }
        }

        if (_stoneBallsSpawners != null)
        {
            foreach (var stoneBall in _stoneBallsSpawners)
            {
                if (!value)
                {
                    stoneBall.Disable();
                }
                stoneBall.gameObject.SetActive(value);
            }
        }

        if (_tilesParent != null) _tilesParent.gameObject.SetActive(value);
        if (_platformsParent != null) _platformsParent.gameObject.SetActive(value);
        if (_objectsParent != null) _objectsParent.gameObject.SetActive(value);
        if (_triggersParent != null) _triggersParent.gameObject.SetActive(value);
    }
}
