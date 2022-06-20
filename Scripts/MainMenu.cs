using Godot;
using System;

public class MainMenu : Control
{
    private Slider _fieldSize;
    private Label _fieldSizeLabel;
    private Slider _shipNum;
    private Label _shipNumLabel;
    private CheckBox _hideEnemies;
    private int _maxShips = 100;

    public void OnFieldSizeValueChanged(float value)
    {
        _fieldSize.Value = (int)value;
        GameMain.FieldSize = (int)value;
        _shipNum.MaxValue = GameMain.FieldSize < 5 ? (int)Math.Pow(GameMain.FieldSize, 3) : _maxShips;
        OnShipNumValueChanged (GameMain.GetShipsNumByFieldSize());
        _fieldSizeLabel.Text = "Field size: " + GameMain.FieldSize.ToString();
    }
    public void OnShipNumValueChanged(float value)
    {
        _shipNum.Value = Mathf.Clamp(value,(int)_shipNum.MinValue,(int)_shipNum.MaxValue);
        GameMain.MaxShipsNum = (int)_shipNum.Value;
        _shipNumLabel.Text = "Ships amount: " + GameMain.MaxShipsNum.ToString();
    }

    public void OnStartButtonUp()
    {
        GetTree().ChangeScene("res://Nodes/GameMain.tscn");
    }
    public void OnExitButtonUp ()
    {
        GetTree().Quit();
    }

    public override void _Ready()
    {
        _fieldSize = GetNode<Slider>("FieldSize");
        _shipNum = GetNode<Slider>("ShipNum");
        _fieldSizeLabel = GetNode<Label>("FieldSize/Label");
        _shipNumLabel = GetNode<Label>("ShipNum/Label");
        _hideEnemies = GetNode<CheckBox>("HideEnemies");
        
        OnShipNumValueChanged(GameMain.MaxShipsNum);
        OnFieldSizeValueChanged(GameMain.FieldSize);
        _hideEnemies.SetPressedNoSignal(true);
        _hideEnemies.Pressed = AIControlledFleet.HideShips;
    }

    public override void _Process(float delta)
    {
        AIControlledFleet.HideShips = _hideEnemies.Pressed;
    }
}
