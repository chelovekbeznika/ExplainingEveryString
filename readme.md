# What?
This repository belongs to youtube channel ["Explaining every string"](https://www.youtube.com/c/explainingeverystring)

I'm making here videogame on "Monogame framework".
# How to start up?
You need VS2019/2022 with .NET Core SDK 3.1
Also you need to install some dotnet instruments with these commands

`dotnet tool install --global dotnet-mgcb`

`dotnet tool install --global dotnet-mgcb-editor`

Now, after this you can open your VS studio solution and build solution. It should find, build and copy assets in postbuild of ExplainingEveryString project.

WARNING!
If you updating Monogame.Extended.Content.Pipeline you should find pipeline extensions DLLs in folder `C:\Users\[User]\.nuget\packages\monogame.extended.content.pipeline\[Version]\tools\` and copy them to "PipelineExtension" folder in solution. mgcb-editor should be able to find them and this is easiest way to ensure it.

# How to add/edit sprite
1. Draw it in aserprite and save in Raw/Sprites in appropriate subfolder or edit existing folder
2. Export it as spritesheet in Ready/Sprites in appropriate subfolder
3. Add it to mgcb project in Monogame Pipeline tool using mgcb-editor
4. If sprite animated - add info about frames amount in assets_metadata.dat
# How to add/edit other assets
1. Create and save "source" or "preexported" version in "Raw" folder if necessary.
2. Export/save ready for adding to mgcb project assets in "Ready" folder.
3. Add it to mgcb project.
# History info
Some time I stored assets [here](https://github.com/chelovekbeznika/ExplainingEveryStringAssets). Now this repository in archive mode.