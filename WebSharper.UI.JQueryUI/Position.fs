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

//
//module Position =

type Target =
    | Element of Dom.Element
    | Event of JQuery.Event
    | Id of string

    [<JavaScript>]
    member this.Get =
        match this with
        | Element el -> box el
        | Event ev   -> box ev
        | Id str     -> box ("#" + str)


type PositionConfiguration [<JavaScript>]() =

    [<Name "my">]
    [<Stub>]
    //Possible values: "top", "center", "bottom", "left", "right"
    member val My = Unchecked.defaultof<string> with get, set

    [<Name "at">]
    [<Stub>]
    //Possible values: "top", "center", "bottom", "left", "right"
    member val At = Unchecked.defaultof<string> with get, set

    //Element to position against. You can use a browser event object contains pageX and pageY values. Example: "#top-menu"
    [<DefaultValue>]
    val mutable ``of``: obj

    //Element to position against. You can use a browser event object contains pageX and pageY values. Example: "#top-menu"
    [<DefaultValue>]
    val mutable private ofTarget : Target

    [<JavaScript>]
    member this.Of
        with get () =
            this.ofTarget
        and set t =
            this.ofTarget <- t
            this.``of`` <- t.Get

    //Add these left-top values to the calculated position, eg. "50 50" (left top) A single value such as "50" will apply to both
    [<DefaultValue>]
    val mutable offset: string
    //
    [<DefaultValue>]
    val mutable private offsetTuple: (int * int)

    [<Name "collision">]
    [<Stub>]
    //This accepts a single value or a pair for horizontal/vertical, eg. "flip", "fit", "fit flip", "fit none".
    member val Collision = Unchecked.defaultof<string> with get, set

    [<Name "by">]
    [<Stub>]
    //When specified the actual property setting is delegated to this callback. Receives a single parameter which is a hash of top and left values for the position that should be set.
    member val By = Unchecked.defaultof<unit -> unit> with get, set //should take a one parameter

    [<Name "bgiframe">]
    [<Stub>]
    //true by default
    member val Bgiframe = Unchecked.defaultof<bool> with get, set

    [<JavaScript>]
    member this.Offset
        with get () =
            this.offsetTuple
        and set pos =
            this.offsetTuple <- pos
            let (x,y) = pos
            this.offset <- string x + " " + string y


module internal PositionInternal =
    [<Inline "jQuery($el).position($conf)">]
    let internal New (el: Dom.Element, conf: PositionConfiguration) = ()

[<Require(typeof<Dependencies.JQueryUIJs>)>]
[<Require(typeof<Dependencies.JQueryUICss>)>]
type Position [<JavaScript>] internal () =
    inherit Utils.Pagelet()

    (****************************************************************
    * Constructors
    *****************************************************************)
    /// Creates a new position object given an element and a
    /// configuration object.
    [<JavaScript>]
    [<Name "New1">]
    static member New (el : Doc, conf: PositionConfiguration): Position =
        let pos = new Position()
        pos.element <-
            ((el :?> Elt).OnAfterRender (fun el  ->
                PositionInternal.New(el, conf)
            ))
        pos

    /// Creates a new position object given an element
    /// using the default configuration.
    [<JavaScript>]
    [<Name "New2">]
    static member New (el : Doc) : Position =
        let conf = new PositionConfiguration()
        Position.New(el, conf)
