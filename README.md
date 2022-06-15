# YouTubePlaylistSyncer
![image](https://user-images.githubusercontent.com/45084896/173921196-b98fa1e6-366c-4ebb-a50f-d368244a4532.png)

Queries the YouTube Data API for information about a playlist and downloads the entries to .mp3 files, skipping those with a matching filename.

Requires a Google API key saved in your Environment Variables to access the YouTube Data API.  See https://cloud.google.com/docs/authentication/api-keys.

Currently uses VideoLibrary for downloading and MediaToolkit to convert .mp4 to .mp3.
