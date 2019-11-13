# Usage

This document explains the main points in using WebSharper.UI.JQueryUI
and how it differs from its corresponding JavaScript library.

Notice that this extension differs from WebSharper.JQueryUI, the latest
targeting the deprecated WebSharper.Html extension, as this one targets
WebSharper.UI.

The original library, jQuery UI 1.12.1, works by doing a jQuery
selection on an element and calling the relevant widget constructor as
an instance member, for example:

```javascript
$("#datepicker").datepicker();
```

In the jQuery UI binding for WebSharper™ widgets like `Datepicker` are
represented as types.  To create the corresponding `Datepicker` widget
with WebSharper™ you write:

```fsharp
let datePicker = Datepicker.New()
```

The `datePicker` object now contains the available methods.  For
example, to call the `getDate` methods you use:

```fsharp
let date = datePicker.GetDate()
```

## Resource Dependencies

Using the JQuery UI WebSharper™ extension within a WebControl will automatically
include the required JavaScript and Css resources.

You may override the location of the fetched resources by modifying the `app settings`
in in your `web.config` file.

There are two configurable values: One for the JavaScript file and another one for the
Css file. Here is an example of how to override both settings.

```xml
<appSettings>
  <add key="WebSharper.JQueryUI.Dependencies+JQueryUIJs" value="/scripts/jquery-ui.js" />
  <add key="WebSharper.JQueryUI.Dependencies+JQueryUICss" value="/css/jquery-ui.css" />
</appSettings>
```

If you want to serve the resource files locally you need to make sure to you use the same
naming conventions as used by the online repository.


## Constructors

Constructor functions are provided for all `Widgets` and `Effects`.
There are typically a few options for constructing Widgets depending
on the desired configuration.

In the original jQuery UI library configuration settings are usually
passed as a JavaScript object.  For example, a calendar may be
initialized in a disabled state by passing a configuration setting to
the constructor function:

```javascript
$("#datepicker").datepicker({{ disabled: true }});
```

In WebSharper™ configuration settings for any widget are represented
by a corresponding configuration type and may be passed to the
constructors as parameters:

```fsharp
let conf = DatepickerConfiguration(Disabled = true)
let datePicker = Datepicker.New(conf)
```

## Methods

jQuery UI methods are represented as member methods on the objects returned when
creating a widget.  Typically the widget and effect objects contain
some common methods like `destroy`, `enable`, `disable`, and `option`.

For example, the `Datepicker` widget has a method for setting the
date. The JavaScript snippet for this may look like:

```javascript
$("#datepicker").datepicker("setDate", "03/14/2010");
```

The WebSharper™ bindings contain (possibly overloaded) methods with
corresponding names:

```fsharp
datePicker.SetDate(new Date(2010,03,14))
```


## Warning about Initialization

The fundamental difference between original jQuery UI JavaScript
widgets and their WebSharper™ counterparts is that in JavaScript the
constructor methods are typically called on elements already attached
to the DOM tree.  For example, in order for the following code to
work:

```javascript
$(selector).datepicker();
```

the selector needs to refer to an element already attached to the
page.

In WebSharper™ you normally do not work with inserted nodes,
so whenever you call constructor method such as:

```fsharp
Datepicker.New()
```

the actual initialization of the internal element is delayed until
this node is rendered to the page.

Therefore caution needs to be taken when using widget objects.  If the
widget is not yet rendered, some instance member calls may result in
runtime errors.

For example, the following code:

```fsharp
let datePicker = Datepicker.New()
datePicker.SetDate(new Date(2010,3,14))
```

will not work since the call to `SetDate` assumes that the
`datePicker` object is rendered.

Since all widgets implement the `IPagelet` interface you may use the
`OnAfterRender` function to accomodate for this.

You may rewrite the code above using the `OnAfterRender` as in:

```fsharp
let dp = DatePicker.New() 
dp.OnAfterRender(fun datePicker -> 
    dp.SetDate(new Date(2010,3,14))
)
```


## Events

jQuery UI widgets allow to add event handlers to widgets. For example,
the `Datepicker` widget supports the `onSelect` event:

```javascript
$("#datepicker").datepicker({
   onSelect: function(dateText, inst) { ... }
});
```

In the WebSharper™ binding Events are also represented as
functions. To add an event handler for a Datepicker objects you use:

```fsharp
datePicker.OnSelect (fun date -> ...)
```

The `OnSelect` function accepts a callback function of type
`JavaScript.Date -> unit`.  The signatures of the callback functions
differ slightly from their JavaScript counterparts.  For example, the
JavaScript callback function may accept two arguments, with the second
one representing a reference to the `datePicker` object and the date
is passed as a string rather than a `DateTime` object.

In contrast with other method invocations, events may be applied to
objects that has not yet been initialized (rendered).  If an event is
attached to a non initialized object it is implicitly delayed until
the rendering of the object. Therefore the following code is
perfectly valid:

```fsharp
let datePicker = DatePicker.New() 
datePicker.OnSelect(fun date -> ...)
```
# Differences between this WebSharper.UI.JQueryUI and WebSharper.JQueryUI
The main difference in usage is regarding the Doc combinators. The original
project relayed on the Pagelet class from WebSharper.Html extension, which
is not available on WebSharper.UI.

As it is not possible to inherit from the Doc or Elt classes, composition was
used instead and an underlying Elt element is exposed in the base class used
by all Widgets.

The base class expose an AsDoc() method to help with Doc elements composition.
The following examples illustrate how such method or the underlying element can
be used:
```fsharp
let datePicker = DatePicker.New()
div [] [ datePicker.element ]
```

or

```fsharp
let datePicker = DatePicker.New()
div [] [ datePicker.AsDoc() ]
```

Another difference is regarding the OnBeforeRender(), which doesn't exists on
WebSharper.UI. As it is not used anywhere in the Widget classes, it was used
by several examples on WebSharper.JQueryUI verion.

# Examples

This document lists a few usage examples of WebSharper.JQueryUI. To
read on the principles of WebSharper.UI.JQueryUI and how it differs from
the JavaScript library, check the [main documentation](WebSharperJQueryUI.md).

## Accordion

The following example shows how to wrap elements inside an Accordion
widget.

```fsharp
[<JavaScript>]
let Accordion () =
    let accElems =
        [
            "Panel 1", h1 [] [text "Panel 1"]
            "Panel 2", h1 [] [text "Panel 2"]
            "Panel 3", h1 [] [text "Panel 3"]
        ]
    let accordion = Accordion.New(accElems)
    div [] [accordion.AsDoc()]
```

Result:

![jqui-accordion.png](docs/jqui-accordion.png)



## Datepicker

The datepicker provides a convenient way of letting the user select
dates.  Here a label is updated with the most recently selected date
by utilizing the `OnSelect` function.

```fsharp
[<JavaScript>]
let PickDate () =
	let rvSelDate = Var.Create ""
	let selDate = label [] [ textView rvSelDate.View ]

	let datepicker = Datepicker.New()
	datepicker.OnSelect (fun dt dp ->
		Var.Set rvSelDate (dt.ToLocaleDateString())
	)
	div []
		[ div [] [label [] [text "Selected date:"]; selDate]
		  datepicker.AsDoc()
		]
```

![jqui-datepicker.png](docs/jqui-datepicker.png)



## Dialog

An example of how you can use the `Dialog` and `Button` widgets:

```fsharp
[<JavaScript>]
let Dialog () =
    let button = Button.New "Open"        
    let pan = div [] [button.AsDoc()]
    button.OnClick (fun ev ->            
        let dialog = Dialog.New(h1 [] [text "Dialog"])
        (pan :?> Elt).AppendChild(dialog.element)
        dialog.OnClose(fun ev ->
            button.Enable()
        )
        button.Disable()
    )
    pan
```

The button is created using the `New` constructor with a string
argument specifying the label.  To add an event handler for the click
for the button you use the `button.OnClick` function.  Here a new
`Dialog` object is created and the button is disabled. To enable the
button again when the dialog closes you use the OnClose function for
the dialog.

![jqui-buttonDialogOpen.png](docs/jqui-buttonDialogOpen.png)


## Slider

The following sample shows how to use the slider widget to let the
user specify a percentage value.

```fsharp
[<JavaScript>]
let Slider () =
	let rvSelVal = Var.Create ""
    let slider = Slider.New()
    let selVal = label [] [textView rvSelVal.View]
    slider.OnChange(fun ev ->
        Var.Set rvSelVal (string slider.Value + "%")
    )
    div []
		[ div []
		  	[ label [] [text "Selected Value: "]
                selVal            
              ]
		  slider.AsDoc()
	    ]		
```

To capture the change event for the slider you add an event handler
for the `OnChange` event. Every time the value of the slider changes,
the text property of the `selVal` label is updated with the most
recent text.

![jqui-slider.png](docs/jqui-slider.png)


## Tabs

In the example, the widgets above are displayed using the Tabs
control.  Each widget is displayed on a a separate tab panel.

```fsharp
[<JavaScript>]
let Main () =
    let tabElems =
        [
            "Dialog", Dialog ()
            "Accordion", Accordion ()
            "Datepicker", Date ()
            "Slider", Slider ()
        ]
    let tabs = Tabs.New(tabElems, TabsConfiguration())
    div [attr.Style "width:500px"] [tabs]
```

![jqui-tabs.png](docs/jqui-tabs.png)
