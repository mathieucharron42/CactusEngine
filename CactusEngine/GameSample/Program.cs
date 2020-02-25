using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestForms
{
	static class Program
	{
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			GameEngine.Engine engine = new GameEngine.Engine();
			engine.Initialize();

			GridGameObject grid = engine.CreateGameObject<GridGameObject>(newGrid =>
			{
				newGrid.GridSize = new Vector2(3, 3);

				Transform localTransform = Transform.Origin;
				Vector2 offset = new Vector2(50, 50);
				localTransform.Position = Vector2.Zero + offset;
				localTransform.Size = engine.GetViewport().Size - offset * 2;
				newGrid.LocalTransform = localTransform;

				newGrid.LineWidth = 0.005f;
			});
			engine.Root.AddChild(grid);

			List<Vector2> positions = new List<Vector2>()
			{
				new Vector2(0,0),
				new Vector2(1,1),
				new Vector2(2,2),
			};
			for(int i = 0; i < positions.Count; ++i)
			{
				UnitGameObject unit = engine.CreateGameObject<UnitGameObject>(newUnit =>
				{
					if (i % 2 == 0)
					{
						newUnit.SpritePath = "Assets/character.png";
					}
					else
					{
						newUnit.SpritePath = "Assets/topdown.png";
						newUnit.AnimatedSpritePattern = "Assets/Units/Knife/unit_knife_{0}.png";
						newUnit.AnimatedSpriteCount = 20;
						newUnit.AnimatedSpriteTotalTime = TimeSpan.FromSeconds(0.5);
					}
				});
				grid.AddUnit(unit, positions[i]);
			}

			engine.StartSubSystem<CombatInteractionHandler>(newCombatInteractionHandler =>
			{
				newCombatInteractionHandler.Grid = grid;
			});
			
			engine.Run();
		}
	}
}
