using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows.Forms.VisualStyles;

namespace cs319project;

public partial class AddItemForm : Form
{
    TextBox enterItem = new TextBox();
    public String ItemName
    {
        get{ return enterItem.Text;}
    }
    Button OK = new Button();
    Button Cancel = new Button();
    public AddItemForm()
    {
        TableLayoutPanel tableLayout = new TableLayoutPanel();
        tableLayout.ColumnCount = 2;
        tableLayout.RowCount = 2;
        tableLayout.Dock = DockStyle.Fill;
        tableLayout.Controls.Add(enterItem,0,0);
        
        OK.DialogResult = DialogResult.OK;
        OK.Text = "OK";
        tableLayout.Controls.Add(OK,1,0);
       
        Cancel.DialogResult = DialogResult.Cancel;
        Cancel.Text = "Cancel";
        tableLayout.Controls.Add(Cancel,1,1);
        this.Controls.Add(tableLayout);
    }
}