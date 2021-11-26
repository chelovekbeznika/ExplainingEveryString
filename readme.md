# What?
This repository belongs to youtube channel ["Explaining every string"](https://www.youtube.com/c/explainingeverystring)

I'm making here videogame on "Monogame framework".
# How to start up?
You need VS2019/2022 with .NET Core SDK 3.1
Also you need to install some dotnet instruments with these commands

`dotnet tool install --global dotnet-mgcb`

`dotnet tool install --global dotnet-mgcb-editor`

WARNING!
There are one thing to do before building solution in VS Studio. Thing is I'm using Monogame.Extended.Content.Pipeline to build tilemaps. And mgcb should find some additional DLLs. Tragedy here - there are lying in nuget cache. And this cache lying in `C:\Users\[User]\.nuget\packages` folder. That means - you should open with text editor file `Assets\Ready\Content.mgcb`, find "References" section and replace "chelovekbeznika" with your username before building it with VS. If you know some workaround which will allow everyone else just built it away without editing content.mgcb contact me, please.

Now, after this you can open your VS studio solution and build solution. It should find, build and copy assets in postbuild of ExplainingEveryString project.

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