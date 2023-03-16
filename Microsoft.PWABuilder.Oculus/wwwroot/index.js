const codeArea = document.querySelector("textarea");
const submitBtn = document.querySelector("#submitBtn");
const resultsDiv = document.querySelector("#results");
const spinner = document.querySelector(".spinner-border");

submitBtn.addEventListener("click", () => submit());

setCode(getSimpleJson());
codeArea.scrollTop = 0;

function setCode(options) {
    const code = JSON.stringify(options, undefined, 4);
    codeArea.value = code;
    codeArea.scrollTop = 1000000;
}

function getSimpleJson() {
    return {
        "packageId": "app.webboard",
        "name": "WebBoard",
        "url": "https://webboard.app",
        "versionCode": 1,
        "versionName": "1.0.0.0",
        "signingMode": 1,
        "manifestUrl": "https://webboard.app/manifest.json",
        "manifest": getManifest()
    }
}


function getManifest() {
    return {
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
                "icons": [{ "src": "icons/android/maskable_icon_192.png", "sizes": "192x192" }]
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

async function submit() {
    resultsDiv.textContent = "";

    setLoading(true);
    try {
        // Convert the JSON to an object and back to a string to ensure proper formatting.
        const options = JSON.stringify(JSON.parse(codeArea.value));
        const response = await fetch("/packages/create", {
            method: "POST",
            body: options,
            headers: new Headers({ 'content-type': 'application/json', 'platform-identifier': 'ServerUI', 'platform-identifier-version': '1.0.0' }),
        });
        if (response.status === 200) {
            const data = await response.blob();
            const url = window.URL.createObjectURL(data);
            window.location.assign(url);

            resultsDiv.textContent = "Success, download started 😎";
        } else {
            const responseText = await response.text();
            resultsDiv.textContent = `Failed. Status code ${response.status}, Error: ${response.statusText}, Details: ${responseText}`;
        }
    } catch (err) {
        resultsDiv.textContent = "Failed. Error: " + err;
    }
    finally {
        setLoading(false);
    }
}

function setLoading(state) {
    submitBtn.disabled = state;
    if (state) {
        spinner.classList.remove("d-none");
    } else {
        spinner.classList.add("d-none");
    }
}