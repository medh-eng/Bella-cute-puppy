# Closed-test kit — the 12 testers × 14 days step

Google requires a **personal** developer account (created after 13 Nov 2023) to run a closed test
with **at least 12 testers opted in for 14 days in a row** before it may apply for production.
Since 2026 Google also looks at whether testers really *used* the app. An organisation account
(needs a D-U-N-S number) is exempt — see the open decision in `docs/PLAN.md` §7.

## Timeline

| Day | What happens |
|---|---|
| −3 to 0 | Collect 15–20 Gmail addresses (aim above 12 so drop-outs do not matter) |
| 0 | Upload the .aab to **Testing → Closed testing**, add the testers' emails, send review |
| ~1–3 | Google approves the test build; copy the **opt-in link** and send message 1 |
| 1–14 | Testers play a few minutes on most days; send the two reminders |
| 7 | Send the feedback form (questions below) |
| 15 | **Apply for production access** (answers below) |
| ~17–24 | Google's decision, then production review |

Do not pause the closed-testing track during the 14 days — pausing restarts the count.
Uploading a new build during the test is fine and does not reset anything.

## Who to ask

Clinic staff, family, friends with young children. Every tester needs an **Android phone** and a
**Google (Gmail) account**. Parents with a 4–8 year-old are the most useful testers.

## Message 1 — the invitation (WhatsApp)

```
Hi! I've made a small game for young children — Bella the Cute Puppy 🐶
Before Google lets me publish it, 12 people have to try it for 2 weeks. Could you be one of them?

It takes 1 minute to join:
1) Open this link on your Android phone and tap "Become a tester": [OPT-IN LINK]
2) Then tap "Download it on Google Play" and install
3) Play for a few minutes on most days for the next 14 days — please keep it installed

No ads, nothing to pay, no sign-up inside the game. If you have a little one, let them play!
Please send me the Gmail address you use on your phone so I can add you. Thank you 🙏
```

## Message 2 — day 4 reminder

```
Thank you for testing Bella! 🐶 A small reminder — please open the game for a couple of minutes
when you can this week. She has a new present every day, and try drawing a circle in the
"tricks" game. Anything odd or confusing? Just tell me here.
```

## Message 3 — day 10 reminder + feedback

```
Four more days and Bella can be published 🎉 Please keep the game installed until [DATE] and open
it once or twice more. If you have 2 minutes, these questions would help me a lot: [FORM LINK]
```

## What to ask testers to try

- Feed, bath, sleep and play (the four round buttons)
- Tap Bella's thought bubble
- Trick Training — draw a line down, a circle, a zigzag
- Buy and wear one thing from the wardrobe
- Close the game, come back the next day: is there a present?

## Feedback form (Google Forms — 8 questions)

1. Which phone do you use? *(short answer)*
2. Did the game ever close by itself, freeze or show a strange screen? *(No / Yes — what happened?)*
3. Was anything on the screen cut off, too small or hidden behind the camera notch? *(No / Yes)*
4. Who played? *(Me / A child — age: __)*
5. Could the child play without help? *(Yes straight away / After a little help / No)*
6. Which part was the favourite? *(Feeding / Bath / Tricks / Dress-up / Clinic / Stickers)*
7. Was anything confusing or boring? *(paragraph)*
8. Would you keep it on your phone? *(Yes / Maybe / No)*

Keep the answers — Google asks about them in the next step.

## Applying for production access — the questions Google asks

Write these in your own words using your real results; the drafts below show the shape.

**How did you recruit testers?**
> Friends, family and colleagues, mostly parents of children aged 4–8, invited personally by
> WhatsApp. [N] testers opted in and played across the 14 days.

**How easy was it to recruit them? / Describe engagement**
> Testers were asked to play a few minutes on most days and to try each activity (feeding, bath,
> tricks, wardrobe, clinic, daily present). Feedback was collected with a short form on day 10
> and by direct messages.

**Summarise the feedback you received**
> [Fill in: e.g. "Children understood the picture buttons without help. Two testers reported the
> top icons were close to the camera cut-out on one phone, which was fixed in build 3."]

**What changes did you make based on testing?**
> [List each build uploaded during the test and what it fixed.]

**Who is the intended audience?**
> Children aged about 4–8, playing with or near a parent.

**How do you know the app is ready for production?**
> It ran for 14 days on [N] different Android phones without crashes, every activity was completed
> by testers, and the issues found were fixed and re-tested. The app has no ads, purchases, network
> access or data collection, so there is no back-end to scale.
