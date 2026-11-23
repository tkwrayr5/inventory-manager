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
    Label labelValue = new Label();
    Label labelQuantity = new Label();
    TextBox textboxNotes = new TextBox();
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
        TableLayoutPanel detailsLayout = new TableLayoutPanel();
        Label labelV = new Label();
        labelV.Text = "Value";
        Label labelQ = new Label();
        labelQ.Text = "Quantity";

        tableLayout.ColumnCount = 2;
        tableLayout.RowCount = 4;
        detailsLayout.ColumnCount = 2;
        detailsLayout.RowCount = 2;
        tableLayout.Dock = DockStyle.Fill;
        textboxNotes.ReadOnly = true;
        textboxNotes.Multiline = true;
        textboxNotes.Width = 360;
        textboxNotes.Height = 400;
        tableLayout.Controls.Add(detailsLayout,1,1);
        detailsLayout.Controls.Add(labelV,0,0);
        detailsLayout.Controls.Add(labelQ,1,0);
        detailsLayout.Controls.Add(labelQuantity,1,1);
        detailsLayout.Controls.Add(labelValue,0,1);
        detailsLayout.Controls.Add(labelQuantity,1,1);
        tableLayout.Controls.Add(textboxNotes,1,0);
        
        inventoryList.NodeMouseClick += treeClick;
        inventoryList.Size = new Size(360, 400);
        UpdateInventoryList();
        tableLayout.Controls.Add(inventoryList, 0, 0);

        Button addItemButton = new Button();
        addItemButton.Text = "Add Item";
        addItemButton.Click += addClick;
        tableLayout.Controls.Add(addItemButton, 0, 2);

        Button removeItemButton = new Button();
        removeItemButton.Text = "Remove Item";
        removeItemButton.Click += removeClick;
        tableLayout.Controls.Add(removeItemButton, 1, 2);

        Button editItemButton = new Button();
        editItemButton.Text = "Edit Item";
        editItemButton.Click += valueClick;
        tableLayout.Controls.Add(editItemButton, 0, 3);

        Button valueButton = new Button();
        valueButton.Text = "Total Value";
        valueButton.Click += valueClick;
        tableLayout.Controls.Add(valueButton, 1, 3);
        

        this.Controls.Add(tableLayout);

        ExportCsv(appFolder, InventoryTable);

    }
    public static void ExportCsv(string path, List<InventoryItem> itemTable)
    {

        using (var writer = CsvWriter.ToFile(Path.Combine(path, "inventory.csv")))
        {
            writer.WriteHeader<InventoryItem>();
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
       labelValue.Text = selectedItem.price.ToString();
       labelQuantity.Text = selectedItem.quantity.ToString();
       textboxNotes.Text = selectedItem.notes;
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
            newItem.price = 0.00m;
            newItem.notes = "Notes go here"; 
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
        selectedItem = null;
        removeItemWindow.Dispose();
    }
    private void editClick(object sender, EventArgs e)
    {
        ItemDetailsForm editItemWindow = new ItemDetailsForm();
        if (editItemWindow.ShowDialog() == DialogResult.OK)
        {
            if(selectedItem != null)
            {
                selectedItem.quantity = editItemWindow.ItemQuantity;
                selectedItem.price = (decimal)editItemWindow.ItemValue;
                selectedItem.notes = editItemWindow.ItemNotes;
            }
            editItemWindow.Dispose();
        }
    }
    private void valueClick(object sender, EventArgs e)
    {
        ValueForm valueWindow = new ValueForm();
        sumValue(InventoryTable);
        if (valueWindow.ShowDialog() == DialogResult.OK)
        {
            
        }
        valueWindow.Dispose();
    }

    public static decimal sumValue(List<InventoryItem> inventorylist)
    {
        decimal sum = 0;
        foreach (var item in inventorylist)
        {
            sum = sum + item.price;
        }
        return sum;
    }

    public void UpdateInventoryList()
    {
        inventoryList.BeginUpdate();
        inventoryList.Nodes.Clear();
        lookUpTable.Clear();
        //inventoryList.Nodes.Add("Earth");
        PopulateInventoryList();
        OrderInventoryList();
        inventoryList.EndUpdate();
    }
    public void PopulateInventoryList()
    {
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
               
    }
    public void OrderInventoryList()
    {
                foreach (var item in InventoryTable)
        {
            try
            {
                if (item.locationId >= 0)
                {
                    InventoryItem parentItem = this.lookUpTable.GetValueOrDefault(item.locationId);
                    TreeNode parentNode = parentItem.node;
                    parentNode.Nodes.Insert(parentNode.Nodes.Count, (TreeNode)item.node.Clone());
                    item.node.Remove();
                    item.node = parentNode.Nodes[parentNode.Nodes.Count - 1];
                }
            }
            catch
            {
                Console.WriteLine("Not a child node, or missing parent");
            }
        }
    }
}

[CsvRecord]
public partial class InventoryItem
{
    [CsvColumn("ID")]
    public int Id { get; set; }
    [CsvColumn("Name")]
    public string name { get; set; } = "";
    [CsvColumn("Ammount")]
    public int quantity { get; set; }
    [CsvColumn("Value")]
    public decimal price { get; set; }
    [CsvColumn("Notes")]
    public string notes { get; set; } = "";
    [CsvColumn("Location")]
    public int locationId { get; set; }
    [CsvIgnore]
    public TreeNode? node { get; set; }
}