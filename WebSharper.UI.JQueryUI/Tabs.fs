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
open WebSharper.UI.Client


type TabsAjaxOptionsConfiguration =
    {
    [<Name "ajaxOptions">]
    async: bool
    }

    [<JavaScript>]
    static member Default = {async = false}


type TabsCookieConfiguration =
    {
    [<Name "cookie">]
    expires: int
    }

    [<JavaScript>]
    static member Default = {expires = 30}


type TabsFxConfiguration =
    {
    [<Name "fx">]
    opacity: string
    }
    [<JavaScript>]
    static member Dafault = {opacity = "toggle"}


type TabsInfo =
    {
        tab : JQuery.JQuery
        panel : JQuery.JQuery
    }

type TabsActivateEvent =
    {
        [<Name "newTab">]
        NewTab : JQuery.JQuery

        [<Name "oldTab">]
        OldTab : JQuery.JQuery

        [<Name "newPanel">]
        NewPanel : JQuery.JQuery

        [<Name "oldPanel">]
        OldPanel : JQuery.JQuery
    }

type TabsConfiguration[<JavaScript>]() =

    [<Name "active">]
    [<Stub>]
    //null by default
    member val Active = Unchecked.defaultof<int> with get, set

    [<Name "collapsible">]
    [<Stub>]
    //false by default
    member val Collapsible = Unchecked.defaultof<bool> with get, set

    [<Name "disabled">]
    [<Stub>]
    //[] by default
    member val Disabled = Unchecked.defaultof<array<int>> with get, set

    [<Name "event">]
    [<Stub>]
    //"click" by default
    member val Event = Unchecked.defaultof<string> with get, set

    [<Name "heightStyle">]
    [<Stub>]
    //"content" by default
    member val HeightStyle = Unchecked.defaultof<string> with get, set

    [<Name "hide">]
    [<Stub>]
    //null by default
    member val Hide = Unchecked.defaultof<string> with get, set

    [<Name "show">]
    [<Stub>]
    //null by default
    member val Show = Unchecked.defaultof<string> with get, set

[<AutoOpen>]
module internal TabsInternal =
    [<Inline "jQuery($el).tabs($conf)">]
    let Init(el: Dom.Element, conf: TabsConfiguration) = ()

[<Require(typeof<Dependencies.JQueryUIJs>)>]
[<Require(typeof<Dependencies.JQueryUICss>)>]
type Tabs[<JavaScript>] internal (tabContainer, panelContainer) =
    inherit Utils.Pagelet()

    (****************************************************************
    * Constructors
    *****************************************************************)
    /// Creates a new tabs object with panels and titles fromt the given
    /// list of name and element pairs and configuration settings object.
    [<JavaScript>]
    [<Name "New1">]
    static member New (els : List<string * Doc>, conf: TabsConfiguration): Tabs =
        let panelContainer, tabContainer =
            let itemPanels =
                els
                |> List.map (fun (label, panel) ->
                   let id = Utils.NewId()
                   let item = li [] [a [attr.href ("#" + id)] [text label]]
                   let tab = div [attr.id id] [panel]
                   //(item :?> Utils.Pagelet, tab :?> Utils.Pagelet)
                   (item, tab)
                )
            let tabs = ul [] (Seq.map fst itemPanels)
            let tabs' = 
                //(tabs :?> Utils.Pagelet :: List.map snd itemPanels)
                (tabs :: List.map snd itemPanels)
            div [] tabs', tabs

        let tabs = new Tabs (tabContainer, panelContainer)
        tabs.element <-
            (panelContainer :?> Elt)
                .OnAfterRender (fun el ->
                    TabsInternal.Init(el, conf)
                )
        tabs



    /// Creates a new tabs object using the default configuration.
    [<JavaScript>]
    [<Name "New2">]
    static member New (els : List<string * Doc>): Tabs =
        Tabs.New(els, new TabsConfiguration())


    (****************************************************************
    * Methods
    *****************************************************************)
    /// Removes the tabs functionality completely.
    [<Inline "jQuery($this.element.elt).tabs('destroy')">]
    member this.Destroy() = ()

    /// Disables the tabs functionality.
    [<Inline "jQuery($this.element.elt).tabs('disable')">]
    member this.Disable () = ()

    /// Enables the tabs functionality.
    [<Inline "jQuery($this.element.elt).tabs('enable')">]
    member this.Enable () = ()

    /// Sets a tabs option.
    [<Inline "jQuery($this.element.elt).tabs('option', $name, $value)">]
    member this.Option (name: string, value: obj) = ()

    /// Gets a tabs option.
    [<Inline "jQuery($this.element.elt).tabs('option', $name)">]
    member this.Option (name: string) = X<obj>

    [<Inline "jQuery($this.element.elt).tabs('widget')">]
    member private this.getWidget () = X<Dom.Element>

    /// Returns the .ui-tabs element.
    [<JavaScript>]
    member this.Widget = this.getWidget()

    /// Reloads the content of an Ajax tab.
    [<Inline "jQuery($this.element.elt).tabs('load', $index)">]
    member this.Load (index: int) = ()

    /// Process any tabs that were added or removed directly in the DOM and recompute the height of the tab panels. Results depend on the content and the heightStyle option.
    [<Inline "jQuery($this.element.elt).tabs('refresh')">]
    member this.Refresh () = ()

    /// Retrieve the number of tabs of the first matched tab pane.
    [<JavaScript>]
    member this.Length = JQuery.JQuery.Of(tabContainer).Children().Length


    /// Add a new tab with the given element and label
    /// inserted at the specified index.
    [<JavaScript>]
    member this.Add(el: Doc, label: string, ix: int) =
        let id = Utils.NewId()
        let tab = li [] [a [attr.href ("#" + id)] [text label]]
        let panel = div [attr.id id] [el]
        JQuery.JQuery.Of(tabContainer).Children().Eq(ix).Before((tab :?> Elt).GetDom()).Ignore
        JQuery.JQuery.Of(panelContainer).Children().Eq(ix).After((panel :?> Elt).GetDom()).Ignore // after because the first child is the tabset
        //tab.Render()
        //panel.Render()
        this.Refresh()

    /// Add a new tab with the given element and label.
    [<JavaScript>]
    member this.Add(el: Doc, label: string) =
        let id = Utils.NewId()
        let tab = li [] [a [attr.href ("#" + id)] [text label]]
        let panel = div [attr.id id] [el]
        (tabContainer :?> Elt).AppendChild(tab)
        (panelContainer :?> Elt).AppendChild(panel)
        this.Refresh()

    /// Remove the tab at the specified index.
    [<JavaScript>]
    member this.Remove(ix: int) =
        JQuery.JQuery.Of(tabContainer).Children().Eq(ix).Remove().Ignore
        JQuery.JQuery.Of(panelContainer).Children().Eq(ix).Remove().Ignore
        this.Refresh()

    (****************************************************************
    * Utilities
    *****************************************************************)

    [<JavaScript>]
    member this.TabContainer = tabContainer

    (****************************************************************
    * Events
    *****************************************************************)
    [<Inline "jQuery($this.element.elt).bind('tabscreate', function (x,y) {($f(x))(y);})">]
    member private this.onCreate(f : JQuery.Event -> TabsInfo -> unit) = ()

    [<Inline "jQuery($this.element.elt).bind('tabsload', function (x,y) {($f(x))(y);})">]
    member private this.onLoad(f : JQuery.Event -> TabsInfo -> unit) = ()

    [<Inline "jQuery($this.element.elt).bind('tabsbeforeload', function (x,y) {($f(x))(y);})">]
    member private this.onBeforeLoad(f : JQuery.Event -> TabsInfo -> unit) = ()

    [<Inline "jQuery($this.element.elt).bind('tabsactivate', function (x,y) {($f(x))(y);})">]
    member private this.onActivate(f : JQuery.Event -> TabsActivateEvent -> unit) = ()

    [<Inline "jQuery($this.element.elt).bind('tabsbeforeactivate', function (x,y) {($f(x))(y);})">]
    member private this.onBeforeActivate(f : JQuery.Event -> TabsActivateEvent -> unit) = ()


    /// Event triggered when the tabs are created.
    [<JavaScript>]
    member this.OnCreate f =
        this.OnAfterRender(fun _ ->
            this.onCreate f
        )
        |> ignore

    /// Event triggered when a tab is loaded.
    [<JavaScript>]
    member this.OnLoad f =
        this.OnAfterRender(fun _ ->
            this.onLoad f
        )
        |> ignore

    /// Event triggered before a tab is loaded.
    [<JavaScript>]
    member this.OnBeforeLoad f =
        this.OnAfterRender(fun _  ->
            this.onBeforeLoad f
        )
        |> ignore

    /// Event triggered when a tab is activated.
    [<JavaScript>]
    member this.OnActivate f =
        this.OnAfterRender(fun _  ->
            this.onActivate f
        )
        |> ignore

    /// Event triggered when a tab is beforeActivated.
    [<JavaScript>]
    member this.OnBeforeActivate f =
        this.OnAfterRender(fun _  ->
            this.onBeforeActivate f
        )
        |> ignore

