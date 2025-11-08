using System.Diagnostics.CodeAnalysis;

namespace Labyrinth.Items
{
    /// <summary>
    /// Inventory of collectable items for rooms and players.
    /// </summary>
    /// <param name="item">Optional initial item in the inventory.</param>
    public abstract class Inventory(ICollectable? item = null)
    {
        /// <summary>
        /// True if the inventory has items, false otherwise.
        /// </summary>
        [MemberNotNullWhen(true, nameof(_items))]
        public bool HasItems => _items != null && _items.Count > 0;

        /// <summary>
        /// Gets the types of the items in the inventory.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the inventory has no items (check with <see cref="HasItems"/>).</exception>
        public IEnumerable<Type> ItemTypes
        {
            get
            {
                if (!HasItems)
                {
                    throw new InvalidOperationException("No items in the inventory");
                }
                return _items.Select(i => i.GetType());
            }
        }

        /// <summary>
        /// Moves the nth item from another inventory to this one.
        /// </summary>
        /// <param name="from">The inventory from which the item is taken. The item is removed from this inventory.</param>
        /// <param name="nth">The zero-based index of the item to take (default is 0 for the first item).</param>
        /// <exception cref="InvalidOperationException">Thrown if the source inventory has no items at the specified index.</exception>
        public void MoveItemFrom(Inventory from, int nth = 0)
        {
            if (!from.HasItems || nth >= from._items.Count)
            {
                throw new InvalidOperationException("No item to take from the source inventory at the specified index");
            }

            var item = from._items[nth];
            from._items.RemoveAt(nth);

            _items ??= new List<ICollectable>();
            _items.Add(item);
        }

        protected List<ICollectable>? _items = item != null ? new List<ICollectable> { item } : null;
    }
}
