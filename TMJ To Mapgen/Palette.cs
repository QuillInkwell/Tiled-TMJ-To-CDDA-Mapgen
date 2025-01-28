using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Windows;

namespace TMJ_To_Mapgen
{
	class Corpse
	{
		public string group;
		public int age;
	}

	class Trap 
	{
		public string trap;
		public bool remove;
	}

	class Sign
	{
		public string signage;
		public string snippet;
		public string furniture;
	}

	class Graffiti
	{
		public string text;
		public string snippet;
	}

	class Monster
	{
		public string monster;
		public bool friendly;
		public string name;
		public int[] chance;
	}

	class Monsters
	{
		public string monster;
		public float density;
		public int[] chance;
	}

	class Vehicle
	{
		public string vehicle;
		public int[] chance;
		public int rotation;
		public int fuel;
		public int status;
		public string faction;
	}

	class Toilet
	{
		public int[] amount;
	}

	class GasPump
	{
		public int[] amount;
		public string fuel;
	}

	class VendingMachine
	{
		public string item_group;
		public bool reinforced;
		public bool lootable;
		public bool powered;
	}

	class Item
	{
		public string item;
		public int[] chance;
		public int[] amount;
		public int[] repeat;
	}

	class Items
	{
		public string item;
		public int[] chance;
		public int[] repeat;
		public string faction;
	}

	class SealedItem
	{
		public string furniture;
		public string item;
		public string items;
	}

	class Loot
	{
		public string group;
		public string item;
		public int chance;
		public int ammo;
		public int magazine;
		public string variant;
	}

	class Liquid
	{
		public string liquid;
		public int[] amount;
		public int[] chance;
	}

	class Computer
	{
		public string name;
		public Dictionary<string, string>[] options;
		public Dictionary<string, string>[] failures;
		public int security;
		public string access_dennied;
		public string[] eocs;
		public string[] chat_topics;
	}


	class Palette
	{
		public string type = "palette";
		public string id = "TMJ_Generated_Palette";

		public Dictionary<string, string> terrain;
		public Dictionary<string, string> furniture;
		public Dictionary<string, Sign> signs;
		public Dictionary<string, Graffiti> grafiti;
		public Dictionary<string, Trap> traps;
		public Dictionary<string, Vehicle> vehicles;
		public Dictionary<string, Monster> monster;
		public Dictionary<string, Monsters> monsters;
		public Dictionary<string, Corpse> corpses;
		public Dictionary<string, Toilet> toilets;
		public Dictionary<string, GasPump> gaspumps;
		public Dictionary<string, VendingMachine> vendingmachines;
		public Dictionary<string, Item> item;
		public Dictionary<string, Items> items;
		public Dictionary<string, SealedItem> sealed_items;
		public Dictionary<string, Loot> loot;
		public Dictionary<string, Liquid> liquids;
		public Dictionary<string, Computer> computers;

		public Palette()
		{
			terrain = new Dictionary<string, string>();
			furniture = new Dictionary<string, string>();
		}

		public void SetTerrains(List<TerrainFurnitureCombo> combos)
		{
			foreach(TerrainFurnitureCombo combo in combos)
			{
				terrain.Add(combo.mapSymbol.ToString(), combo.CDDATerrainID);
				if (combo.CDDAFurnitureID == "") continue;
				furniture.Add(combo.mapSymbol.ToString(), combo.CDDAFurnitureID);
			}
		}

		public string WritePalette()
		{
			JsonSerializerSettings settings = new JsonSerializerSettings();
			settings.Formatting = Formatting.Indented;
			settings.NullValueHandling = NullValueHandling.Ignore;
			return JsonConvert.SerializeObject(this, settings);
		}
	}
}
