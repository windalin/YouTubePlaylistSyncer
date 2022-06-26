# YouTubePlaylistSyncer
![image](https://user-images.githubusercontent.com/45084896/175823654-889211a1-7520-42c8-830a-3032f3d50b1d.png)

Queries the YouTube Data API for information about a playlist and downloads the entries to .mp3 files, skipping those with a matching filename.

Requires a Google API key saved in your Environment Variables to access the YouTube Data API.  See https://cloud.google.com/docs/authentication/api-keys.

Currently uses VideoLibrary for downloading and MediaToolkit to convert .mp4 to .mp3.  Uses FlaUI 3 for automated UI tests.
