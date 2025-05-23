﻿namespace LostBlocks.Models;

public class LegoInventorySet
{
    public int InventoryId { get; set; }
    public string SetNum { get; set; } = null!;
    public int Quantity { get; set; }
    public required LegoInventory Inventory { get; set; }
    public required LegoSet Set { get; set; }
}
