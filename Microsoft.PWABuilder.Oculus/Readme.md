## Microsoft.PWABuilder.Oculus

This is PWABuilder's Oculus platform. It's used by PWABuilder to generate Oculus app packages that can be submitted to the Oculus store.

### Running locally

Open the Microsoft.PWABuilder.Oculus.sln in Visual Studio 2022. Hit F5 to run the web app. It will launch https://localhost:7115.

To test the package generation, load `/`, and it will show a page where you can test the service.

Alternately, you can POST to `/api/packages/create` with the following JSON body:

```json
{
	"packageId": "com.sadchonks",
	"name": "Sad Chonks",
	"versionCode": 1,
    "versionName": "1.0.0.0",
    "signingMode": 0, // 0 = unsigned. 1 = create a new signing key. 2 = use existing signing key (must specify existingSigningKey object)
	"manifestUrl": "https://sadchonks.com/manifest.json",
    "existingSigningKey": {
        "keyStoreFile": "", // base64-encoded string of your existing .keystore file
        "storePassword": "", 
        "alias": "",
        "password": ""
    },
	"manifest": {
      "short_name": "Chonks",
      "name": "Sad Chonks",
      "description": "Your daily source for Sad Chonks",
      "categories": ["cats", "memes"],
      "screenshots": [
        {
          "src": "/chonkscreenshot1.jpeg",
          "type": "image/jpeg",
          "sizes": "728x409",
          "label": "App on homescreen with shortcuts",
          "platform": "play"
        },
        {
          "src": "/chonkscreenshot2.jpg",
          "type": "image/jpeg",
          "sizes": "551x541",
          "label": "Really long text describing the screenshot above which is basically a picture showing the app being long pressed on Android and the WebShortcuts popping out",
          "platform": "xbox"
        }
      ],
      "icons": [
        {
          "src": "/favicon.png",
          "type": "image/png",
          "sizes": "128x128"
        },
        {
          "src": "/kitteh-192.png",
          "type": "image/png",
          "sizes": "192x192"
        },
        {
          "src": "/kitteh-512.png",
          "type": "image/png",
          "sizes": "512x512"
        }
      ],
      "start_url": "/saved",
      "background_color": "#3f51b5",
      "display": "standalone",
      "scope": "/",
      "theme_color": "#3f51b5",
      "shortcuts": [
        {
          "name": "New Chonks",
          "short_name": "New",
          "url": "/?shortcut",
          "icons": [{"src": "/favicon.png", "sizes": "128x128"}]
        },
        {
          "name": "Saved Chonks",
          "short_name": "Saved",
          "url": "/saved?shortcut",
          "icons": [{"src": "/favicon.png", "sizes": "128x128"}]
        }
      ]
    }
}

```

For full set of options for generating an Oculus package, see [OculusAppPackageOptions.cs](https://github.com/pwa-builder/pwabuilder-oculus/blob/main/Microsoft.PWABuilder.Oculus/Models/OculusAppPackageOptions.cs).