using System;
using System.Collections.Generic;
using System.Windows;


namespace TMJ_To_Mapgen
{
	/// <summary>
	/// Interaction logic for ManualPaletteDesigner.xaml
	/// </summary>
	public partial class ManualPaletteDesigner : Window
	{
		List<ComboKeyControl> comboControls;
		public MainWindow mainWindow;
		public List<TerrainFurnitureCombo> combos;

		public ManualPaletteDesigner()
		{
			InitializeComponent();
		}

		public void CreateUserControls(List<TerrainFurnitureCombo> combos)
		{
			CombosNumberLabel.Content = combos.Count.ToString();
			
			this.combos = combos;
			comboControls = new List<ComboKeyControl>();

			foreach(TerrainFurnitureCombo combo in combos)
			{
				ComboKeyControl newControl = new ComboKeyControl();
				newControl.Furniture_ID_TextBlock.Text = combo.CDDAFurnitureID;
				newControl.Terrain_ID_Label.Text = combo.CDDATerrainID;

				comboControls.Add(newControl);

				Stacker.Children.Add(newControl);
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			foreach(ComboKeyControl comboKey in comboControls)
			{
				foreach(TerrainFurnitureCombo combo in combos)
				{
					if(comboKey.Furniture_ID_TextBlock.Text == combo.CDDAFurnitureID &&
						comboKey.Terrain_ID_Label.Text == combo.CDDATerrainID)
					{
						combo.mapSymbol = comboKey.Map_Key_Textbox.Text.ToCharArray()[0];
					}
				}
			}

			bool exportSuccess = mainWindow.Export(combos);
			
			if (exportSuccess) Close();
		}

		private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			Scroll_Window.MaxHeight = Height - 100;
		}
	}
}
