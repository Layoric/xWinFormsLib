xWinFormsLib
============

##Overview of changes
This project adds a XML markup to generate the xWinForms controls. I've added an schema file to give intellisense within Visual studio for valid markup. This should make designing Winform like interfaces a bit nicer and simpler. 

For eg, the following markup

```xml
<?xml version="1.0" encoding="utf-8" ?>
<FormCollection xmlns="http://tempuri.org/Markup.xsd">
  <Form id="myForm" size="600,400" position="30,30" title="Simple Form" style="Sizable">
    <Menu id="menu1">
      <SubMenu id="submenu1">File
        <MenuItem id="menuItem1">
          New
        </MenuItem>
        <MenuItem id="menuItem2">
          Close
        </MenuItem>
      </SubMenu>
      <SubMenu id="submenu2">
        View
        <SubMenu id="submenu3">
          Options
          <MenuItem id="menuItem11">
          Enable
        </MenuItem>
        </SubMenu>
      </SubMenu>
    </Menu>
    <Label id="mylabel" position="15,135" width="70" backcolor="White" forecolor="Black" alignment="Left">Label #1</Label>

    <Textbox id="textbox1" position="310,50" width="130" height="80" backcolor="White" forecolor="Black">This is a test</Textbox>
    <Button id="button1" position="15,50" width="130" backcolor="White" forecolor="Black">Button1</Button>
    <Button id="btRemove" position="15,320" width="60" backcolor="White" forecolor="Black">Remove Listbox Item</Button>
    <Button id="btAdd" position="440,100" width="100" backcolor="White" forecolor="Black">Add to Listbox</Button>
  </Form>
</FormCollection>
```

would generate

![example xWinForms](https://docs.google.com/folder/d/0B89V3E7BmHRdYWQ2ZjQ0YjMtNzU5ZC00MmJmLWExMmItNzg1NGQ5YTFjMDFi/edit)

## In Progress

Not all controls are yet supported by the markup, and also events have yet to be supported but will be by providing the 'Form' a namespace and assembly of where the methods you want to invoke live

## Performance

It's worth noting that using the markup does add some intialization time compared with writing the C#, but once instantiated, performance is very similar
