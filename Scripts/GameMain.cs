using Godot;
using System;
using System.Collections.Generic;

public class GameMain : Node
{
    private int _turnNum = 0;
    private List<Fleet> Players;
    private int _side = 0;
    private bool _currentSideTurnOver = false;

    public static int FieldSize = 3;
    public static int MaxShipsNum = GetShipsNumByFieldSize();

    public static int GetShipsNumByFieldSize()
    {
        return FieldSize > 5 ? (int)Math.Pow(FieldSize, 3) / 25 : FieldSize;
    }

    //обработчик сигнала следующего хода
    public void OnFleetNextTurn()
    {
        _currentSideTurnOver = true;
    }

    private void ReturnToMenu()
    {
        GetTree().ChangeScene("res://Nodes/MainMenu.tscn");
    }

    private void NextSideTurn()
    {
        _currentSideTurnOver = false;
        _side++;
        if (_side == Players.Count)
        {   
            _side = 0;
            _turnNum++;
            GD.Print("Turn: " + _turnNum.ToString());
        }
        if (Players[_side].GetShipsRemain == 0)
        {
            ReturnToMenu();
        }
        Players[_side].TurnStart();
    }

    public override void _Ready()
    {
        _side = 0;
        Players = new List<Fleet>();
        foreach(var child in GetChildren())
        {
            if (child is Fleet fleet)
            {
                Players.Add(fleet);
            }
        }
        Players[_side].TurnStart();
    }

    public override void _Process(float delta)
    {
        if (_currentSideTurnOver)
        {
            NextSideTurn();
        }
        if (Input.IsActionJustPressed("ui_cancel"))
        {
            ReturnToMenu();
        }
    }
}
