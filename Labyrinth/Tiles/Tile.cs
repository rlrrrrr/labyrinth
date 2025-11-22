using Labyrinth.Items;

namespace Labyrinth.Tiles
{
    /// <summary>
    /// Base class for all tiles in the labyrinth.
    /// </summary>
    public abstract class Tile(ICollectable? item = null)
    {
        /// <summary>
        /// Gets a value indicating whether the tile can be traversed.
        /// </summary>
        public abstract bool IsTraversable { get; }

        /// <summary>
        /// Actually pass through the tile.
        /// </summary>
        /// <exception cref="InvalidOperationException">The tile is not traversable.</exception>
        /// <see cref="IsTraversable"/>
        public Inventory Pass()
        {
            if (!IsTraversable)
            {
                throw new InvalidOperationException("Cannot pass through a non-traversable tile.");
            }
            return LocalInventory;
        }

        protected MyInventory LocalInventory { get; private init; } = new(item);
    }
}
