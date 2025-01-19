using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMJ_To_Mapgen
{
	class OvermapTile
	{
		public List<int> terrainsIds;
		public List<int> furnituresIds;

		public int x;
		public int y;
		public int z;

		public OvermapTile(int x, int y, int z)
		{
			this.x = x;
			this.y = y;
			this.z = z;

			terrainsIds = new List<int>();
			furnituresIds = new List<int>();
		}

		
	}
}
