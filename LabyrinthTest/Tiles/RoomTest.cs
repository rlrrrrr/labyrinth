using Labyrinth.Items;
using Labyrinth.Tiles;

namespace LabyrinthTest.Tiles;

/// <summary>
/// Fake collectable item for testing purposes (not a key).
/// </summary>
internal class FakeItem : ICollectable
{
}

[TestFixture(Description = "Tests for Room HasKey property")]
public class RoomTest
{
    [Test]
    public void EmptyRoomHasNoKey()
    {
        var room = new Room();

        var result = room.HasKey;

        Assert.That(result, Is.False);
    }

    [Test]
    public void RoomWithKeyHasKey()
    {
        var room = new Room(new Key());

        var result = room.HasKey;

        Assert.That(result, Is.True);
    }

    [Test]
    public void RoomWithNonKeyItemHasNoKey()
    {
        var room = new Room(new FakeItem());

        var result = room.HasKey;

        Assert.That(result, Is.False);
    }

    [Test]
    public void RoomAfterSwapWithKeyInventoryHasKey()
    {
        var room = new Room();
        var keyInventory = new MyInventory(new Key());

        room.Pass().SwapItems(keyInventory);

        using var all = Assert.EnterMultipleScope();
        Assert.That(room.HasKey, Is.True);
        Assert.That(keyInventory.HasItems, Is.False);
    }

    [Test]
    public void RoomWithKeyAfterSwapWithEmptyInventoryHasNoKey()
    {
        var room = new Room(new Key());
        var emptyInventory = new MyInventory();

        room.Pass().SwapItems(emptyInventory);

        using var all = Assert.EnterMultipleScope();
        Assert.That(room.HasKey, Is.False);
        Assert.That(emptyInventory.HasKey, Is.True);
    }

    [Test]
    public void RoomWithKeyAfterSwapWithNonKeyInventoryHasNoKey()
    {
        var room = new Room(new Key());
        var fakeItemInventory = new MyInventory(new FakeItem());

        room.Pass().SwapItems(fakeItemInventory);

        using var all = Assert.EnterMultipleScope();
        Assert.That(room.HasKey, Is.False);
        Assert.That(fakeItemInventory.HasKey, Is.True);
    }

    [Test]
    public void RoomWithNonKeyItemAfterSwapWithKeyInventoryHasKey()
    {
        var room = new Room(new FakeItem());
        var keyInventory = new MyInventory(new Key());

        room.Pass().SwapItems(keyInventory);

        using var all = Assert.EnterMultipleScope();
        Assert.That(room.HasKey, Is.True);
        Assert.That(keyInventory.HasKey, Is.False);
    }

    [Test]
    public void RoomIsTraversable()
    {
        var room = new Room();

        var result = room.IsTraversable;

        Assert.That(result, Is.True);
    }

    [Test]
    public void RoomWithKeyIsTraversable()
    {
        var room = new Room(new Key());

        var result = room.IsTraversable;

        Assert.That(result, Is.True);
    }

    [Test]
    public void RoomHasItemsWhenCreatedWithKey()
    {
        var room = new Room(new Key());

        var result = room.Pass().HasItems;

        Assert.That(result, Is.True);
    }

    [Test]
    public void RoomHasNoItemsWhenCreatedEmpty()
    {
        var room = new Room();

        var result = room.Pass().HasItems;

        Assert.That(result, Is.False);
    }
}
