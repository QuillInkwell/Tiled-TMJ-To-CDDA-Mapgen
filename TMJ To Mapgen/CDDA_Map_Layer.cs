using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TMJ_To_Mapgen
{
	class CDDA_Map_Layer
	{
		public List<TerrainFurnitureCombo> mapPoints;

		public CDDA_Map_Layer()
		{
			mapPoints = new List<TerrainFurnitureCombo>();
		}

		public string WriteLayer(int mapWidth, int mapHeight)
		{
			StringBuilder sb = new StringBuilder();
			StringWriter sw = new StringWriter(sb);
			int tileCount = 1;
			int row = 1;
			for(int i = 0; i < mapPoints.Count; i++)
			{
				if(tileCount == 1)
				{
					// Start of Line
					sw.Write("\"");
				}
				sw.Write(mapPoints[i].mapSymbol);
				if(tileCount == mapWidth)
				{
					if (row == mapHeight)
					{
						// End of Layer
						sw.Write("\"\n");
					}
					else
					{
						// End of line
						sw.Write("\",\n");
						row++;
					}
					tileCount = 1;
				}
				else
				{
					tileCount++;
				}
			}
			sw.WriteLine();
			sw.Close();
			return sw.ToString();
		}
	}
}
