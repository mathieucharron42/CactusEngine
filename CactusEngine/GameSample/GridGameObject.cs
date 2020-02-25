using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using GameEngine;

namespace TestForms
{
    class GridGameObject : GameObject
    {
        public delegate void CellMouseClick(Vector2 coordinates);
        public event CellMouseClick OnCellClick;

        public GridGameObject()
        {
            _gridSize = Vector2.One;
        }

        public Vector2 GridSize
        {
            get { return _gridSize; }
            set { _gridSize = value; }
        }

        public float LineWidth
        {
            get { return _lineWidth; }
            set { _lineWidth = value; }
        }

        public override void Initialize(Engine engine)
        {
            _gridCells = new List<GridCell>();
            for(int x = 0; x < _gridSize.X; ++x)
            {
                for (int y = 0; y < _gridSize.Y; ++y)
                {
                    GridCell cell = new GridCell();
                    cell.Coordinates = new Vector2(x, y);
                    _gridCells.Add(cell);
                }
            }
        }

        public override void Shutdown(Engine engine)
        {
            _gridCells = null;
        }

        public override void Update(Engine engine, TimeSpan timespan)
        {
            //Transform transform = LocalTransform;
            //transform.Angle = ((transform.Angle + (float)timespan.TotalMilliseconds * 0.001f) % (2 * (float)Math.PI));
            //transform.RotationPivot = LocalTransform.Position + LocalTransform.Size / 2;
            //LocalTransform = transform;
        }

        public override void Render(Engine engine, Renderer renderer)
        {
            renderer.RenderRectangle(WorldTransform, Color.Black);

            int i = 1;
            for (int x = 0; x < GridSize.X; ++x)
            {
                for (int y = 0; y < GridSize.Y; ++y)
                {
                    Transform cellTransform = GetCellTransform(WorldTransform, new Vector2(x, y));

                    //Vector2 from = GetCellRenderPosition(WorldTransform, new Vector2(x, y));
                    //Vector2 to = from + cellSize - Vector2.One;

                    Color color = Color.Blue;
                    renderer.RenderRectangle(cellTransform.Position, cellTransform.Position + cellTransform.Size, cellTransform.Angle, WorldTransform.RotationPivot, color);
                    //renderer.RenderTexture(engine.CreateTexture("Assets/test.png"), from, to);
                    //renderer.RenderLine(from2, to2, 1, Color.Green);
                    //renderer.RenderLine(from, to, 1, Color.Green);
                    renderer.RenderString(cellTransform.Position, cellTransform.Position+cellTransform.Size, cellTransform.Angle, WorldTransform.RotationPivot, i.ToString(), 20, Color.WhiteSmoke, Renderer.StringAlignment.Centered);

                    ++i;
                }
            }
        }

        public override void Click(Engine engine, Vector2 localPosition)
        {
            Vector2? coordinates = GetCoordinatesFor(LocalTransform, localPosition);
            if (coordinates.HasValue)
            {
                GridCell cell = GetCellAt(coordinates.Value);
                if (OnCellClick != null)
                {
                    OnCellClick.Invoke(cell.Coordinates);
                }
            }
        }

        public List<UnitGameObject> GetUnits()
        {
            List<UnitGameObject> units = new List<UnitGameObject>();
            foreach(GridCell cell in _gridCells)
            {
                if(cell.Unit != null)
                {
                    units.Add(cell.Unit);
                }
            }
            return units;
        }

        public UnitGameObject GetUnitAt(Vector2 coordinates)
        {
            return GetCellAt(coordinates).Unit;
        }

        public Vector2? GetUnitCoordinates(UnitGameObject unit)
        {
            GridCell foundCell = _gridCells.FirstOrDefault((GridCell cell) =>
            {
                return cell.Unit == unit;
            });

            if (foundCell != null)
            {
                return foundCell.Coordinates;
            }
            else
            {
                return null;
            }
        }

        public void AddUnit(UnitGameObject unit, Vector2 coordinates)
        {
            AddChild(unit);
            SetUnit(unit, coordinates);
        }

        public void MoveUnit(UnitGameObject unit, Vector2 coordinates)
        {
            Vector2? previous = GetUnitCoordinates(unit);
            if (previous.HasValue)
            {
                SetUnit(null, previous.Value);
            }
            SetUnit(unit, coordinates);
        }

        private class GridCell
        {
            public Vector2 Coordinates
            {
                get { return _coordinates; }
                set { _coordinates = value; }
            }
            public UnitGameObject Unit
            {
                get { return _unit; }
                set { _unit = value; }
            }
            private Vector2 _coordinates;
            private UnitGameObject _unit;
        }

        private GridCell GetCellAt(Vector2 coordinates)
        {
            int index = (int)(coordinates.X * _gridSize.X + coordinates.Y);
            return _gridCells[index];
        }

        private Transform GetCellTransform(Transform transform, Vector2 coordinate)
        {
            Vector2 position = GetCellRenderPosition(transform, coordinate);
            Vector2 cellSize = GetCellSize(WorldTransform);
            return new Transform(position, cellSize, transform.ZOrder, transform.Angle, transform.RotationPivot); ;
        }

        private Vector2 GetCellRenderPosition(Transform transform, Vector2 coordinate)
        {
            return transform.Position
                + coordinate * GetCellSize(transform)
                + coordinate * GetLineWidth(transform);  
        }

        public Vector2? GetCoordinatesFor(Transform transform, Vector2 localPosition)
        {
            Vector2 rawCoordinates = localPosition / (GetCellSize(transform) + GetLineWidth(transform));
            Vector2 coordinates = new Vector2((float)Math.Floor(rawCoordinates.X), (float)Math.Floor(rawCoordinates.Y));
            return coordinates;
        }

        private void SetUnit(UnitGameObject unit, Vector2 coordinates)
        {
            if (unit != null)
            {
                Transform transform = unit.LocalTransform;
                transform.Position = GetCellRenderPosition(Transform.Origin, coordinates);
                transform.Size = GetCellSize(Transform.Origin);
                unit.LocalTransform = transform;
            }
            GetCellAt(coordinates).Unit = unit;
        }

        private Vector2 GetCellSize(Transform transform)
        {
            // cellSize = (size - lineCount*lineWidth) / gridSize
            Vector2 lineCount = (GridSize - Vector2.One);
            Vector2 lineWidth = GetLineWidth(transform);
            return (transform.Size - lineCount * lineWidth) / GridSize;
        }

        public Vector2 GetLineWidth(Transform transform)
        {
            return _lineWidth * transform.Size;
        }

        private Vector2 _gridSize;
        private List<GridCell> _gridCells;

        private float _lineWidth;
    }
}
