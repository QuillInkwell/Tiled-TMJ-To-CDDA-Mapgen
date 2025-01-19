using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMJ_To_Mapgen
{
	public class TerrainFurnitureCombo
	{
		public int terrainID = 0;
		public int furnitureID= 0;
		
		public string CDDATerrainID = "";
		public string CDDAFurnitureID = "";

		public char mapSymbol = 'a';

		public TerrainFurnitureCombo(int terrainID, int furnitureID)
		{
			this.terrainID = terrainID;
			this.furnitureID = furnitureID;
		}

		public bool IsSameCombo(TerrainFurnitureCombo otherCombo)
		{
			return (terrainID == otherCombo.terrainID) && (furnitureID == otherCombo.furnitureID);
		}

		public string Write()
		{
			return "terrain: " + terrainID + ", furniute: " + furnitureID + ", map symbol: " + mapSymbol;
		}
	}
}
