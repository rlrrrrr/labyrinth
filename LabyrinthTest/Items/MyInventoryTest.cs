using Labyrinth.Items;

namespace LabyrinthTest.Items;

/// <summary>
/// Fake collectable item for testing purposes (not a key).
/// </summary>
internal class FakeItem : ICollectable
{
}

[TestFixture(Description = "Tests for MyInventory HasKey property")]
public class MyInventoryTest
{
    [Test]
    public void EmptyInventoryHasNoKey()
    {

        var inventory = new MyInventory();


        var result = inventory.HasKey;


        Assert.That(result, Is.False);
    }

    [Test]
    public void InventoryWithKeyHasKey()
    {

        var inventory = new MyInventory(new Key());


        var result = inventory.HasKey;


        Assert.That(result, Is.True);
    }

    [Test]
    public void InventoryWithNonKeyItemHasNoKey()
    {

        var inventory = new MyInventory(new FakeItem());

        var result = inventory.HasKey;

        Assert.That(result, Is.False);
    }

    [Test]
    public void InventoryWithMultipleKeysHasKey()
    {

        var inventory = new MyInventory(new Key());
        var source = new MyInventory(new Key());

        inventory.MoveItemFrom(source);

        using var all = Assert.EnterMultipleScope();
        Assert.That(inventory.HasKey, Is.True);
        Assert.That(inventory.Items.Count(), Is.EqualTo(2));
    }

    [Test]
    public void InventoryWithKeyAndOtherItemHasKey()
    {

        var inventory = new MyInventory(new Key());
        var source = new MyInventory(new FakeItem());

        inventory.MoveItemFrom(source);

        using var all = Assert.EnterMultipleScope();
        Assert.That(inventory.HasKey, Is.True);
        Assert.That(inventory.Items.Count(), Is.EqualTo(2));
    }

    [Test]
    public void InventoryWithOnlyNonKeyItemsHasNoKey()
    {
        var inventory = new MyInventory(new FakeItem());
        var source = new MyInventory(new FakeItem());

        inventory.MoveItemFrom(source);

        using var all = Assert.EnterMultipleScope();
        Assert.That(inventory.HasKey, Is.False);
        Assert.That(inventory.Items.Count(), Is.EqualTo(2));
    }

    [Test]
    public void InventoryWithKeyAfterMovingToAnotherInventoryHasNoKey()
    {
        // Arrange
        var inventory = new MyInventory(new Key());
        var targetInventory = new MyInventory();

        // Act
        targetInventory.MoveItemFrom(inventory);

        // Assert
        using var all = Assert.EnterMultipleScope();
        Assert.That(inventory.HasKey, Is.False);
        Assert.That(targetInventory.HasKey, Is.True);
    }
}
