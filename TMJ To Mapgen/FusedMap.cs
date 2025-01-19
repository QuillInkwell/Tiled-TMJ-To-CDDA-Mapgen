using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TMJ_To_Mapgen
{
	class FusedMap
	{
		public List<FusedMapLayer> fusedMapLayers;

		public FusedMap()
		{
			fusedMapLayers = new List<FusedMapLayer>();
		}

		public string WriteMap(int mapWidth, int mapHeight)
		{
			StringBuilder sb = new StringBuilder();
			StringWriter sw = new StringWriter(sb);

			foreach(FusedMapLayer layer in fusedMapLayers)
			{
				sw.Write(layer.WriteLayer(mapWidth, mapHeight));
			}
			sw.Close();
			return sw.ToString();
		}
	}
}
