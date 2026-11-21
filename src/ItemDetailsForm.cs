using System.ComponentModel;

namespace cs319project;

public partial class ItemDetailsForm : Form
{
    TextBox enterNotes = new TextBox();
    public String ItemNotes
    {
        get{ return enterNotes.Text;}
    }
    TextBox enterQuantity = new TextBox();
    public int ItemQuantity
    {
        get{ if (int.TryParse(enterQuantity.Text, out int result))
            {
              return result;  
            }
            else
            {
                return 0;
            }
        }
    }
    TextBox enterValue = new TextBox();
    public float ItemValue
    {
        get{ if ( float.TryParse(enterValue.Text, out float result))
            {
              return result;  
            }
            else
            {
                return 0.0f;
            }
        }
    }
    Button OK = new Button();
    Button Cancel = new Button();
    public ItemDetailsForm()
    {
        TableLayoutPanel tableLayout = new TableLayoutPanel();
        tableLayout.ColumnCount = 2;
        tableLayout.RowCount = 4;
        tableLayout.Dock = DockStyle.Fill;
        
        OK.DialogResult = DialogResult.OK;
        OK.Text = "OK";
        tableLayout.Controls.Add(OK,3,0);
       
        Cancel.DialogResult = DialogResult.Cancel;
        Cancel.Text = "Cancel";
        tableLayout.Controls.Add(Cancel,3,1);
        this.Controls.Add(tableLayout);
    }
}