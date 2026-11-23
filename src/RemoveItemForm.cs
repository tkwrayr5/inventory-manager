using System.ComponentModel;

namespace cs319project;

public partial class RemoveItemForm : Form
{
    Button OK = new Button();
    Button Cancel = new Button();
    public RemoveItemForm()
    {
        TableLayoutPanel tableLayout = new TableLayoutPanel();
        tableLayout.ColumnCount = 2;
        tableLayout.RowCount = 2;
        tableLayout.Dock = DockStyle.Fill;
        
        OK.DialogResult = DialogResult.OK;
        OK.Text = "OK";
        tableLayout.Controls.Add(OK,1,0);
       
        Cancel.DialogResult = DialogResult.Cancel;
        Cancel.Text = "Cancel";
        tableLayout.Controls.Add(Cancel,1,1);
        this.Controls.Add(tableLayout);
    }
}