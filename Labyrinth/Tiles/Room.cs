using Labyrinth.Items;

namespace Labyrinth.Tiles
{
    /// <summary>
    /// A room in the labyrinth.
    /// </summary>
    /// <remarks>
    /// Initialize a new room, optionally with a collectable item.
    /// </remarks>
    /// <param name="item">Item in the room</param>
    public class Room(ICollectable? item = null) : Tile(item)
    {
        public override bool IsTraversable => true;

        /// <summary>
        /// True if the room contains at least one key.
        /// </summary>
        public bool HasKey => LocalInventory.HasItems && LocalInventory.ItemTypes.Any(type => type == typeof(Key));
    }



}
