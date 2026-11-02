Inventory Manager for logging and tracking personal inventory. Written for a class.
By Thomas Wray

This project is built as a Windows Forms application. It is built with dotnet framework 10.0 and needs the dotnet SDK
This is installed through your vscode or visual studio package manager.

It also uses CsvHelper version 34.0.0
The bash command to install that is: dotnet add package CsvHelper

It requires no configuration at this time, but it does expect to be able to write to your appdata folder so make sure it has permissions to do so.
It should create the file it needs the first time it is run in Appdata\Roaming\InventoryManager\inventory.csv

To run the program after compilation simply find the cs319project.exe in the bin folder and run that.


How to Use

The program will open up a form window with a small inventory window and an add and remove button.

To add an item click the add button, enter a name and click OK and the item should be added to the inventory window.
To remove an item click it in the inventory window then click remove, another window will open up, click OK to confirm the removal.
To put an item inside of another item click the item you want to put the item into and then click add and follow the same actions as above.


Known Limitations

Currently there is no way to change what item an item is inside of, it must be deleted and recreated.
There can be some weirdness if you delete items that have other items in them.
Currently there is very little that the program tracks other than the names of objects and where they are, I hope to add functionality for quantities of items and descriptions and metrics for the items.