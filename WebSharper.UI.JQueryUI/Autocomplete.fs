// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2018 IntelliFactory
//
// Licensed under the Apache License, Version 2.0 (the "License"); you
// may not use this file except in compliance with the License.  You may
// obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
// implied.  See the License for the specific language governing
// permissions and limitations under the License.
//
// $end{copyright}

//JQueryUI Wrapping: (version Stable 1.8rc1)
namespace WebSharper.UI.JQueryUI

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Html

type AutocompleteRequest =
    {
        [<Name "term">]
        Term : string
    }

type AutocompleteItem =
    {
        [<Name "label">]
        Label : string

        [<Name "value">]
        Value : string
    }

type AutocompleteCallback =
    AutocompleteRequest * (AutocompleteItem[] -> unit) -> unit

[<JavaScript>]
type AutocompleteSource =
    | Listing of array<string>
    | Items of array<AutocompleteItem>
    | Url of string
    | Callback of AutocompleteCallback

type AutocompleteConfiguration[<JavaScript>]() =

    [<DefaultValue>]
    val mutable source : obj

    [<Direct "this.source=function(x, y) { $scall([x, y]) }">]
    let setCallback (scall : AutocompleteCallback) = ()

    [<Name "appendTo">]
    [<Stub>]
    member val AppendTo = Unchecked.defaultof<string> with get, set

    [<Name "autoFocus">]
    [<Stub>]
    member val AutoFocus = Unchecked.defaultof<bool> with get, set

    [<Name "delay">]
    [<Stub>]
    member val Delay = Unchecked.defaultof<int> with get, set

    [<Name "disabled">]
    [<Stub>]
    member val Disabled = Unchecked.defaultof<bool> with get, set

    [<Name "minLength">]
    [<Stub>]
    member val MinLength = Unchecked.defaultof<int> with get, set

    [<Name "position">]
    [<Stub>]
    member val Position = Unchecked.defaultof<PositionConfiguration> with get, set

    [<JavaScript>]
    member this.Source
        with set (s : AutocompleteSource) =
            match s with
            | Listing x -> this.source <- unbox x
            | Items x -> this.source <- unbox x
            | Url x -> this.source <- unbox x
            | Callback x -> setCallback x

module internal AutocompleteInternal =
    [<Inline "jQuery($el).autocomplete($conf)">]
    let Init (el: Dom.Element, conf: AutocompleteConfiguration) = ()

[<Require(typeof<Dependencies.JQueryUIJs>)>]
[<Require(typeof<Dependencies.JQueryUICss>)>]
type Autocomplete[<JavaScript>] internal () =
    inherit Utils.Pagelet()

    (****************************************************************
    * Constructors
    *****************************************************************)
    /// Creates an autocomplete widget from the given element and
    /// configuration object.
    [<JavaScript>]
    [<Name "New1">]
    static member New (el : Doc, conf: AutocompleteConfiguration): Autocomplete =
        let ac = new Autocomplete()
        ac.element <- 
            (el :?> Elt).OnAfterRender (fun el  ->
                AutocompleteInternal.Init(el, conf)
            )
        ac

    /// Creates an autocomplete widget from the given element and
    /// configuration object.
    [<JavaScript>]
    [<Name "New2">]
    static member New (el : Doc): Autocomplete =
        Autocomplete.New(el, new AutocompleteConfiguration())

    /// Creates an autocomplete widget from an input element using
    /// the default configuration.
    [<JavaScript>]
    [<Name "New3">]
    static member New (): Autocomplete =
        Autocomplete.New(input [] [] :?> Elt, new AutocompleteConfiguration())

    /// Creates an autocomplete widget from an input element using
    /// the given configuration.
    [<JavaScript>]
    [<Name "New4">]
    static member New (conf: AutocompleteConfiguration): Autocomplete =
        Autocomplete.New(input [] [] :?> Elt, conf)

    (****************************************************************
    * Methods
    *****************************************************************)
    /// Remove the autocomplete functionality completely.
    [<Inline "jQuery($this.element.elt).autocomplete('destroy')">]
    member this.Destroy() = ()

    /// Disables the autocomplete.
    [<Inline "jQuery($this.element.elt).autocomplete('disable')">]
    member this.Disable () = ()

    // Enables the autocomplete.
    [<Inline "jQuery($this.element.elt).autocomplete('enable')">]
    member this.Enable () = ()

    /// Get or set any autocomplete option. If no value is specified, will act as a getter.
    [<Inline "jQuery($this.element.elt).autocomplete('option', $name, $value)">]
    member this.Option (name: string, value: obj) = ()

    /// Get or set any autocomplete option. If no value is specified, will act as a getter.
    [<Inline "jQuery($this.element.elt).autocomplete('option', $name)">]
    member this.Option (name: string) = X<obj>

    /// Gets all options.
    [<Inline "jQuery($this.element.elt).autocomplete('option')">]
    member this.Option () = X<AutocompleteConfiguration>

    /// Sets one or more options.
    [<Inline "jQuery($this.element.elt).autocomplete('option', $options)">]
    member this.Option (options: AutocompleteConfiguration) = X<unit>

    [<Inline "jQuery($this.element.elt).autocomplete('widget')">]
    member private this.getWidget () = X<Dom.Element>

    /// Returns the .ui-autocomplete element.
    [<JavaScript>]
    member this.Widget = this.getWidget()

    /// Triggers a search event using the current string value, which, when data is available,
    /// then will display the suggestions;
    /// can be used by a selectbox-like button to open the suggestions when clicked.
    /// The current input's value is used.
    [<Inline "jQuery($this.element.elt).autocomplete('search')">]
    member this.Search () = ()

    /// Triggers a search event using the given string value, which, when data is
    /// available, then will display the suggestions;
    /// can be used by a selectbox-like button to open the suggestions when clicked.
    [<Inline "jQuery($this.element.elt).autocomplete('search', $value)">]
    member this.Search (value: string) = ()

    /// Close the Autocomplete menu. Useful in combination with
    /// the search method, to close the open menu.
    [<Inline "jQuery($this.element.elt).autocomplete('close')">]
    member this.Close () = ()

    (****************************************************************
    * Events
    *****************************************************************)

    [<Inline "jQuery($this.element.elt).bind('autocompletecreate', function (x,y) {($f(x));})">]
    member private this.onCreate(f : JQuery.Event -> unit) = ()

    [<Inline "jQuery($this.element.elt).bind('autocompletesearch', function (x,y) {($f(x));})">]
    member private this.onSearch(f : JQuery.Event -> unit) = ()

    [<Inline "jQuery($this.element.elt).bind('autocompleteopen', function (x,y) {($f(x));})">]
    member private this.onOpen(f : JQuery.Event -> unit) = ()

    [<Inline "jQuery($this.element.elt).bind('autocompleteresponse', function (x,y) {($f(x))(y.content);})">]
    member private this.onResponse(f : JQuery.Event -> AutocompleteItem[] -> unit) = ()

    [<Inline "jQuery($this.element.elt).bind('autocompletefocus', function (x,y) {($f(x))(y.item);})">]
    member private this.onFocus(f : JQuery.Event -> string -> unit) = ()

    [<Inline "jQuery($this.element.elt).bind('autocompleteselect', function (x,y) {($f(x))(y.item);})">]
    member private this.onSelect(f : JQuery.Event -> string -> unit) = ()

    [<Inline "jQuery($this.element.elt).bind('autocompleteclose', function (x,y) {($f(x));})">]
    member private this.onClose(f : JQuery.Event -> unit) = ()

    /// After an item was selected; ui.item refers to the selected item.
    /// Always triggered after the close event
    [<Inline "jQuery($this.element.elt).bind('autocompletechange', function (x,y) {($f(x))(y.item);})">]
    member private this.onChange(f : JQuery.Event -> string -> unit) = ()

    /// This event is triggered when autocomplete is created.
    [<JavaScript>]
    member this.OnCreate f =
        this.OnAfterRender (fun _ -> this.onCreate f)
        |> ignore

    /// Triggered before a request (source-option) is started.
    [<JavaScript>]
    member this.OnSearch f =
        this.OnAfterRender (fun _ -> this.onSearch f)
        |> ignore

    /// After an item was selected. Always triggered after the close event.
    [<JavaScript>]
    member this.OnChange f =
        this.OnAfterRender (fun _ -> this.onChange f)
        |> ignore

    /// This event is triggered when autocomplete is created.
    [<JavaScript>]
    member this.OnResponse f =
        this.OnAfterRender (fun _ -> this.onResponse f)
        |> ignore

    /// Triggered when the list is opened.
    [<JavaScript>]
    member this.OnOpen f =
        this.OnAfterRender (fun _ -> this.onOpen f)
        |> ignore

    /// Triggered when the list is hidden.
    [<JavaScript>]
    member this.OnClose f =
        this.OnAfterRender (fun _ -> this.onClose f)
        |> ignore

    /// Before focus is moved to an item (not selecting), ui.item refers to the focused item.
    [<JavaScript>]
    member this.OnFocus f =
        this.OnAfterRender (fun _ -> this.onFocus f)
        |> ignore

    /// Tiggered when an item is selected from the menu;
    [<JavaScript>]
    member this.OnSelect f =
        this.OnAfterRender (fun _ -> this.onSelect f)
        |> ignore
