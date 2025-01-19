using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMJ_To_Mapgen
{
	class Map
	{
		public int compressionlevel;
		public int height;
		public bool infinite;
		public Layer[] layers;
		public int nextlayerid;
		public int nextobjectid;
		public string orientation;
		public string renderorder;
		public string tiledversion;
		public int tileheight;
		public Tileset[] tilesets;
		public int tilewidth;
		public string type;
		public string version;
		public int width;
	}
}
