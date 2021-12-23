## Microsoft.PWABuilder.Oculus

This is PWABuilder's Oculus platform. It's used by PWABuilder to generate Oculus app packages that can be submitted to the Oculus store.

This platform uses the [Oculus CLI](https://developer.oculus.com/documentation/web/pwa-packaging/) to create Oculus app packages from PWAs.

### Running locally

Open the Microsoft.PWABuilder.Oculus.sln in Visual Studio 2022. Hit F5 to run the web app. It will launch https://localhost:7115.

To test the package generation, load `https://localhost:7115`, and it will show a page where you can test the service.

You can also try the published production service at https://pwabuilder-oculus.centralus.cloudapp.azure.com

Alternately, you can POST to `/api/packages/create` with the following JSON body:

```json
{
  "packageId": "com.sadchonks",
  "name": "Sad Chonks",
  "url": "https://sadchonks.com",
  "versionCode": 1,
  "versionName": "1.0.0.0",
  "signingMode": 0,
  "manifestUrl": "https://sadchonks.com/manifest.json",
  "existingSigningKey": {
    "keyStoreFile": "",
    "storePassword": "",
    "alias": "",
    "password": ""
  },
  "manifest": {
    "short_name": "Chonks",
    "name": "Sad Chonks",
    "description": "Your daily source for Sad Chonks",
    "categories": [
      "cats",
      "memes"
    ],
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
        "icons": [
          {
            "src": "/favicon.png",
            "sizes": "128x128"
          }
        ]
      },
      {
        "name": "Saved Chonks",
        "short_name": "Saved",
        "url": "/saved?shortcut",
        "icons": [
          {
            "src": "/favicon.png",
            "sizes": "128x128"
          }
        ]
      }
    ]
  }
}
```

For `signingMode`, 0 = skip signing, 1 = create a new signing key, 2 = use an existing signing key (`existingSigningKey` must be specified).

If `existingSigningKey` is specified, `keyStoreFile` should be a base64-encoded string of your existing .keystore file.

For full set of options for generating an Oculus package, see [OculusAppPackageOptions.cs](https://github.com/pwa-builder/pwabuilder-oculus/blob/main/Microsoft.PWABuilder.Oculus/Models/OculusAppPackageOptions.cs).

The generated zip file will contain:
- The Android App Package (APK) file, ready for upload to the Oculus Store. See [Oculus's PWA app submission documentation](https://developer.oculus.com/documentation/web/pwa-submit-app/).
- A .keystore file if you set `signingMode` to 1 (meaning new signing key)
- A readme file showing the developer's next steps for uploading their APK file to the Oculus Store.