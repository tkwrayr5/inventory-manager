using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows.Forms.VisualStyles;
using CsvHelper;


namespace cs319project;


public partial class Form1 : Form
{
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
        tableLayout.RowCount = 2;
        tableLayout.Dock = DockStyle.Fill;
        tableLayout.Controls.Add(new DataGridView(), 1, 0);

        TreeView inventoryList = new TreeView();
        inventoryList.BeginUpdate();
        inventoryList.Nodes.Add("Earth");
        foreach (var item in InventoryTable)
        {
            try 
            {
            inventoryList.Nodes[item.locationId].Nodes.Add(item.name);
            }
            catch
            {
            inventoryList.Nodes.Add(item.name);
            }
        }
        inventoryList.EndUpdate();
        tableLayout.Controls.Add(inventoryList, 0, 0);
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
} 