using Godot;

public class Ship : ObjectMouseHandler
{
    private Vector3 _direction = Vector3.Forward;
    private int _speedMain = 1;
    private int _speedManuver = 1;

    private int _beforeFire = 0;
    private int _recharge = 2;
    private Vector3 _moveTo;

    public bool Firing = false;
    public Vector3 MoveTo 
    {
        get => _moveTo; 
        set
        {
            _moveTo.x = Mathf.Clamp(value.x, 0, GameMain.FieldSize-1);
            _moveTo.y = Mathf.Clamp(value.y, 0, GameMain.FieldSize-1);
            _moveTo.z = Mathf.Clamp(value.z, 0, GameMain.FieldSize-1);
        }
    }
    public string ShipName {get; private set;}

    [Signal]
    public delegate void ShipFire(Vector3 shootFrom);
    [Signal]
    public delegate void ShipReadyToFire(Vector3 shootFrom);
    [Signal]
    public delegate void ShipFiring(Vector3 shootFrom);

    private Vector3 SpeedNormalization(Vector3 movement)
    {
        Vector3 positive = Vector3.One * _speedManuver;
        Vector3 negative = -Vector3.One * _speedManuver;
        if (_direction.Abs() == _direction)
        {
            positive += _direction * (_speedMain - _speedManuver);
        }
        else
        {
            negative += _direction * (_speedMain - _speedManuver);
        }

        if (movement.x > positive.x)
            movement.x = positive.x;
        if (movement.y > positive.y)
            movement.y = positive.y;
        if (movement.z > positive.z)
            movement.z = positive.z;
        if (movement.x < negative.x)
            movement.x = negative.x;
        if (movement.y < negative.y)
            movement.y = negative.y;
        if (movement.z < negative.z)
            movement.z = negative.z;

        return movement;
    }

    public void NewShip(Vector3 position, string shipName)
    {
        Translation = position;
        MoveTo = position;
        ShipName = shipName;
    }
    public void TurnStartUpdate()
    {
        if (_beforeFire > 0)
        {
            _beforeFire--;
        }
        if (Firing)
        {
            EmitSignal("ShipFiring", Translation);
        }
        if (_beforeFire == 0 && Firing)
        {
            EmitSignal("ShipReadyToFire", Translation);
        }

    }
    public void TurnEndUpdate()
    {
        if (_beforeFire == 0 && Firing)
        {
            EmitSignal("ShipFire", Translation);
            _beforeFire = _recharge;
        }
        if (Translation != MoveTo)
        {
            Translate(SpeedNormalization(MoveTo - Translation));
        }

    }
}
