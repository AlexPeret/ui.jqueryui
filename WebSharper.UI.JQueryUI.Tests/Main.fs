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
namespace WebSharper.UI.JQueryUI.Tests

open WebSharper

[<JavaScript>]
module internal Client =

    open WebSharper.JavaScript
    open WebSharper.UI
    open WebSharper.UI.Html
    open WebSharper.UI.Client
    open WebSharper.JQuery
    open WebSharper.UI.JQueryUI

    let private Log (x: string) = Console.Log(x)

    let TestAccordion () =
        let els1 =
            [
                "Foo", div [] [button [] [text "click"]]
                "Bar", div [] [text "Second content"]
                "Baz", div [] [text "Third content"]
            ]
        let options1 = new AccordionConfiguration(Collapsible = true)
        let acc1 = Accordion.New(els1, options1)
        // Events
        acc1.OnAfterRender (fun _ ->
            Log "Acc1 - After Render"
        ) |> ignore
        acc1.OnActivate (fun _ _ ->
            Log "Acc1 - Activate"
        ) |> ignore

        let els2 =
            [
                "Foo", div [] [acc1.AsDoc()]
                "Bar", div [] [text "Second content"]
                "Baz", div [] [text "Third content"]
            ]
        let options2 = new AccordionConfiguration(Collapsible = true)
        let acc2 = Accordion.New(els2, options2)

        // Events
        acc2.OnActivate (fun _ _ ->
            Log "Acc2 - Activate"
        ) |> ignore
        let btn1 = JQueryUI.Button.New("Click")
        btn1.OnClick (fun _ ->
            Console.Log "btn1.OnClick"
            acc2.Activate(2)
            acc1.Disable()
        )
        let btn2 = JQueryUI.Button.New "Click"
        btn2.OnClick (fun _ ->
            Console.Log "btn2.OnClick"
            acc2.Activate 1
        )
        div [] [acc2.AsDoc(); btn1.AsDoc(); btn2.AsDoc()]

    let RunAutocompleter conf =
        let ac = Autocomplete.New(input [] [], conf)
        ac.OnAfterRender ( fun _ -> Log "After Render") |> ignore
        ac.OnChange (fun _ _ -> Log "Change")
        ac.OnClose <| fun _ -> Log "Close"
        ac.OnSearch <| fun _ -> Log "Search"
        ac.OnFocus <| fun _ _ -> Log "Focus"

        let bClose = JQueryUI.Button.New "Close"
        bClose.OnClick (fun _ -> ac.Close())

        let bDestroy = JQueryUI.Button.New "Destroy"
        bClose.OnClick (fun _ -> ac.Destroy())

        div []
            [ ac.AsDoc()
              bClose.AsDoc()
              bDestroy.AsDoc()
            ]

    let TestAutocomplete1 () =
        let conf = new AutocompleteConfiguration()
        conf.Source <- Listing [|"Apa"; "Beta"; "Zeta" ; "Zebra"|]
        RunAutocompleter conf

    let TestAutocomplete2 () =
        let conf = new AutocompleteConfiguration()
        let x : array<AutocompleteItem> =
            [|{Label = "test"; Value = "value"}|]
        conf.Source <- Items x
        RunAutocompleter conf

    let TestAutocomplete3 () =
        let conf = new AutocompleteConfiguration()
        let completef (_ : AutocompleteRequest, f : array<AutocompleteItem> -> unit) =
            let x : array<AutocompleteItem> =
                [|{Label = "test"; Value = "value"}|]
            f x
        conf.Source <- Callback completef
        RunAutocompleter conf

    let TestButton () =
        let b1 = JQueryUI.Button.New ("Click")
        b1.OnAfterRender(fun _ -> Log "After Render") |> ignore
        b1.OnClick(fun ev -> Log "Click")
        let b2 = JQueryUI.Button.New "Click 2"
        b2.OnClick(fun ev ->
            b1.OnClick (fun ev -> Log "New CB")
            if b1.IsEnabled then
                b1.Disable()
            else
                b1.Enable()
        )
        div [] [b1.AsDoc(); b2.AsDoc()]

    let TestDatepicker1 () =
        let conf = new DatepickerConfiguration()
        let dp = Datepicker.New(input [] [], conf)
        dp.OnAfterRender(fun _ -> Log "Dp After Render") |> ignore
        div [] [dp.AsDoc()]

    let TestDatepicker2 () =
        let conf = new DatepickerConfiguration(AutoSize = true)
        let dp = Datepicker.New(input [] [],conf)

        dp.OnClose(fun dt elem ->
            Log "Dp2 OnClose"
            Console.Log dt
            Console.Log elem
        )
        dp.OnSelect(fun dt elem ->
            Log "Dp2 OnSelect"
            Console.Log dt
            Console.Log elem
        )

        dp.OnAfterRender (fun elem ->
            dp.Option("changeYear", true)
        ) |> ignore

        dp.OnAfterRender(fun elem -> 
            Log "Dp2 After Render"
            Log (dp.Option("changeYear").ToString())
            Log (dp.Option("autoSize").ToString())
            Console.Log <| dp.Option()
        ) |> ignore
        div [] [dp.AsDoc()]

    let TestDatepicker3 () =
        let conf = 
            new DatepickerConfiguration(
                AutoSize = true,
                OnClose = (
                    fun dt ->
                        Log "Dp3 OnClose"
                        Console.Log dt),
                OnSelect = (
                    fun dt ->
                        Log "Dp3 OnSelect"
                        Console.Log dt)
            )
        let dp = Datepicker.New(input [] [],conf)

        dp.OnAfterRender (fun elem ->
            dp.Option("changeYear", true)
        ) |> ignore

        dp.OnAfterRender(fun elem -> 
            Log "Dp3 After Render"
            Log (dp.Option("changeYear").ToString())
            Log (dp.Option("autoSize").ToString())
            Console.Log <| dp.Option()
        ) |> ignore
        div [] [dp.AsDoc()]


    let TestDatepicker4 () =
        let conf = 
            new DatepickerConfiguration(
                AutoSize = true,
                ChangeMonth = true,
                ChangeYear = true,
                NumberOfMonths = [|1;3|],
                MinDate = Date(),
                DefaultDate = Date(),
                ShowButtonPanel = true,
                OnClose = (
                    fun dt ->
                        Log "Dp4 OnClose"
                        Console.Log dt),
                OnSelect = (
                    fun dt ->
                        Log "Dp4 OnSelect"
                        Console.Log dt)
            )

        let dp = Datepicker.New(input [] [],conf)
        dp .OnAfterRender(fun elem ->
            Console.Log "OnAfterRender chained"
            dp.SetDate(new Date(2010,3,14))
        ) |> ignore

        dp.OnBeforeShow(fun _ ->
            Log "Dp4 OnBeforeShow"
            Console.Log (dp.GetDate())
            dp.Option()
        )

        dp.OnBeforeShowDay(fun dt ->
            if (dt.GetDay() % 2 = 0) then
                [|false; ""; "Reserved"|]
            else
                [|true; "ui-state-highlight"; "Available"|]
        )

        dp.OnClose(fun dt elem ->
            Log "Dp4 OnClose"
            Console.Log dt
            Console.Log elem
        )

        dp.OnAfterRender (fun elem ->
            dp.Option("changeYear", true)
        ) |> ignore

        dp.OnAfterRender(fun elem -> 
            Log "Dp4 After Render"
            Log (dp.Option("changeYear").ToString())
            Log (dp.Option("autoSize").ToString())
            Console.Log <| dp.Option()
        ) |> ignore
        div [] [dp.AsDoc()]

    let TestPickDate () =
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


    let TestDraggable () =
        let d =
            Draggable.New(
                div [attr.style "width:200px;background:lightgray;text-align:center"]
                    [ text "Drag me!" ],
                DraggableConfiguration(Axis = "x"))
        div [] [d.AsDoc()]


    let TestDialog () =
        let conf = DialogConfiguration()
        conf.Buttons <- [|DialogButton(Text = "Ok", Click = fun d e -> d.Close())|]
        conf.AutoOpen <- false
        let d = Dialog.New(div [] [text "Dialog"], conf)
        d.OnClose(fun ev ->
            Log "close"
        )
        d.OnAfterRender(fun _ -> Log "dialog: after render") |> ignore
        d.OnOpen(fun ev -> Log "dialog: open")
        d.OnClose(fun ev -> Log "dialog: close")
        d.OnResize(fun ev -> Log "dialog: resize")
        d.OnResizeStop(fun ev -> Log "dialog: resize stop")
        d.OnResizeStart(fun ev -> Log "dialog: resize start")
        d.OnFocus(fun ev -> Log "dialog: focus")
        d.OnDrag(fun ev -> Log "dialog: drag")
        d.OnDragStart(fun ev -> Log "dialog: drag start")
        d.OnDragStop(fun ev -> Log "dialog: drag stop")
        let bO = JQueryUI.Button.New ("open")
        bO.OnClick (fun ev -> d.Open())
        let bC = JQueryUI.Button.New "Close"
        bC.OnClick (fun ev -> d.Close())
        div [] 
            [d.AsDoc()
             bO.AsDoc()
             bC.AsDoc()
        ]

    let TestProgressbar () =
        let conf = ProgressbarConfiguration()
        let pb = Progressbar.New(div [] [], conf)
        pb.OnAfterRender(fun _ ->
            pb.Value <- 30
        ) |> ignore

        let btn = JQueryUI.Button.New("inc")
        btn.OnClick (fun ev ->
            pb.Value <- pb.Value + 10
        )
        div [] [pb.AsDoc(); btn.AsDoc()]


    let TestSlider () =
        let slider = Slider.New()
        slider.OnAfterRender(fun _  ->
            Log "slider: after render"
        ) |> ignore
        slider.OnChange(fun ev ->
            Log "change"
        )
        let btn = JQueryUI.Button.New("check")
        let pan = div [] [slider.AsDoc(); btn.AsDoc()]
        btn.OnClick (fun ev ->
            let dialog = Dialog.New(div [] [text <| string slider.Value])
            (pan :?> Elt).AppendChild(dialog.AsDoc())
        )
        pan

    let TestTabs () =
        let conf = new TabsConfiguration()
        let tabs =
            [
                "Tab 1",  div [] [h1 [] [text "Tab 1"]]
                "Tab 2",  div [] [h1 [] [text "Tab 2"]]
                "Tab 3" , div [] [text "R"]
            ]
        let tab = Tabs.New(tabs, conf)
        tab.OnAfterRender(fun _ ->  Log "Aa" ) |> ignore

        let btn = JQueryUI.Button.New("inc")
        btn.OnClick (fun ev ->
            JQuery.Of(tab.TabContainer).Children().Eq(2).Click().Ignore
            tab.Add(div [] [h1 [] [text "New tab"]], "Tab" + (string (tab.Length + 1)))
        )
        div [] [tab.AsDoc(); btn.AsDoc()]

    let TestSortable () =
        let elem =
            List.init 6 (fun i ->
                attr.src ("http://www.look4design.co.uk/l4design/companies/designercurtains/image" + string (i+1) + ".jpg"))
            |> List.map (fun e -> li [] [img [e] []])
            |> ul [attr.style "list-style: none"]
        let sortable = Sortable.New elem
        div [] [sortable.AsDoc()]

    let TestWidget t w =
        div [attr.style "border:solid 1px gray; padding:10px; margin-top: 10px"]
            [ h1 [] [text t ]
              w
            ]

    [<Inline "jQuery(document)">]
    let Document () : Dom.Element = Unchecked.defaultof<_>()

    let TestPosition() =
        let position1Body =
            div [attr.style "width:50px; height:50px; background-color:#F00;"] []
        let targetBody =
            div [attr.style "width:240px; height:200px; background-color:#999; margin:30px auto;"] [text "hej"]
        (targetBody :?> Elt)
            .OnAfterRender (fun el ->
                let conf1 = new PositionConfiguration()
                conf1.My <- "center"
                conf1.At <- "center"
                conf1.Of <- JQueryUI.Target.Element el
                conf1.Collision <- "fit"
                conf1.offset <- "10 -10"
                let p1 = Position.New(position1Body, conf1)
                ()
            ) |> ignore

        div []
            [ position1Body
              targetBody
            ]

//        let position2Body =
//            Div [Style "width:50px; height:50px; background-color:#0F0;"]
//        let conf2 = new PositionConfiguration()
//        conf2.My <- "left top"
//        conf2.At <- "left top"
//        conf2.Of <- Target.Element targetBody.Dom
//        conf2.Collision <- "fit"
//        conf2.offset <- "10 -10"
//        let p2 = Position.New(position2Body, conf2)
//
//        let position3Body =
//            Div [Style "width:50px; height:50px; background-color:#00F;"]
//        let conf3 = new PositionConfiguration()
//        conf3.My <- "right center"
//        conf3.At <- "right bottom"
//        conf3.Of <- Target.Element targetBody.Dom
//        conf3.Collision <- "fit"
//        conf3.offset <- "10 -10"
//        let p3 = Position.New(position3Body, conf3)
//
//        let position4Body =
//            Div [Style "width:50px; height:50px; background-color:#FF0;"]
//        let conf4 = new PositionConfiguration()
//        conf4.My <- "left bottom"
//        conf4.At <- "center"
//        conf4.Of <- Target.Element targetBody.Dom
//        conf4.Collision <- "fit"
//        conf4.offset <- "10 -10"
//        let p4 = Position.New (position4Body, conf4)
//
//        Document()
//        |>! OnMouseMove (fun _ ev ->
//            let conf = new PositionConfiguration()
//            conf.My <- "left bottom"
//            conf.At <- "center"
//            conf.Of <- Target.Event ev
//            conf.Collision <- "fit"
//            conf.offset <- "10 -10"
//            Position.New (position4Body, conf)|>ignore)
//        |> ignore
//        Div [
//            targetBody
//            ]
//            -< [p1; p2; p3; p4 ]

    let TestResizable () =
        let img = div [attr.style "background:url(http://www.look4design.co.uk/l4design/companies/light-iq/image14.jpg);height:100px;width:100px" ] []
        let resizable = Resizable.New img
        resizable.OnStart  (fun _ _ -> Log("Started!"))
        resizable.OnResize (fun event ui ->
            if ui.Size.Width > 300 then
                ui.Size.Width <- 300
            if ui.Size.Height < 200  then
                ui.Size.Height <- 200
            Log("Resized!"))
        resizable.OnStop   (fun _ _ -> Log("Stopped!"))
        let drag = Draggable.New (div [] [resizable.AsDoc()])
        div [] [drag.AsDoc()]


    let Tests () =
        let tab =
            [
                "Accordion", TestAccordion ()
                "Autocomplete1", TestAutocomplete1 ()
                "Autocomplete2", TestAutocomplete2 ()
                "Autocomplete3", TestAutocomplete3 ()
                "Button", TestButton ()
                "Datepicker1", TestDatepicker1 ()
                "Datepicker2", TestDatepicker2 ()
                "Datepicker3", TestDatepicker3 ()
                "Datepicker4", TestDatepicker4 ()
                "PickDate", TestPickDate ()
                "Draggable", TestDraggable ()
                "Dialog", TestDialog ()
                "Progressbar", TestProgressbar ()
                "Slider", TestSlider ()
                "Tabs", TestTabs ()
                "Sortable", TestSortable ()
                "Position", TestPosition ()
                "Resizable",  TestResizable ()
            ]
            |> Tabs.New
        div [] [tab.AsDoc()]

open WebSharper.Sitelets

type Action = | Index

module Site =

    open WebSharper.UI.Html

    let HomePage ctx =
        Content.Page(
            Title = "WebSharper JQueryUI Tests",
            Body = [ div [] [client <@ Client.Tests() @>] ]
        )

    [<Website>]
    let Main =
        Sitelet.Content "/" Index HomePage


[<Sealed>]
type Website() =
    interface IWebsite<Action> with
        member this.Sitelet = Site.Main
        member this.Actions = [Action.Index]

[<assembly: Website(typeof<Website>)>]
do ()
