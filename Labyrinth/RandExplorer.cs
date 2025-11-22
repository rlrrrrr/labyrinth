using Labyrinth.Crawl;
using Labyrinth.Items;
using Labyrinth.Sys;
using Labyrinth.Tiles;

namespace Labyrinth
{
    public class RandExplorer(ICrawler crawler, IEnumRandomizer<RandExplorer.Actions> rnd)
    {
        private readonly ICrawler _crawler = crawler;
        private readonly IEnumRandomizer<Actions> _rnd = rnd;

        public enum Actions
        {
            TurnLeft,
            Walk
        }

        public int GetOut(int n)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(n, 0, "n must be strictly positive");
            MyInventory bag = new();

            for (; n > 0 && _crawler.FacingTile is not Outside; n--)
            {
                EventHandler<CrawlingEventArgs>? changeEvent;

                if (_crawler.FacingTile is Door door && door.IsLocked
                    && bag.HasKey)
                {
                    TryOpenDoorWithAllKeys(door, bag);
                }

                if (_crawler.FacingTile.IsTraversable
                    && _rnd.Next() == Actions.Walk)
                {
                    var roomInventory = _crawler.Walk();
                    while (roomInventory.HasItems)
                    {
                        bag.MoveItemFrom(roomInventory);
                    }
                    changeEvent = PositionChanged;
                }
                else
                {
                    _crawler.Direction.TurnLeft();
                    changeEvent = DirectionChanged;
                }
                changeEvent?.Invoke(this, new CrawlingEventArgs(_crawler));
            }
            return n;
        }

        private void TryOpenDoorWithAllKeys(Door door, MyInventory bag)
        {
            var keyCount = bag.Items.Count(item => item is Key);

            for (int attempt = 0; attempt < keyCount && door.IsLocked; attempt++)
            {
                var keyIndex = bag.Items
                    .Select((item, index) => new { item, index })
                    .Where(x => x.item is Key)
                    .First()
                    .index;

                var tempInventory = new MyInventory();
                tempInventory.MoveItemFrom(bag, keyIndex);

                if (!door.Open(tempInventory))
                {
                    bag.MoveItemFrom(tempInventory);
                }
            }
        }

        public event EventHandler<CrawlingEventArgs>? PositionChanged;

        public event EventHandler<CrawlingEventArgs>? DirectionChanged;
    }

}
