# Crawl IT

Crawl IT is a dungeon crawler, quiz-style combat game with the purpose of
educating the player about the
[Bachelor in Applied Information Technology](https://binfo.uni.lu)
(short: BINFO) at the University of Luxembourg.

It started as a project for the **Software Engineering Project** course but
from there became a hobby project for us to code away at whenever we feel like.

Currently we plan on getting it to a state that we would personally consider
'finished'.

## Playing

Currently we plan on releasing all of our builds via the github release feature
in the form of **apk**'s only, as we don't have access to an MacOS device to
test on iOS.

We also push our new releases to
[Google Play Store](https://play.google.com/store/apps/details?id=com.crawl.it).

## Development

To get this bad boy running on your machine is almost harder than developing it.
We use MonoGame as engine and develop on Visual Studio
(2019 Enterprise because student licenses are dope 🎉).

The general procedure for getting it to run (without any guarantee) is:

- Install MonoGame 3.8.\*
- Install Visual Studio 2019
  - Note: you might want to install workloads such as:
    - Mobile development with .NET
    - Game development with Unity
      (might as well since you'll want to switch after dealing with MonoGame)
    - .NET Core cross-platform development
- Clone this pretty repo 💅
- Load the pretty solution 💅
- Realise everything is broken 🙂
- Usually Visual Studio will complain about some Android stuff being out of date
  so fix that first (just updating things through Android SDK manager)
- Now everything should absolutely not work, but usually only two issues pop up:
  - If the error mentions something about *MGCB* or *Texture importer* or
  *Map import something*, then this should fix it:
    - [Issue regarding MonoGame Extended](https://github.com/craftworkgames/MonoGame.Extended/issues/495#issuecomment-487315604)
  - If the error mentions something about AndroidGameActivity, or in general some namespaces missing, first make sure that you have the *Android* project selected, as the iOS one is currently not being worked on, if that fails, this should fix it:
    - [Issue regarding MonoGame](https://github.com/craftworkgames/MonoGame.Extended/issues/609#issuecomment-500259738)
- ???
- Profit! Pretend you're a game dev the same way we do 🎉
