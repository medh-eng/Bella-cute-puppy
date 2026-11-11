# Play Console — every form, with the answer to give

Work through **Play Console → your app → Dashboard → "Set up your app"**. Each task below is one
item on that list. All answers describe the game exactly as built: it has no network code, no
advertising, no purchases, no accounts, and requests no Android permissions.

> Google changes these forms from time to time. If a question appears that is not listed here,
> the rule of thumb is: **Bella collects nothing, shows no ads, sells nothing, links nowhere.**

## 1. Privacy policy

A privacy-policy **web address** is compulsory for apps aimed at children.

The policy page is already written: `docs/privacy.html`. To give it an address:

1. On GitHub: repository **Settings → Pages → Build and deployment → Source: "Deploy from a branch"**
   → Branch **main**, folder **/docs** → Save. (The repository must be public for free GitHub Pages.)
2. After a minute the page is live at
   `https://medh-eng.github.io/Bella-cute-puppy/privacy.html`
3. Paste that address into Play Console.

Before publishing, open `docs/privacy.html` and replace **CONTACT-EMAIL-HERE** with a real address.

## 2. App access

**All functionality is available without special access.** (No login.)

## 3. Ads

**No, my app does not contain ads.**

## 4. Content rating (IARC questionnaire)

- Category: **Game**
- Violence, blood, fear, sexuality, bad language, controlled substances, gambling: **No** to all
- Does the game let users interact or exchange content? **No**
- Does it share the user's location? **No**
- Does it allow purchases of digital goods? **No**

Expected result: Everyone / PEGI 3 / IARC 3+.

## 5. Target audience and content

- Target age groups: **Ages 5 and under** and **Ages 6–8**
- Could the store listing unintentionally appeal to children? Not applicable — it is for children.
- This puts the app in the **Families programme**; accept the Families Policy declaration.

Families policy points, and how Bella meets them:

| Requirement | How the game meets it |
|---|---|
| Content suitable for children | No violence, no fear; Bella is never hurt or distressed |
| No personal data from children | Nothing is collected at all; no network access |
| Ads only from certified networks | There are no ads |
| No manipulative design | No timers that punish, no purchase prompts, no fail states |
| Parental gate on anything adult | Settings / privacy / reset sit behind an arithmetic gate |
| Privacy policy | Provided (step 1) |

**No neutral age screen is needed.** An age screen is only for apps aimed at both children *and*
adults that behave differently for each. Bella is for children only and treats every player as a
child, so asking for a birth year would add data handling for no benefit. (This replaces the
age-screen item in the original plan.)

## 6. Data safety

- Does your app collect or share any of the required user data types? **No**
- Is all of the user data collected by your app encrypted in transit? *Not asked when the answer above is No*
- Do you provide a way for users to request that their data is deleted? *Not asked when the answer above is No*

Result shown on the store page: "No data collected · No data shared".

Why this is accurate: the build has no INTERNET permission, includes no analytics, crash-reporting
or advertising SDK, and the save file (`bella_save.json`) never leaves the device.

## 7. Other declarations

| Form | Answer |
|---|---|
| Government apps | No |
| Financial features | None |
| Health apps | No health features |
| News app | No |
| COVID-19 contact tracing / status | No |
| Advertising ID | **No**, the app does not use an advertising ID |
| Uses AI-generated content? (if asked) | The artwork was made with AI tools during development; the app does not generate content for users |

## 8. App signing

Accept **Play App Signing** (the default). You upload a bundle signed with your own *upload key*
(the keystore made on Unity day); Google holds the real signing key. If the upload keystore is ever
lost, Google can reset it — but keep a backup anyway (see `docs/UNITY_DAY.md`).

## 9. Countries

Closed test: at least **India** (add every country your testers live in).
Production: all countries, or start with India and widen later.

## 10. Requirements Google enforces on the build itself (checked November 2026)

| Rule | Status |
|---|---|
| Target API level 36 (Android 16) for new apps | Met by building with **Unity 6.3 LTS** and target = Automatic |
| 64-bit support | ARM64 is enabled by the setup script |
| 16 KB memory page size | Supported by Unity 6.3 LTS; no third-party native libraries are used |
| App bundle (.aab), not .apk | Menu **Bella → Build Android AAB (Play Store)** |

Re-check the target-API page in Play Console help before uploading — the number rises every August.
