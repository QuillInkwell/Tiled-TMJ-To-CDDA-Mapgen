Disclaimer: I built this on WPF because I made it mostly for myself and my own needs. And WPF is what I know. Anyway that just means it'll pretty much only run Windows unless you run it through a compatibility layer APP like Wine if you are on Linux.
At least somone got one of my WPF modding tools to work on Linux using Wine in the past. Not sure about recently.

**Tilesets**
***
This does not include any tools for making your own tilesets. That part is still pretty manual. You need to grab the images from the tilesets of your choice and create a tilesheet yourself. You could use the gfx images directly too I guess. But I find those too 
large to use for most mapping projects. Once you create the Tileset from the image in Tiled you need to add a custom property to each tile called "id" (yes case senstive). The value should be the CDDA id of that tile. All tiles need to be id'd or the exporter 
won't be able to do the conversion properly.

I use Asesprite personally but along as you use a pixel art porgram you should be good. I don't super recomend standard image editing software. I found Affinity Photo would introduce wierd anti-aliasing whenever I copied sprites over which was annoying to deal
with and changed the look of sprites.

![image](https://github.com/user-attachments/assets/17f59899-a95f-4f36-8c1c-31381781cfbe)
Example of a Tileset

![image](https://github.com/user-attachments/assets/b7517674-30b3-4e43-9782-9279dea261a8)
You can add a property by having a tile selected and right clicking anywhere in the custom properties panel or the blue + in the corner. You can't do this from the maps you need to be on the Tileset itself.

You can use different size tilesets to be able to use the tall and large graphics alongside the normal graphics. But they won't always display correctly.
For reference ChibiUltica's Sizes are:
32x32 Normal
32x64 Tall
32x96 Taller
80x64 Big
64x64 Large
96x96 Giant
128x128 Huge

In the future when I sort out some problems it will be possible to just cherry pick the tiles you want from the GFX images of your choosing. And Generate a Tiled Tileset and tilesheet image that way. But currently that's a lot of work for not much reward (to me
personally) so it's a low priority.

I will probably start storing any palettes I work with in here soon so people can see an example and grab some palettes to start working with right away if they feel so inclined. But get ready for mostly Aftershock stuff.

**Layers**
***
You don't need to name your layers anything special but the total number and ordering is important. You should have 3 layers for each z-level (do not group them...yet.) Layers should be ordered from lowest Z-level to highest z-level. For each z-level the ordering is:
Fill Terrain
Terrain
Furniture

Don't mix up your terrain and furniture tiles because what will happen is your export will still suceed (usually) but CDDA will complain.

Fill Terrain is mostly cosmetic and useful for setting your ground cover actually. The exporter will asign the fill terrain to any tiles that don't have a terrain defined in the terrain layer.
Terrain overrides the Fill Terrain with whatever tiles it ha.
Furniture is self evident and doesn't have any special behavior.

So a full map would look like:
![image](https://github.com/user-attachments/assets/48e90380-9712-4f4e-95b4-dbc5ad76c40c)

Don't add any extra layers because that will upset and confuse the exporter greatly. I currently don't support adding any objects primarily because I find it easier to just do the finishing touches in JSON. This tool is more like a useful way to knock out
the bulk of the mapping work in a big project. But isn't yet intended to take you all the way to the finish line. The Tiled Co-ordinates do match up perfectly with cdda co-ordinates so you can use that as a super easy hack whenever you need an x and y.

**Export Output**
***
IMPORTANT:
You want to Export your tiled maps as TMJ. Additionally be sure to enable embeding your tilesets so the exporter can read the id data:
You find the options in the preferences menu under the edit drop down.
![image](https://github.com/user-attachments/assets/936b57a4-4620-455f-b8db-63417cbec59d)

![image](https://github.com/user-attachments/assets/141755c1-f107-4049-a85a-d5a3213a143e)
The Exporter will always generate a palette file and Map for you which look like this:
![image](https://github.com/user-attachments/assets/307f8623-053c-487c-826d-64226415f131)
Output map
![image](https://github.com/user-attachments/assets/b370a227-baf8-4768-ad1c-0f3394fb5da2)
Output Palette

If you leave it on automode it will try to automatically generate Map keys for you with no regard for how the export looks in the end. If you turn on Manual Palette designer it will pop open a window that lets you define your palette's keys yourself. Don't use
space since that is auto-defined for any tile that is empty. If you use a duplicate key anywhere it will complain and probably crash.

![image](https://github.com/user-attachments/assets/e147c2b2-b4ef-478a-b8af-0c3be89b74e4)

I want to make it able to load CDDA palettes in the future so you don't always have to define your keys yourself. But that's a tall order since CDDA does some funky stuff with their JSON. Gonna need some time to cook up something that works.




