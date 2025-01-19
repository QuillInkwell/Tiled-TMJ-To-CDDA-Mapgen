using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace TMJ_To_Mapgen
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private static string SYMBOLS = "!#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[]^_`abcdefghijklmnopqrstuvwxyz{|}~€ƒ„…†‡ˆ‰Š‹ŒŽ‘’“”•–—˜™š›œžŸ¡¢£¤¥¦§¨©ª«¬®¯°±²³´µ¶·¸¹º»¼½¾¿ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÚÛÜÝÞßàáâãäåæçèé";
		public static int CDDA_OVERMAP_TILE_WIDTH = 24;

		private Dictionary<int, string> CDDAConversionKey;
		private Palette palette;

		private ManualPaletteDesigner paletteDesigner;
		
		private FusedMap fusedMap;
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

			if(result == true)
			{
				string fileName = dialog.FileName;

				string content = File.ReadAllText(fileName);
				Map map = JsonConvert.DeserializeObject<Map>(content);

				CreateConversionKey(map);

				if (!(bool)SplitFurnitureAndTerrainCheckbox.IsChecked)
				{
					CreateLargeMapExport(map);
				}
				else
				{
					CreateTileByTileMapExport(map);
				}
			}
		}

		private void CreateTileByTileMapExport(Map map)
		{
			mapWidth = map.width;
			mapHeight = map.height;

			List<int> uniqueFurntiures = new List<int>();
			List<int> uniqueTerrains = new List<int>();

			// Create the Tiles and assign co-ords
			int mapChunksWide = mapWidth / CDDA_OVERMAP_TILE_WIDTH;
			int mapChunksHeight = mapHeight / CDDA_OVERMAP_TILE_WIDTH;
			int mapZLayers = map.layers.Count() / 3;

			List<OvermapTile> tiles = new List<OvermapTile>();

			for (int z = 0; z < mapZLayers; z++)
			{
				for (int y = 0; y < mapChunksHeight; y++)
				{
					for(int x = 0; x < mapChunksWide; x++)
					{
						tiles.Add(new OvermapTile(x, y, z));
					}
				}
			}
			
			// Loop through the entire map layer by layer and assign divide into chunks
			for (int i = 0; i < map.layers.Length; i = i + 3)
			{

				for (int ii = 0; ii < map.layers[i].data.Length; ii++)
				{
					int[] combo = GetTerrainAndFurnitureFromMapPoint(i, ii, map);

					int mapX = ii % mapWidth;
					int mapY = ii / mapWidth;

					int mapChunkX = mapX / CDDA_OVERMAP_TILE_WIDTH;
					int mapChunkY = mapY / CDDA_OVERMAP_TILE_WIDTH;
					int mapZ = i / 3;

					OvermapTile tile = GetTileFromCoords(mapChunkX, mapChunkY, mapZ, tiles);

					tile.terrainsIds.Add(combo[0]);
					tile.furnituresIds.Add(combo[1]);

					// Check if these ids are being used for the first time and add them to the lists if so
					if(uniqueTerrains.Count == 0 || !uniqueTerrains.Contains(combo[0]))
					{
						uniqueTerrains.Add(combo[0]);
					}
					if(uniqueFurntiures.Count == 0 || !uniqueFurntiures.Contains(combo[1]))
					{
						uniqueFurntiures.Add(combo[1]);
					}
				}
			}
			
			// Create a TerFur combos just so we can feed them to the palette designer
			List<TerrainFurnitureCombo> terrainFurnitureCombos = new List<TerrainFurnitureCombo>();
			foreach (int terrain in uniqueTerrains)
			{
				var combo = new TerrainFurnitureCombo(terrain, 0);
				combo.CDDATerrainID = CDDAConversionKey[terrain];
				terrainFurnitureCombos.Add(combo);
			}

			foreach (int furniture in uniqueFurntiures)
			{
				var combo = new TerrainFurnitureCombo(0, furniture);
				combo.CDDAFurnitureID = CDDAConversionKey[furniture];
				terrainFurnitureCombos.Add(combo);
			}

			paletteDesigner = new ManualPaletteDesigner();
			paletteDesigner.Owner = this;
			paletteDesigner.CreateUserControls(terrainFurnitureCombos);
			paletteDesigner.mainWindow = this;
			paletteDesigner.Show();
		}

		private OvermapTile GetTileFromCoords(int x, int y, int z, List<OvermapTile> tiles)
		{
			foreach (OvermapTile tile in tiles)
			{
				if (tile.x == x && tile.y == y && tile.z == z)
				{
					return tile;
				}
			}
			return null;
		}

		private void CreateLargeMapExport(Map map)
		{
			fusedMap = new FusedMap();
			mapWidth = map.width;
			mapHeight = map.height;

			List<TerrainFurnitureCombo> uniqueCombos = new List<TerrainFurnitureCombo>();

			for (int i = 0; i < map.layers.Length; i = i + 3)
			{
				// Loop  through all layers
				var newLayer = new FusedMapLayer();
				fusedMap.fusedMapLayers.Add(newLayer);

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
			AssignMapKeys(uniqueCombos, fusedMap);

			if (!(bool)ManualPaletteCheckbox.IsChecked)
			{
				WritePalette(uniqueCombos);
				WriteMap(fusedMap, map.width, map.height);
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

		private void LoadPalette_Button(object sender, RoutedEventArgs e)
		{
			var dialog = new Microsoft.Win32.OpenFileDialog();
			dialog.FileName = "Palette File";
			dialog.DefaultExt = ".json";
			dialog.Filter = "Palette File (.json)|*.json";

			bool? result = dialog.ShowDialog();

			if(result == true)
			{
				string fileName = dialog.FileName;

				string content = File.ReadAllText(fileName);

				palette = JsonConvert.DeserializeObject<Palette>(content);
			}
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

		private void AssignMapKeys(List<TerrainFurnitureCombo> uniqueCombos, FusedMap fusedMap)
		{

			
			if ((bool)ManualPaletteCheckbox.IsChecked)
			{
				paletteDesigner = new ManualPaletteDesigner();
				paletteDesigner.Owner = this;
				paletteDesigner.CreateUserControls(uniqueCombos);
				paletteDesigner.mainWindow = this;
				paletteDesigner.Show();
				return;
			}

			// Assing each combination into a symbol
			// TODO: Read keys from palettes before assigning fresh keys
			int charIndex = 0;
			foreach (TerrainFurnitureCombo combo in uniqueCombos)
			{
				combo.mapSymbol = SYMBOLS[charIndex];
				charIndex++;
			}

			AssignAllOtherKeys(uniqueCombos);
		}

		public void Export(List<TerrainFurnitureCombo> combos)
		{
			if (!(bool)SplitFurnitureAndTerrainCheckbox.IsChecked)
			{
				AssignAllOtherKeys(combos);

				WritePalette(combos);
				WriteMap(fusedMap, mapWidth, mapHeight);
			}
			else
			{

			}

		}

		private void AssignAllOtherKeys(List<TerrainFurnitureCombo> combos)
		{
			foreach (FusedMapLayer layer in fusedMap.fusedMapLayers)
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

		private void WriteMap(FusedMap map, int mapWidth, int mapHeight)
		{
			File.WriteAllText("C:\\Users\\Marc\\OneDrive\\Desktop\\outputMap.json", map.WriteMap(mapWidth, mapHeight));
		}

		// Debug Methods
		private void WriteComboFile(List<TerrainFurnitureCombo> uniqueCombos)
		{
			string AllCombos = "";
			foreach (TerrainFurnitureCombo combo in uniqueCombos)
			{
				AllCombos = String.Concat(AllCombos, combo.Write(), "\n");
			}
			File.WriteAllText("C:\\Users\\Marc\\OneDrive\\Desktop\\output.txt", AllCombos);
		}


	}
}
