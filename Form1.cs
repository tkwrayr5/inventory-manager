using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows.Forms.VisualStyles;
using CsvHelper;


namespace cs319project;

public partial class Form1 : Form
{
    private Dictionary<int, InventoryItem> lookUpTable { get; set; } = new Dictionary<int, InventoryItem> { };
    List<InventoryItem> InventoryTable = new List<InventoryItem>();
    static string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    string appFolder = Path.Combine(appData, "InventoryManager");
    TreeView inventoryList = new TreeView();
    InventoryItem selectedItem = new InventoryItem();
    public Form1()
    {
        Directory.CreateDirectory(appFolder);
        try
        {
            using (var reader = CsvReader.FromFile(Path.Combine(appFolder, "inventory.csv")))
            {
                ImportCsvRecord(reader, InventoryTable);
            }
        }
        catch
        {
            ExportCsv(appFolder, InventoryTable);
        }
        InitializeComponent();
        TableLayoutPanel tableLayout = new TableLayoutPanel();
        tableLayout.ColumnCount = 2;
        tableLayout.RowCount = 3;
        tableLayout.Dock = DockStyle.Fill;

        
        inventoryList.NodeMouseClick += treeClick;
        UpdateInventoryList();
        tableLayout.Controls.Add(inventoryList, 0, 0);
        Button addItemButton = new Button();
        addItemButton.Text = "Add";
        addItemButton.Click += addClick;
        tableLayout.Controls.Add(addItemButton, 0, 2);

        Button removeItemButton = new Button();
        removeItemButton.Text = "Remove Item";
        removeItemButton.Click += removeClick;
        tableLayout.Controls.Add(removeItemButton, 1, 2);

        this.Controls.Add(tableLayout);

        ExportCsv(appFolder, InventoryTable);

    }
    public static void ExportCsv(string path, List<InventoryItem> itemTable)
    {

        using (var writer = CsvWriter.ToFile(Path.Combine(path, "inventory.csv")))
        {
            writer.WriteHeader("ID", "Name", "Ammount", "Location");
            foreach (var item in itemTable)
            {
                writer.WriteRecord(item);
            }

        }
    }
    public static void ImportCsvRecord(CsvReader reader, List<InventoryItem> csv)
    {
        while (reader.Read())
        {
            csv.Add(reader.GetRecord<InventoryItem>());
        }
    }
    private void treeClick(object sender, TreeNodeMouseClickEventArgs e)
    {
       selectedItem = (InventoryItem)e.Node.Tag;
    }
    private void addClick(object sender, EventArgs e)
    {
        AddItemForm addItemWindow = new AddItemForm();
        if (addItemWindow.ShowDialog() == DialogResult.OK)
        {
            Console.WriteLine(addItemWindow.ItemName);
            InventoryItem newItem = new InventoryItem();
            for (int i = 1; i < InventoryTable.Count + 2; i++)
            {
                if (!InventoryTable.Any(item => item.Id == i))
                {
                    newItem.Id = i;
                }
            }
            newItem.name = addItemWindow.ItemName; 
            newItem.quantity = 1;
            if (selectedItem != null)
            {
               newItem.locationId = selectedItem.Id; 
            }
            else
            {
               newItem.locationId = 0; 
            }            
            InventoryTable.Add(newItem);
            UpdateInventoryList();
            ExportCsv(appFolder, InventoryTable);
        }
        addItemWindow.Dispose();
    }
    private void removeClick(object sender, EventArgs e)
    {
        RemoveItemForm removeItemWindow = new RemoveItemForm();
        if (removeItemWindow.ShowDialog() == DialogResult.OK)
        {
            if (selectedItem != null)
            {
            inventoryList.Nodes.Remove(selectedItem.node);
            InventoryTable.RemoveAll(item => item.Id == selectedItem.Id);
            lookUpTable.Remove(selectedItem.Id);
            UpdateInventoryList();
            ExportCsv(appFolder, InventoryTable);
            }
        }
        removeItemWindow.Dispose();
    }
    public void UpdateInventoryList()
    {
        inventoryList.BeginUpdate();
        inventoryList.Nodes.Clear();
        lookUpTable.Clear();
        inventoryList.Nodes.Add("Earth");
        foreach (var item in InventoryTable)
        {
            try
            {
                TreeNode newNode = new TreeNode(item.name);
                newNode.Tag = item;
                item.node = inventoryList.Nodes.Add(newNode.Text);
                item.node.Tag = item;
                // Console.WriteLine(item.node.ToString());
                this.lookUpTable.Add(item.Id, item);
            }
            catch
            {
                Console.WriteLine("Failed to add node");
            }
        }
        foreach (var item in InventoryTable)
        {
            try
            {
                if (item.locationId > 0)
                {
                    InventoryItem parentItem = this.lookUpTable.GetValueOrDefault(item.locationId);
                    TreeNode parentNode = parentItem.node;
                    parentNode.Nodes.Insert(parentNode.Nodes.Count, (TreeNode)item.node.Clone());
                    item.node.Remove();
                    item.node = parentNode.Nodes[parentNode.Nodes.Count - 1];

                    // Console.WriteLine(item.node.ToString());
                    // this.lookUpTable.Add(item.locationId,item);

                }
            }
            catch
            {
                Console.WriteLine("Not a child node, or missing parent");
            }
        }
        inventoryList.EndUpdate();
    }
}

[CsvRecord]
public partial class InventoryItem
{
    [CsvColumn("ID")]
    public int Id { get; set; }
    [CsvColumn("Name")]
    public String name { get; set; } = "";
    [CsvColumn("Ammount")]
    public int quantity { get; set; }
    [CsvColumn("Location")]
    public int locationId { get; set; }
    [CsvIgnore]
    public TreeNode? node { get; set; }
}