using Godot;
using System;

public class AIControlledFleet : Fleet
{
    public static bool HideShips = true;
    public static bool ShowHits = true;
    private float _processDelay = 1.0f;
    private float _processFromStart = 0.0f;
    private ShootDrawer _shootDrawer;
    private Label _enemyShipsRemainLabel;

    protected override void TurnEnd()
    {
        _processFromStart = 0;
        //вероятно переписать через шарповый рандом
        var order = _rng.Randi()%3;
        _chosenShip = _ships[Convert.ToInt32(_rng.Randi() % _ships.Count)];
        if(order == 0)
        {
            _chosenShip.Firing = !_chosenShip.Firing;
        }
        else if(order == 1)
        {
            _chosenShip.MoveTo =  new Vector3(_rng.Randi()%GameMain.FieldSize, _rng.Randi()%GameMain.FieldSize, _rng.Randi()%GameMain.FieldSize);
        }
        _chosenShip = null;
        _shootDrawer.Clear();
        base.TurnEnd();
    }

    public override void OnFleetHit (Vector2 position)
    {
        base.OnFleetHit(position);
        if (ShowHits)
        {
            _shootDrawer.FleetHit(_hitPosition);
        }
    } 

    public override void _Ready()
    {
        base._Ready();
        _shootDrawer = GetNode<ShootDrawer>("ShootDrawer");
        Translate(new Vector3(GameMain.FieldSize-1, 0, -1));
        RotationDegrees = new Vector3(0, -180, 0);
        if (HideShips)
        {
            foreach (var ship in _ships)
            {
                ship.Hide();
            }
        }
        _enemyShipsRemainLabel = GetNode<Label>("EnemyShipsRemainLabel");
    }

    public override void _Process(float delta)
    {
        if (_processFromStart >= _processDelay && _isActive)
        {
            TurnEnd();
        }
        else if (_isActive)
        {
            _processFromStart += delta;
        }
        _enemyShipsRemainLabel.Text = "Enemy ships remain: " +  GetShipsRemain.ToString();
    }
}
