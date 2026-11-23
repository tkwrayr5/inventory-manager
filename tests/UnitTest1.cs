using System.Reflection;
using System.Collections.Generic;
using Xunit;
using cs319project;

namespace ProjectTests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var inventory = new List<InventoryItem>
        {
            new InventoryItem {price = 5.00m},
            new InventoryItem {price = 0.50m},
            new InventoryItem {price = 4.50m}
        };
        decimal result = Form1.sumValue(inventory);

        Assert.Equal(10.00m,result);
    }
    [Fact]
    public void Test2()
    {
        var inventory = new List<InventoryItem>
        {
            new InventoryItem {},
            new InventoryItem {price = 0.50m},
            new InventoryItem {price = 4.50m}
        };
        decimal result = Form1.sumValue(inventory);

        Assert.Equal(5.00m,result);
    }
    [Fact]
    public void Test3()
    {
        var inventory = new List<InventoryItem>
        {
            new InventoryItem {},
            new InventoryItem {},
            new InventoryItem {}
        };
        decimal result = Form1.sumValue(inventory);

        Assert.Equal(0,result);
    }
}
