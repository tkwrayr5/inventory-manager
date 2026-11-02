using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows.Forms.VisualStyles;
using CsvHelper;


namespace cs319project;

public partial class Form1 : Form
{
    private Dictionary<int, InventoryItem> lookUpTable{get; set;} = new Dictionary<int, InventoryItem>{};
    public Form1()
    {
        
        List<InventoryItem> InventoryTable = new List<InventoryItem>();
        string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string appFolder = Path.Combine(appData, "InventoryManager");
        Directory.CreateDirectory(appFolder);
        try{
        using (var reader = CsvReader.FromFile(Path.Combine(appFolder, "inventory.csv")))
        {
        ImportCsvRecord(reader,InventoryTable);
        }
        }
        catch
        {
         ExportCsv(appFolder,InventoryTable);
        }
        InitializeComponent();
        TableLayoutPanel tableLayout = new TableLayoutPanel();
        tableLayout.ColumnCount = 2;
        tableLayout.RowCount = 3;
        tableLayout.Dock = DockStyle.Fill;

        TreeView inventoryList = new TreeView();
        inventoryList.NodeMouseClick += treeClick;
        inventoryList.BeginUpdate();
        inventoryList.Nodes.Add("Earth");
        foreach (var item in InventoryTable)
        {
            try 
            {
            item.node = inventoryList.Nodes.Add(item.name);
           // Console.WriteLine(item.node.ToString());
            this.lookUpTable.Add(item.Id,item);
            

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
                if (item.locationId > 0){
            InventoryItem parentItem = this.lookUpTable.GetValueOrDefault(item.locationId);
            TreeNode parentNode = parentItem.node;
            parentNode.Nodes.Insert(parentNode.Nodes.Count, (TreeNode)item.node.Clone());
            item.node.Remove();
            item.node = parentNode.Nodes[parentNode.Nodes.Count -1];
            
           // Console.WriteLine(item.node.ToString());
           // this.lookUpTable.Add(item.locationId,item);

                }
            }
            catch
            {
            Console.WriteLine("Not a child node");
            }
        }
        inventoryList.EndUpdate();
        tableLayout.Controls.Add(inventoryList, 0, 0);
        Button addItemButton = new Button();
        addItemButton.DialogResult = DialogResult.OK;
        addItemButton.Text = "Add";
        tableLayout.Controls.Add(addItemButton, 0, 2);
        Button removeItemButton = new Button();
        removeItemButton.DialogResult = DialogResult.OK;
        removeItemButton.Text = "Remove Item";
        tableLayout.Controls.Add(removeItemButton, 1, 2);
        this.Controls.Add(tableLayout);

        ExportCsv(appFolder, InventoryTable);

    }
    public static void ExportCsv(string path, List<InventoryItem> itemTable)
    {
        
        using (var writer = CsvWriter.ToFile(Path.Combine(path, "inventory.csv")))
        {
            writer.WriteHeader("ID","Name","Ammount","Location");
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
      //  Console.WriteLine(e.Node.Parent.Index);  
    }
}

[CsvRecord]
public partial class InventoryItem
{
    [CsvColumn("ID")]
    public int Id {get; set;}
    [CsvColumn("Name")]
    public String name {get; set;} = "";
    [CsvColumn("Ammount")]
    public int quantity {get; set;}
    [CsvColumn("Location")]
    public int locationId {get; set;}
    [CsvIgnore]
    public TreeNode? node {get; set;}
} 