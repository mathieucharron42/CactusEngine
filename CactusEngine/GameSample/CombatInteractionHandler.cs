using GameEngine;
using System.Linq;
using System.Numerics;

namespace TestForms
{
    class CombatInteractionHandler : SubSystem
    {
        GridGameObject _grid;
        CombatManager _combatManager;

        public CombatInteractionHandler()
        {
            
        }

        public GridGameObject Grid
        {
            get { return _grid; }
            set { _grid = value; }
        }

        public override void Initialize(Engine engine)
        {
            _grid.OnCellClick += OnCellClick;
        }

        public override void Shutdown(Engine engine)
        {
            _grid.OnCellClick -= OnCellClick;
        }

        private void OnCellClick(Vector2 coordinates)
        {
            UnitGameObject unitAtClick = _grid.GetUnitAt(coordinates);
            UnitGameObject selected = GetSelectedUnit();
            if(unitAtClick != null)
            {
                if (selected == null)
                {
                    // Select
                    unitAtClick.Selected = true;
                }
                else if(selected == unitAtClick)
                {
                    selected.Selected = false;
                }
            }
            else
            {
                // Move
                _grid.MoveUnit(selected, coordinates);
            }
        }

        private UnitGameObject GetSelectedUnit()
        {
            return _grid.GetUnits().FirstOrDefault((UnitGameObject unit) =>
            {
                return unit.Selected;
            });
        }
    }
}
