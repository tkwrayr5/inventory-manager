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
        tableLayout.Controls.Add(OK,0,3);
       
        Cancel.DialogResult = DialogResult.Cancel;
        Cancel.Text = "Cancel";
        tableLayout.Controls.Add(Cancel,1,3);

        Label enterValueLabel = new Label();
        enterValueLabel.Text = "New Value";
        Label enterQuantityLabel = new Label();
        enterQuantityLabel.Text = "New Ammount";
        Label enterNotesLabel = new Label();
        enterNotesLabel.Text = "Notes";
        enterNotes.Height = 300;

        tableLayout.Controls.Add(enterValue,0,0);
        tableLayout.Controls.Add(enterQuantity,0,1);
        tableLayout.Controls.Add(enterNotes,0,2);
        tableLayout.Controls.Add(enterValueLabel,1,0);
        tableLayout.Controls.Add(enterQuantityLabel,1,1);
        tableLayout.Controls.Add(enterNotesLabel,1,2);

        this.Controls.Add(tableLayout);
    }
}