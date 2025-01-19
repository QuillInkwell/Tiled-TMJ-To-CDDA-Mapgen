using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.IO;
using Newtonsoft.Json;

namespace TMJ_To_Mapgen
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public static string SYMBOLS = "!#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[]^_`abcdefghijklmnopqrstuvwxyz{|}~€ƒ„…†‡ˆ‰Š‹ŒŽ‘’“”•–—˜™š›œžŸ¡¢£¤¥¦§¨©ª«¬®¯°±²³´µ¶·¸¹º»¼½¾¿ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÚÛÜÝÞßàáâãäåæçèé";
		public static int CDDA_OVERMAP_TILE_WIDTH = 24;

		private Dictionary<int, string> CDDAConversionKey;
		private Palette palette;

		private ManualPaletteDesigner paletteDesigner;
		
		private CDDA_Map cddaMap;
		private int mapWidth;
		private int mapHeight;

		public MainWindow()
		{
			InitializeComponent();
		}

		private void LoadTileMap_Button(object sender, RoutedEventArgs e)
		{
			// Load the Tiled Map
			var dialog = new Microsoft.Win32.OpenFileDialog();
			dialog.FileName = "Tiled Map";
			dialog.DefaultExt = ".tmj";
			dialog.Filter = "Tiled Maps (.tmj)|*.tmj";

			bool? result = dialog.ShowDialog();

			if (result == true)
			{
				string fileName = dialog.FileName;

				string content = File.ReadAllText(fileName);
				Map map = JsonConvert.DeserializeObject<Map>(content);

				CreateConversionKey(map);
				CreateExport(map);
			}
		}

		private void CreateExport(Map map)
		{
			cddaMap = new CDDA_Map();
			mapWidth = map.width;
			mapHeight = map.height;

			List<TerrainFurnitureCombo> uniqueCombos = new List<TerrainFurnitureCombo>();

			for (int i = 0; i < map.layers.Length; i = i + 3)
			{
				// Loop  through all layers
				var newLayer = new CDDA_Map_Layer();
				cddaMap.cddaMapLayers.Add(newLayer);

				for (int ii = 0; ii < map.layers[i].data.Length; ii++)
				{
					int[] combo = GetTerrainAndFurnitureFromMapPoint(i, ii, map);

					TerrainFurnitureCombo point = new TerrainFurnitureCombo(combo[0], combo[1]);
					newLayer.mapPoints.Add(point);

					if (IsUniqueCombo(uniqueCombos, point))
					{
						uniqueCombos.Add(point);
					}
				}
			}

			AssignCDDAIds(uniqueCombos);
			uniqueCombos = uniqueCombos.OrderBy(o => o.CDDATerrainID).ToList();
			

			// Export right away unless we plan to 
			if (!(bool)ManualPaletteCheckbox.IsChecked)
			{
				AutoAssignMapKeys(uniqueCombos, cddaMap);
				Export(uniqueCombos);
			}
			else
			{
				OpenPaletteDesigner(uniqueCombos);
			}
		}

		private int[] GetTerrainAndFurnitureFromMapPoint(int i, int ii, Map map)
		{
			//Loop through all points in layer
			int terrainID = 0;
			if (map.layers[i + 1].data[ii] != 0)
			{
				// If Terrain has a value use that
				terrainID = map.layers[i + 1].data[ii];
			}
			else
			{
				// Otherwise use the value from the fill terain
				terrainID = map.layers[i].data[ii];
			}
			// Set Furniture to whatever is in the array, even if that's a 0 as in nothing
			int furnitureID = map.layers[i + 2].data[ii];
			
			int[] combo = new int[2];
			combo[0] = terrainID;
			combo[1] = furnitureID;
			
			return combo;
		}

		private bool IsUniqueCombo(List<TerrainFurnitureCombo> combos, TerrainFurnitureCombo comboToCheck)
		{
			if (combos.Count <= 0) return true;
			
			foreach(TerrainFurnitureCombo combo in combos)
			{
				if (combo.IsSameCombo(comboToCheck)) return false;
			}
			return true;
		}

		private void CreateConversionKey(Map map)
		{
			CDDAConversionKey = new Dictionary<int, string>();
			CDDAConversionKey.Add(0, "t_open_air");

			foreach (Tileset tileset in map.tilesets)
			{
				foreach (Tile tile in tileset.tiles)
				{
					foreach (TileProperty property in tile.properties)
					{
						if(property.name == "id")
						{
							CDDAConversionKey.Add(tile.id + tileset.firstgid, property.value);
						}
					}
				}
			}
		}

		private void AssignCDDAIds(List<TerrainFurnitureCombo> combos)
		{
			foreach (TerrainFurnitureCombo combo in combos)
			{
				if (combo.furnitureID != 0)
				{
					string furniture = "";
					CDDAConversionKey.TryGetValue(combo.furnitureID, out furniture);
					combo.CDDAFurnitureID = furniture;
				}

				string terrain = "";
				CDDAConversionKey.TryGetValue(combo.terrainID, out terrain);
				combo.CDDATerrainID = terrain;
			}
		}

		private void OpenPaletteDesigner(List<TerrainFurnitureCombo> combos)
		{
			paletteDesigner = new ManualPaletteDesigner();
			paletteDesigner.Owner = this;
			paletteDesigner.CreateUserControls(combos);
			paletteDesigner.mainWindow = this;
			paletteDesigner.Show();
		}

		private void AutoAssignMapKeys(List<TerrainFurnitureCombo> uniqueCombos, CDDA_Map fusedMap)
		{
			// TODO: Read keys from palettes before assigning fresh keys
			int charIndex = 0;
			foreach (TerrainFurnitureCombo combo in uniqueCombos)
			{
				combo.mapSymbol = SYMBOLS[charIndex];
				charIndex++;
			}
		}

		public void Export(List<TerrainFurnitureCombo> combos)
		{
			AssignAllOtherKeys(combos);

			WritePalette(combos);
			WriteMap(cddaMap, mapWidth, mapHeight);
		}

		private void AssignAllOtherKeys(List<TerrainFurnitureCombo> combos)
		{
			foreach (CDDA_Map_Layer layer in cddaMap.cddaMapLayers)
			{
				foreach (TerrainFurnitureCombo combo in layer.mapPoints)
				{
					foreach (TerrainFurnitureCombo otherCombo in combos)
					{
						// assign all other entires symbols based on the key
						if (otherCombo.IsSameCombo(combo)) combo.mapSymbol = otherCombo.mapSymbol;
					}
				}
			}
		}

		private void WritePalette(List<TerrainFurnitureCombo> uniqueCombos)
		{
			Palette p = new Palette();
			p.SetTerrains(uniqueCombos);
			File.WriteAllText("C:\\Users\\Marc\\OneDrive\\Desktop\\outputPalette.json", p.WritePalette());
		}

		private void WriteMap(CDDA_Map map, int mapWidth, int mapHeight)
		{
			File.WriteAllText("C:\\Users\\Marc\\OneDrive\\Desktop\\outputMap.json", map.WriteMap(mapWidth, mapHeight));
		}

	}
}
