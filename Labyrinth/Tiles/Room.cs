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
        /// True if the room contains a key (a room can only contain one item at a time).
        /// </summary>
        public bool HasKey => LocalInventory.HasItems &&
        LocalInventory.ItemTypes.First() == typeof(Key);
    }



}
