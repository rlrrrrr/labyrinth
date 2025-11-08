using Labyrinth.Build;
using Labyrinth.Tiles;

namespace LabyrinthTest.Build;

[TestFixture(Description = "Tests for Keymaster door and key room management")]
public class KeymasterTest
{
    [Test]
    public void NewDoorCreatesLockedDoor()
    {
        using var km = new Keymaster();

        var door = km.NewDoor();

        Assert.That(door.IsLocked, Is.True);
        Assert.That(door.IsTraversable, Is.False);
    }

    [Test]
    public void NewKeyRoomCreatesEmptyRoom()
    {
        using var km = new Keymaster();

        var room = km.NewKeyRoom();

        Assert.That(room, Is.TypeOf<Room>());
        Assert.That(room.Pass().HasItems, Is.False);
    }

    [Test]
    public void DoorThenKeyRoomPlacesKeyInRoom()
    {
        using var km = new Keymaster();
        var door = km.NewDoor();

        var keyRoom = km.NewKeyRoom();

        using var all = Assert.EnterMultipleScope();
        Assert.That(door.IsLocked, Is.True);
        Assert.That(keyRoom.Pass().HasItems, Is.True);
        Assert.That(keyRoom.Pass().ItemTypes.First(), Is.EqualTo(typeof(Key)));
    }

    [Test]
    public void KeyRoomThenDoorPlacesKeyInRoom()
    {
        using var km = new Keymaster();
        var keyRoom = km.NewKeyRoom();

        var door = km.NewDoor();

        using var all = Assert.EnterMultipleScope();
        Assert.That(door.IsLocked, Is.True);
        Assert.That(keyRoom.Pass().HasItems, Is.True);
        Assert.That(keyRoom.Pass().ItemTypes.First(), Is.EqualTo(typeof(Key)));
    }

    [Test]
    public void MultipleDoorsThenMultipleKeyRooms()
    {
        using var km = new Keymaster();
        var door1 = km.NewDoor();
        var door2 = km.NewDoor();
        var door3 = km.NewDoor();


        var keyRoom1 = km.NewKeyRoom();
        var keyRoom2 = km.NewKeyRoom();
        var keyRoom3 = km.NewKeyRoom();

        using var all = Assert.EnterMultipleScope();
        Assert.That(door1.IsLocked, Is.True);
        Assert.That(door2.IsLocked, Is.True);
        Assert.That(door3.IsLocked, Is.True);
        Assert.That(keyRoom1.Pass().HasItems, Is.True);
        Assert.That(keyRoom2.Pass().HasItems, Is.True);
        Assert.That(keyRoom3.Pass().HasItems, Is.True);
    }

    [Test]
    public void MultipleKeyRoomsThenMultipleDoors()
    {
        using var km = new Keymaster();
        var keyRoom1 = km.NewKeyRoom();
        var keyRoom2 = km.NewKeyRoom();
        var keyRoom3 = km.NewKeyRoom();

        var door1 = km.NewDoor();
        var door2 = km.NewDoor();
        var door3 = km.NewDoor();

        using var all = Assert.EnterMultipleScope();
        Assert.That(door1.IsLocked, Is.True);
        Assert.That(door2.IsLocked, Is.True);
        Assert.That(door3.IsLocked, Is.True);
        Assert.That(keyRoom1.Pass().HasItems, Is.True);
        Assert.That(keyRoom2.Pass().HasItems, Is.True);
        Assert.That(keyRoom3.Pass().HasItems, Is.True);
    }

    [Test]
    public void InterleavedDoorsAndKeyRooms()
    {
        using var km = new Keymaster();

        var door1 = km.NewDoor();
        var keyRoom1 = km.NewKeyRoom();
        var door2 = km.NewDoor();
        var keyRoom2 = km.NewKeyRoom();
        var door3 = km.NewDoor();
        var keyRoom3 = km.NewKeyRoom();

        using var all = Assert.EnterMultipleScope();
        Assert.That(door1.IsLocked, Is.True);
        Assert.That(door2.IsLocked, Is.True);
        Assert.That(door3.IsLocked, Is.True);
        Assert.That(keyRoom1.Pass().HasItems, Is.True);
        Assert.That(keyRoom2.Pass().HasItems, Is.True);
        Assert.That(keyRoom3.Pass().HasItems, Is.True);
    }

    [Test]
    public void MixedOrderDoorsAndKeyRooms()
    {
        using var km = new Keymaster();

        var keyRoom1 = km.NewKeyRoom();
        var door1 = km.NewDoor();
        var door2 = km.NewDoor();
        var keyRoom2 = km.NewKeyRoom();
        var keyRoom3 = km.NewKeyRoom();
        var door3 = km.NewDoor();

        using var all = Assert.EnterMultipleScope();
        Assert.That(door1.IsLocked, Is.True);
        Assert.That(door2.IsLocked, Is.True);
        Assert.That(door3.IsLocked, Is.True);
        Assert.That(keyRoom1.Pass().HasItems, Is.True);
        Assert.That(keyRoom2.Pass().HasItems, Is.True);
        Assert.That(keyRoom3.Pass().HasItems, Is.True);
    }

    [Test]
    public void DisposeWithMatchedDoorsAndKeysSucceeds()
    {
        using var km = new Keymaster();
        km.NewDoor();
        km.NewKeyRoom();
        km.NewKeyRoom();
        km.NewDoor();

        Assert.DoesNotThrow(() => km.Dispose());
    }

    [Test]
    public void DisposeWithUnmatchedDoorThrowsException()
    {
        var km = new Keymaster();
        km.NewDoor();

        Assert.Throws<InvalidOperationException>(() => km.Dispose());
    }

    [Test]
    public void DisposeWithUnmatchedKeyRoomThrowsException()
    {
        var km = new Keymaster();
        km.NewKeyRoom();

        Assert.Throws<InvalidOperationException>(() => km.Dispose());
    }

    [Test]
    public void DisposeWithMoreDoorsThrowsException()
    {
        var km = new Keymaster();
        km.NewDoor();
        km.NewDoor();
        km.NewKeyRoom();

        Assert.Throws<InvalidOperationException>(() => km.Dispose());
    }

    [Test]
    public void DisposeWithMoreKeyRoomsThrowsException()
    {
        var km = new Keymaster();
        km.NewDoor();
        km.NewKeyRoom();
        km.NewKeyRoom();

        Assert.Throws<InvalidOperationException>(() => km.Dispose());
    }

    [Test]
    public void EachDoorHasUniqueKey()
    {
        using var km = new Keymaster();
        var door1 = km.NewDoor();
        var door2 = km.NewDoor();
        var keyRoom1 = km.NewKeyRoom();
        var keyRoom2 = km.NewKeyRoom();
        var key1 = keyRoom1.Pass();
        var key2 = keyRoom2.Pass();

        var opened1 = door1.Open(key1);
        var opened2 = door2.Open(key2);

        Assert.That(opened1, Is.True);
        Assert.That(door1.IsOpened, Is.True);
        Assert.That(opened2, Is.True);
        Assert.That(door2.IsOpened, Is.True);
    }

    [Test]
    public void KeyDoesNotOpenWrongDoor()
    {
        using var km = new Keymaster();
        var door1 = km.NewDoor();
        var door2 = km.NewDoor();
        var keyRoom1 = km.NewKeyRoom();
        var keyRoom2 = km.NewKeyRoom();
        var key1 = keyRoom1.Pass();

        var opened = door2.Open(key1);

        Assert.That(opened, Is.False);
        Assert.That(door2.IsLocked, Is.True);
        Assert.That(key1.HasItems, Is.True);
    }
}
