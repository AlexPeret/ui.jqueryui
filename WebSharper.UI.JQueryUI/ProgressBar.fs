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

type ProgressbarConfiguration[<JavaScript>]() =

    [<Name "disabled">]
    [<Stub>]
    member val Disabled = Unchecked.defaultof<bool> with get, set

    [<Name "value">]
    [<Stub>]
    //0 by default
    member val Value = Unchecked.defaultof<int> with get, set

    [<Name "max">]
    [<Stub>]
    //100 by default
    member val Max = Unchecked.defaultof<int> with get, set

module internal ProgressbarInternal =
    [<Inline "jQuery($el).progressbar($conf)">]
    let Init (el: Dom.Element, conf: ProgressbarConfiguration) = ()

[<Require(typeof<Dependencies.JQueryUIJs>)>]
[<Require(typeof<Dependencies.JQueryUICss>)>]
type Progressbar[<JavaScript>]internal () =
    inherit Utils.Pagelet()

    (****************************************************************
    * Constructors
    *****************************************************************)
    /// Creates a new progressbar given an element and a
    /// configuration object.
    [<JavaScript>]
    [<Name "New1">]
    static member New (el: Doc, conf: ProgressbarConfiguration) =
        let pb = new Progressbar()
        pb.element <-
            (el :?> Elt).OnAfterRender (fun el  ->
                ProgressbarInternal.Init(el, conf)
            )
        pb

    /// Creates a new progressbar given an element using
    /// the default configuration.
    [<JavaScript>]
    [<Name "New2">]
    static member New (el: Doc) =
        Progressbar.New(el, new ProgressbarConfiguration())


    /// Creates a new progressbar based on an
    /// empty Div element and the given a configuration object.
    [<JavaScript>]
    [<Name "New3">]
    static member New ( conf: ProgressbarConfiguration) =
        Progressbar.New(div [] [] :?> Elt, conf)

    /// Creates a new progressbar based on an empty Div element
    /// and the default configuration.
    [<JavaScript>]
    [<Name "New4">]
    static member New () =
        Progressbar.New(div [] [] :?> Elt, new ProgressbarConfiguration())

    (****************************************************************
    * Methods
    *****************************************************************)
    /// Removes the progressbar functionality completely.
    [<Inline "jQuery($this.element.elt).progressbar('destroy')">]
    member this.Destroy() = ()

    /// Disables the progressbar functionality.
    [<Inline "jQuery($this.element.elt).progressbar('disable')">]
    member this.Disable () = ()

    /// Enables the progressbar functionality.
    [<Inline "jQuery($this.element.elt).progressbar('enable')">]
    member this.Enable () = ()

    /// Sets a progressbar option.
    [<Inline "jQuery($this.element.elt).progressbar('option', $name, $value)">]
    member this.Option (name: string, value: obj) = ()

    /// Gets a progressbar option.
    [<Inline "jQuery($this.element.elt).progressbar('option', $name)">]
    member this.Option (name: string) = X<obj>

    [<Inline "jQuery($this.element.elt).progressbar('widget')">]
    member private this.getWidget () = X<Dom.Element>

    /// Returns the .ui-progressbar element.
    [<JavaScript>]
    member this.Widget = this.getWidget()

    /// Sets the value of the progressbar.
    [<Inline "jQuery($this.element.elt).progressbar('value', $v)">]
    member private this.setValue (v: int) = ()

    /// Gets the value of the progressbar.
    [<Inline "jQuery($this.element.elt).progressbar('value')">]
    member private this.getValue () = 0

    /// Gets or sets the value of the progressbar.
    [<JavaScript>]
    member this.Value
        with get () =
            this.getValue()
        and set (v: int) =
            this.setValue v

    (****************************************************************
    * Events
    *****************************************************************)
    [<Inline "jQuery($this.element.elt).bind('progressbarcreate', function (x,y) {$f(x);})">]
    member private this.onCreate(f : JQuery.Event -> unit) = ()

    [<Inline "jQuery($this.element.elt).bind('progressbarchange', function (x,y) {$f(x);})">]
    member private this.onChange(f : JQuery.Event -> unit) = ()

    [<Inline "jQuery($this.element.elt).bind('progressbarcomplete', function (x,y) {$f(x);})">]
    member private this.onComplete(f : JQuery.Event -> unit) = ()

    /// Event triggered when the progressbar is created.
    [<JavaScript>]
    member this.OnCreate(f : JQuery.Event -> unit) =
        this.OnAfterRender(fun _ ->
            this.onCreate f
        )
        |> ignore

    /// Event triggered when the value of the progressbar changes.
    [<JavaScript>]
    member this.OnChange(f : JQuery.Event -> unit) =
        this.OnAfterRender(fun _ ->
            this.onChange f
        )
        |> ignore

    /// This event is triggered when the value of the progressbar reaches the maximum value of 100.
    [<JavaScript>]
    member this.OnComplete(f : JQuery.Event -> unit) =
        this.OnAfterRender(fun _ ->
            this.onComplete f
        )
        |> ignore

