using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TMJ_To_Mapgen
{
	class CDDA_Map
	{
		public List<CDDA_Map_Layer> cddaMapLayers;

		public CDDA_Map()
		{
			cddaMapLayers = new List<CDDA_Map_Layer>();
		}

		public string WriteMap(int mapWidth, int mapHeight)
		{
			StringBuilder sb = new StringBuilder();
			StringWriter sw = new StringWriter(sb);

			foreach(CDDA_Map_Layer layer in cddaMapLayers)
			{
				sw.Write(layer.WriteLayer(mapWidth, mapHeight));
			}
			sw.Close();
			return sw.ToString();
		}
	}
}
