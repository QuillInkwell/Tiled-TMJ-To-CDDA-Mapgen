using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace TMJ_To_Mapgen
{
	class CDDATileInfo
	{
		public int pixelscale;
		public int width;
		public int height;
		public int zlevel_height;
		public bool iso;
		public float retract_dist_min;
		public float retract_dist_max;
	}

	struct CDDATileId
	{
		public string id;
		public List<string> idArray;
	}

	struct CDDATileFG
	{
		public int spriteId;
		public List<CDDATileFGObj> spriteIds;
	}

	struct CDDATileFGObj
	{
		public int weight;
		public int spriteId;
	}

	class CDDATile
	{
		public CDDATileId id;
		public CDDATileFG fg;
	}

	class CDDATileset
	{
		public string file;
		public int sprite_width;
		public int sprite_height;
		public int sprite_offset_x;
		public int sprite_offset_y;

	}

	class CDDATiles
	{
		public List<CDDATileInfo> tileInfo;
		public List<CDDATileset> tiles_new;
	}
}
