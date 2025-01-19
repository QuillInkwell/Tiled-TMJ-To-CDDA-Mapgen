using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMJ_To_Mapgen
{
	class TileProperty
	{
		public string name;
		public string type;
		public string value;
	}

	class Tile
	{
		public int id;
		public TileProperty[] properties;
	}

	class Tileset
	{
		public Tile[] tiles;
		public int firstgid;
	}
}
