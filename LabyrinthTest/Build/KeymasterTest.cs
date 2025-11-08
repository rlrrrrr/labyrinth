using Labyrinth.Build;
using Labyrinth.Items;
using Labyrinth.Tiles;

namespace LabyrinthTest.Build;

[TestFixture(Description = "Tests for Keymaster door and key room management")]
public class KeymasterTest
{
    [Test]
    public void NewDoorCreatesLockedDoor()
    {
        // Arrange
        using var km = new Keymaster();

        // Act
        var door = km.NewDoor();
        km.NewKeyRoom(); // Apparier pour éviter l'exception Dispose

        // Assert
        Assert.That(door.IsLocked, Is.True);
        Assert.That(door.IsTraversable, Is.False);
    }

    [Test]
    public void NewKeyRoomCreatesEmptyRoom()
    {
        // Arrange
        using var km = new Keymaster();

        // Act
        var room = km.NewKeyRoom();
        km.NewDoor(); // Apparier pour éviter l'exception Dispose

        // Assert
        Assert.That(room, Is.TypeOf<Room>());
        Assert.That(room.Pass().HasItems, Is.True); // La salle contient maintenant la clé
    }

    [Test]
    public void DoorThenKeyRoomPlacesKeyInRoom()
    {
        // Arrange
        using var km = new Keymaster();
        var door = km.NewDoor();

        // Act
        var keyRoom = km.NewKeyRoom();

        // Assert
        using var all = Assert.EnterMultipleScope();
        Assert.That(door.IsLocked, Is.True);
        Assert.That(keyRoom.Pass().HasItems, Is.True);
        Assert.That(keyRoom.Pass().ItemTypes.First(), Is.EqualTo(typeof(Key)));
    }

    [Test]
    public void KeyRoomThenDoorPlacesKeyInRoom()
    {
        // Arrange
        using var km = new Keymaster();
        var keyRoom = km.NewKeyRoom();

        // Act
        var door = km.NewDoor();

        // Assert
        using var all = Assert.EnterMultipleScope();
        Assert.That(door.IsLocked, Is.True);
        Assert.That(keyRoom.Pass().HasItems, Is.True);
        Assert.That(keyRoom.Pass().ItemTypes.First(), Is.EqualTo(typeof(Key)));
    }

    [Test]
    public void MultipleDoorsThenMultipleKeyRooms()
    {
        // Arrange
        using var km = new Keymaster();
        var door1 = km.NewDoor();
        var door2 = km.NewDoor();
        var door3 = km.NewDoor();

        // Act
        var keyRoom1 = km.NewKeyRoom();
        var keyRoom2 = km.NewKeyRoom();
        var keyRoom3 = km.NewKeyRoom();

        // Assert
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
        // Arrange
        using var km = new Keymaster();
        var keyRoom1 = km.NewKeyRoom();
        var keyRoom2 = km.NewKeyRoom();
        var keyRoom3 = km.NewKeyRoom();

        // Act
        var door1 = km.NewDoor();
        var door2 = km.NewDoor();
        var door3 = km.NewDoor();

        // Assert
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
        // Arrange
        using var km = new Keymaster();

        // Act
        var door1 = km.NewDoor();
        var keyRoom1 = km.NewKeyRoom();
        var door2 = km.NewDoor();
        var keyRoom2 = km.NewKeyRoom();
        var door3 = km.NewDoor();
        var keyRoom3 = km.NewKeyRoom();

        // Assert
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
        // Arrange
        using var km = new Keymaster();

        // Act
        var keyRoom1 = km.NewKeyRoom();
        var door1 = km.NewDoor();
        var door2 = km.NewDoor();
        var keyRoom2 = km.NewKeyRoom();
        var keyRoom3 = km.NewKeyRoom();
        var door3 = km.NewDoor();

        // Assert
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
        // Arrange
        using var km = new Keymaster();
        km.NewDoor();
        km.NewKeyRoom();
        km.NewKeyRoom();
        km.NewDoor();

        // Act & Assert
        Assert.DoesNotThrow(() => km.Dispose());
    }

    [Test]
    public void DisposeWithUnmatchedDoorThrowsException()
    {
        // Arrange
        var km = new Keymaster();
        km.NewDoor();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => km.Dispose());
    }

    [Test]
    public void DisposeWithUnmatchedKeyRoomThrowsException()
    {
        // Arrange
        var km = new Keymaster();
        km.NewKeyRoom();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => km.Dispose());
    }

    [Test]
    public void DisposeWithMoreDoorsThrowsException()
    {
        // Arrange
        var km = new Keymaster();
        km.NewDoor();
        km.NewDoor();
        km.NewKeyRoom();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => km.Dispose());
    }

    [Test]
    public void DisposeWithMoreKeyRoomsThrowsException()
    {
        // Arrange
        var km = new Keymaster();
        km.NewDoor();
        km.NewKeyRoom();
        km.NewKeyRoom();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => km.Dispose());
    }

    [Test]
    public void EachDoorHasUniqueKey()
    {
        // Arrange
        using var km = new Keymaster();
        var door1 = km.NewDoor();
        var door2 = km.NewDoor();
        var keyRoom1 = km.NewKeyRoom();
        var keyRoom2 = km.NewKeyRoom();
        var key1 = keyRoom1.Pass();
        var key2 = keyRoom2.Pass();

        // Act
        var opened1 = door1.Open(key1);
        var opened2 = door2.Open(key2);

        // Assert
        Assert.That(opened1, Is.True);
        Assert.That(door1.IsOpened, Is.True);
        Assert.That(opened2, Is.True);
        Assert.That(door2.IsOpened, Is.True);
    }

    [Test]
    public void KeyDoesNotOpenWrongDoor()
    {
        // Arrange
        using var km = new Keymaster();
        var door1 = km.NewDoor();
        var door2 = km.NewDoor();
        var keyRoom1 = km.NewKeyRoom();
        var keyRoom2 = km.NewKeyRoom();
        var key1 = keyRoom1.Pass();

        // Act
        var opened = door2.Open(key1);

        // Assert
        Assert.That(opened, Is.False);
        Assert.That(door2.IsLocked, Is.True);
        Assert.That(key1.HasItems, Is.True);
    }
}
