using ExplainingEveryString.Data.AssetsMetadata;
using ExplainingEveryString.Data.Blueprints;
using Microsoft.Xna.Framework.Content;

namespace ExplainingEveryString.Editor
{
    internal class EditableDisplayingCenter
    {
        internal IEditableDisplayer Blueprint { get; private set; }
        internal IEditableDisplayer RectangleCorner { get; private set; }
        internal IEditableDisplayer SpawnPoint { get; private set; }
        internal IBlueprintsLoader BlueprintsLoader { get; private set; }

        internal EditableDisplayingCenter(ContentManager content, IBlueprintsLoader blueprintsLoader, AssetsMetadata assetsMetadata)
        {
            Blueprint = new BlueprintDisplayer(content, blueprintsLoader, assetsMetadata);
            BlueprintsLoader = blueprintsLoader;
            RectangleCorner = new RectangleCornersDisplayer(content);
            SpawnPoint = new SpawnPointsDisplayer(content);
        }
    }
}
