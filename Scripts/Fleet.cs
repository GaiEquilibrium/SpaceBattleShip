using Godot;
using System.Collections.Generic;

public abstract class Fleet : Spatial
{
    private PackedScene ShipScene;

    protected List<Ship> _ships;
    protected Ship _chosenShip;
    protected Vector3 _hitPosition;
    protected bool _isActive;
    protected RandomNumberGenerator _rng;

    public int GetShipsRemain {get => _ships.Count;}

    [Signal] //сигнал наружу, что готовы к следующему ходу
    public delegate void NextTurn();
    [Signal] //выстрел от флота (как раз то, что стреляет корабль)
    public delegate void FleetAtack(Vector2 position);

    private Ship GetShip(string shipName)
    {
        for (int i = 0; i < _ships.Count; i++)
        {
            if (_ships[i].ShipName == shipName)
            {
                return _ships[i];
            }
        }
        return null;
    }
    private Ship GetShip(Vector3 position)
    {
        for (int i = 0; i < _ships.Count; i++)
        {
            if (_ships[i].Translation == position)
            {
                return _ships[i];
            }
        }
        return null;
    }
    private void DestroyShip (Ship ship)
    {
        _ships.Remove(ship);
        ship.IsClickble = false;
        ship.QueueFree();
    }
    private void DestroyCollidedShips ()
    {
        var collidedShips = new List<Ship>();
        foreach(Ship checkedShip in _ships)
        {
            foreach (Ship ship in _ships)
            {
                if (checkedShip.ShipName != ship.ShipName && checkedShip.Translation == ship.Translation)
                {
                    collidedShips.Add(checkedShip);
                }
            }
        }
        foreach (Ship ship in collidedShips)
        {
            DestroyShip(ship);
        }
        collidedShips.Clear();
    }

    protected virtual void AddShip(Vector3 position, string name)
    {
        _chosenShip = (Ship)ShipScene.Instance();
        if (GetShip(name) != null || GetShip(position) != null)
        {
            //GD.Print ("Cannot add ship \"" + name + "\" at point " + position.ToString());
            return;
        }
        _chosenShip.NewShip(position, name);
        _ships.Add(_chosenShip);
        _chosenShip.Connect("ShipFire", this, "OnShipFire");
        AddChild(_chosenShip);
    }
    protected virtual void TurnEnd()
    {
        _isActive = false;
        foreach (Ship ship in _ships)
        {
            ship.TurnEndUpdate();
        }
        DestroyCollidedShips();
        EmitSignal("NextTurn");
    }
    public virtual void TurnStart()
    {
        foreach (Ship ship in _ships)
        {
            ship.TurnStartUpdate();
        }
        _isActive = true;
    }
    public virtual void OnFleetHit(Vector2 position)
    {
        _hitPosition = new Vector3(position.x,position.y,0);
        for (; _hitPosition.z < GameMain.FieldSize; _hitPosition.z++)
        {
            if (GetShip(_hitPosition) != null)
            {
                DestroyShip(GetShip(_hitPosition));
                break;
            }
        }
    }
    public void OnShipFire(Vector3 shootFrom)
    {
        EmitSignal("FleetAtack", new Vector2(GameMain.FieldSize - (shootFrom.x + 1), shootFrom.y));
    }

    public override void _Ready()
    {
        _rng = new RandomNumberGenerator();
        _rng.Randomize();
        ShipScene = ResourceLoader.Load<PackedScene>("res://Nodes/Ship.tscn");
        _ships = new List<Ship>();
        int shipNum = 0;
        while (_ships.Count < GameMain.MaxShipsNum)
        {
            AddShip(new Vector3(_rng.Randi()%GameMain.FieldSize, _rng.Randi()%GameMain.FieldSize, _rng.Randi()%GameMain.FieldSize), "ship-"+shipNum.ToString());
            shipNum++;
        }
        _chosenShip = null;
        _isActive = false;
    }
}
