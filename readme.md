# Personalized Music Manager

Simple manager for the Skyrim "Personalized Music" mod.

Usage is simple:
1. Run the exe once to generate `music.jsonc`.
	- This file is used to set the user configuration, including paths and music.
1. Set `musicPath` to the folder where your music files are stored.
1. Set `playlistsPath` to the folder where the mod playlist folders are located.
1. Re-run the program to generate `playlists.jsonc`.
	- This file is used to track the playlists available based on the folders in the mod's `music` folder. It automatically determines the filenames used for each folder and the maximum amount of tracks allowed per playlist based on the file and folder structure.
1. Add your music to `playlists` in `music.jsonc`, for example:
	```json
	{
		"playlists": {
			"ExploreAllAreas": [
				"mus_explore_dlc2solstheim_08.xwm",
				"Ramon Molesworth - GRUINS.xwm",
				"Ramon Molesworth - 02.xwm"
			],
			"ExploreSnow": [
				"Ramon Molesworth - snowing.xwm",
				"Ramon Molesworth - oversnow.xwm",
				"Ramon Molesworth - 18.xwm",
				"Dan Goodale - 21.xwm"
			]
		}
	}
	```
	- Make sure the playlist names match those in `playlists.jsonc`.
1. Run the program one final time to synchronize the music files.
	- This will **delete** the existing music files in the playlists folder and copy the music files configured to their equivalent playlist folders and rename them appropriately.
	- Any time you make changes to the playlists, simply re-run the program to synchronize again.

## Planned features
- A GUI, preferably in a way that can work cross-platform (thinking of Wine users on Linux).
- Check if `.wav` and `.xwm` files are being mixed.
- Possibly integrate music file conversion with ffmpeg to xWMA with the encoder from the DirectX SDK.
- Maybe implement a way to share music files between multiple playlists without the need of duplicate entries.
- Implement hard or symbolic links instead of copying, it'd be preferable that this works cross-platform.
