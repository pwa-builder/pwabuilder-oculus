## Microsoft.PWABuilder.Oculus

This is PWABuilder's Oculus platform. It's used by PWABuilder to generate Oculus app packages that can be submitted to the Oculus store.

This platform uses the [Oculus CLI](https://developer.oculus.com/documentation/web/pwa-packaging/) to create Oculus app packages from PWAs.

### Running Locally

You will need [Docker](https://www.docker.com/products/docker-desktop/) and the [Azure CLI](https://learn.microsoft.com/en-us/cli/azure/install-azure-cli) to run this service locally.

Steps:

1. Run `az acr login -n pwabuilder` to authenticate with our Azure Container Registry.

2. Run `docker build -t pwa-oculus .` to build the Docker image, this may take a while the first time you run it.

4. Once the build is complete, run `docker run -p 80:80 pwa-oculus` to start the Docker container.

5. Visit `localhost` to see the Oculus packaging testing interface.

You can also try the published production service at https://pwabuilder-oculus.centralus.cloudapp.azure.com

Alternately, you can POST to `/packages/create` with the following JSON body:

```json
{
    "packageId": "app.webboard",
    "name": "WebBoard",
    "url": "https://webboard.app",
    "versionCode": 1,
    "versionName": "1.0.0.0",
    "signingMode": 1,
    "manifestUrl": "https://webboard.app/manifest.json",
    "manifest": {
        "dir": "ltr",
        "lang": "en",
        "name": "Webboard",
        "scope": "/",
        "display": "standalone",
        "start_url": "/",
        "short_name": "Webboard",
        "theme_color": "#FFFFFF",
        "description": "Enhance your work day and solve your cross platform whiteboarding needs with webboard! Draw text, shapes, attach images and more and share those whiteboards with anyone through OneDrive!",
        "orientation": "any",
        "background_color": "#FFFFFF",
        "related_applications": [],
        "prefer_related_applications": false,
        "screenshots": [
            {
                "src": "assets/screen.png"
            },
            {
                "src": "assets/screen.png"
            },
            {
                "src": "assets/screen.png"
            }
        ],
        "features": [
            "Cross Platform",
            "low-latency inking",
            "fast",
            "useful AI"
        ],
        "shortcuts": [
            {
                "name": "Start Live Session",
                "short_name": "Start Live",
                "description": "Jump direction into starting or joining a live session",
                "url": "/?startLive",
                "icons": [
                    {
                        "src": "icons/android/maskable_icon_192.png",
                        "sizes": "192x192"
                    }
                ]
            }
        ],
        "icons": [
            {
                "src": "icons/android/android-launchericon-64-64.png",
                "sizes": "64x64"
            },
            {
                "src": "icons/android/maskable_icon_192.png",
                "sizes": "192x192",
                "purpose": "maskable"
            },
            {
                "src": "icons/android/android-launchericon-48-48.png",
                "sizes": "48x48"
            },
            {
                "src": "icons/android/android-launchericon-512-512.png",
                "sizes": "512x512"
            },
            {
                "src": "icons/android/android-launchericon-28-28.png",
                "sizes": "28x28"
            }
        ]
    }
}
```

For `signingMode`, 0 = skip signing, 1 = create a new signing key, 2 = use an existing signing key (`existingSigningKey` must be specified). See [SigningMode.cs](https://github.com/pwa-builder/pwabuilder-oculus/blob/main/Microsoft.PWABuilder.Oculus/Models/SigningMode.cs) for details.

For `existingSigningKey`, this should be null if you're submitting a new app and don't already have an Android `.keystore` file. If you're uploading a new version of an existing app in Oculus Store or Oculus App Lab, `existingSigningKey` should be the set to the signing key information included in your initial PWABuilder Oculus zip download.

If `existingSigningKey` is specified, `keyStoreFile` should be the base64-encoded string of your existing .keystore file.

For full set of options for generating an Oculus package, see [OculusAppPackageOptions.cs](https://github.com/pwa-builder/pwabuilder-oculus/blob/main/Microsoft.PWABuilder.Oculus/Models/OculusAppPackageOptions.cs).

The generated zip file will contain:
- The Android App Package (APK) file, ready for upload to the Oculus Store. See [Oculus's PWA app submission documentation](https://developer.oculus.com/documentation/web/pwa-submit-app/).
- A .keystore file if you set `signingMode` to 1 (i.e. new signing key)
- A readme file showing the developer's next steps for uploading their APK file to the Oculus Store.