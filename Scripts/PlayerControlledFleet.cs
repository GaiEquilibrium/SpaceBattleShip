using Godot;

public class PlayerControlledFleet : Fleet
{
    private Camera _mainCamera;
    private ObjectMouseHandler _horizontalGridNodeMain;
    private ObjectMouseHandler _horizontalGridNodeAdditional;
    private ObjectMouseHandler _newTarget;
    private ObjectMouseHandler _currentTarget;
    private ObjectMouseHandler _localGridHorizontal;
    private ObjectMouseHandler _verticalCoordinatePlane;
    private LineDrawer _moveLineDrawer;
    private ShootDrawer _shootDrawer;
    private Vector3 _resultMoveCoords;
    private Vector3 _shipPos;


#region internal methods
    private void NodesPosUpdate ()
    {
        Vector3 currentPos = _localGridHorizontal.Translation;
        currentPos.y = _shipPos.y;
        _localGridHorizontal.Translation = currentPos;

        _verticalCoordinatePlane.RotationDegrees = new Vector3(0, _mainCamera.RotationDegrees.y, 0);
        _verticalCoordinatePlane.Translation = new Vector3(_resultMoveCoords.x, 0, _resultMoveCoords.z);

        _currentTarget.Translation = new Vector3(_shipPos.x, _shipPos.y, GridDrawer.GridOffset);
        _newTarget.Translation = new Vector3(_resultMoveCoords.x, _resultMoveCoords.y, GridDrawer.GridOffset);
        _horizontalGridNodeMain.Translation = new Vector3(_resultMoveCoords.x, _shipPos.y, _resultMoveCoords.z);
        _horizontalGridNodeAdditional.Translation = _resultMoveCoords;

        _moveLineDrawer.Verticies[0] = _resultMoveCoords;
        _moveLineDrawer.Verticies[1] = _shipPos;
        _moveLineDrawer.Verticies[2] = _currentTarget.Translation;
        _moveLineDrawer.Verticies[3] = _shipPos;
        _moveLineDrawer.Verticies[4] = _horizontalGridNodeMain.Translation;
        _moveLineDrawer.Verticies[5] = _resultMoveCoords;
        _moveLineDrawer.Verticies[6] = _newTarget.Translation;
    }
    private void ShowLocalGrid()
    {
        _localGridHorizontal.IsClickble = true;
        _currentTarget.IsClickble = true;

        _localGridHorizontal.Show();
        _horizontalGridNodeMain.Show();
        _horizontalGridNodeAdditional.Show();
        _currentTarget.Show();
        _newTarget.Show();
        _moveLineDrawer.Show();

        //NodesPosUpdate();
    }
    private void HideLocalGrid()
    {
        _localGridHorizontal.IsClickble = false;
        _verticalCoordinatePlane.IsClickble = false;

        _localGridHorizontal.Hide();
        _horizontalGridNodeMain.Hide();
        _horizontalGridNodeAdditional.Hide();
        _currentTarget.Hide();
        _newTarget.Hide();
        _moveLineDrawer.Hide();
    }
    private void DisconectCurrentShip()
    {
        if (_chosenShip == null)
        {
            return;
        }
        _chosenShip.IsClickble = true;
        _chosenShip = null;
        _shipPos = Vector3.Zero;
    }
#endregion

#region inherited methods and handlers
    protected override void AddShip(Vector3 position, string name)
    {
        base.AddShip(position, name);
        _chosenShip.Connect("Clicked", this, "OnShipClicked");
        _chosenShip.Connect("ShipFiring", this, "OnShipFiring");
        _chosenShip.Connect("ShipReadyToFire", this, "OnShipReadyToFire");
    }
    protected override void TurnEnd()
    {
        HideLocalGrid();
        DisconectCurrentShip();
        _shootDrawer.NextTurnClear();
        foreach (var ship in _ships)
        {
            ship.IsClickble = false;
        }
        base.TurnEnd();
    }
    public override void TurnStart()
    {
        base.TurnStart();
        foreach (var ship in _ships)
        {
            ship.IsClickble = true;
        }
    }
    public override void OnFleetHit (Vector2 position)
    {
        base.OnFleetHit(position);
        _shootDrawer.FleetHit(_hitPosition);
    } 
#endregion

#region internal handlers
    public void OnShipFiring (Vector3 shipPos)
    {
        _shootDrawer.ShipShooting(shipPos);
    }
    public void OnShipReadyToFire (Vector3 shipPos)
    {
        _shootDrawer.ShipReadyToFire(shipPos);
    }
    public void OnShipClicked(Ship ship)
    {
        if (_chosenShip != null)
        {
            DisconectCurrentShip();
        }
        _chosenShip = ship;
        _shipPos = _chosenShip.Translation;
        _resultMoveCoords = _shipPos;
        NodesPosUpdate();
        ShowLocalGrid();
    }

    public void OnCurrentTargetClicked(ObjectMouseHandler localHorizontalGrid)
    {
        //EmitSignal("ClickOnCurrentTarget");
        _chosenShip.Firing = !_chosenShip.Firing;
        TurnEnd();
    }
    public void OnLocalGridHorizontalClicked(ObjectMouseHandler localHorizontalGrid)
    {
        _verticalCoordinatePlane.IsClickble = true;
        _currentTarget.IsClickble = false;
    }
    public void OnVerticalCoordinatePlaneClicked(ObjectMouseHandler verticalCoordinatePlane)
    {
        _chosenShip.MoveTo = _resultMoveCoords;
        TurnEnd();
    }
#endregion

    public override void _Ready()
    {
        base._Ready();
        _mainCamera = GetNode<Camera>("Camera");
        _localGridHorizontal = GetNode<ObjectMouseHandler>("LocalGridHorizontal");
        _verticalCoordinatePlane = GetNode<ObjectMouseHandler>("VerticalCoordinatePlane");

        _horizontalGridNodeMain = GetNode<ObjectMouseHandler>("HorizontalGridNodeMain");
        _horizontalGridNodeAdditional = GetNode<ObjectMouseHandler>("HorizontalGridNodeAdditional");
        _currentTarget = GetNode<ObjectMouseHandler>("CurrentTarget");
        _newTarget = GetNode<ObjectMouseHandler>("NewTarget");
        _horizontalGridNodeMain.IsClickble = false;
        _horizontalGridNodeAdditional.IsClickble = false;
        _newTarget.IsClickble = false;

        _moveLineDrawer = GetNode<LineDrawer>("LineDrawer");
        _shootDrawer = GetNode<ShootDrawer>("ShootDrawer");

        HideLocalGrid();
    }
    public override void _Process(float delta)
    {
        if (_localGridHorizontal.IsClickble && _localGridHorizontal.IsMouseInside)
        {
            _resultMoveCoords.x = Mathf.Clamp(_localGridHorizontal.MousePos.x, 0, GameMain.FieldSize-1);
            _resultMoveCoords.z = Mathf.Clamp(_localGridHorizontal.MousePos.z, 0, GameMain.FieldSize - 1);
        }
        if (_verticalCoordinatePlane.IsClickble && _verticalCoordinatePlane.IsMouseInside)
        {
            _resultMoveCoords.y = Mathf.Clamp(_verticalCoordinatePlane.MousePos.y, 0, GameMain.FieldSize - 1);
        }
        NodesPosUpdate();

        if (Input.IsActionJustPressed("SkipTurn") && _isActive)
        {
            TurnEnd();
        }
    }
}
