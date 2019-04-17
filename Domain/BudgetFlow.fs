namespace AggregateFlow

module BudgetFlow = 
    open Domain.Workflow
    open Dtos
    open DomainModel.Budget
    open CommonTypes.CommonTypes
    open CommonTypes
    open Microsoft.FSharp.Core
    open System
    open Backend.Event
    open DomainEvents.BudgetEvent
    open Backend.Event

    let save(e:DomainEvent<BudgetEvent>) = 
        let dbSave s:unit = 
            printfn "Saving budget event"
            e |> EventSaver.save
            //raise <| Exception "not implemented" 
        try
            dbSave e
            |> Ok
        with
        |e -> Error("Error while saving")
    
    let hydrate(id: string) = 
        id |> EventSaver.get
        

    let createBudgetId (e:string) = 
        let tryParseId (e:string):Guid =
             Guid.Parse(e)
        try
            let id =  tryParseId e
            Ok(BudgetId(id))
        with
        | e -> Error("Expected guid")

    let createBudget(command: Command<BudgetDto>): Result<unit,string> = 
        let userId = command.UserId
        let budgetId = createBudgetId command.Data.Id
        match budgetId with 
        |Ok b -> 
            let name = command.Data.Name |>  String50.create
            match name with
            |Ok r -> 
                let event = BudgetAggregate.createBudget(b,r)
                let domainevent = 
                    {
                        DomainEvent.Data = event 
                        TimeStamp = DateTime.Now 
                        UserId = userId
                        Version=1
                        ProcessId=command.Data.Id
                    }
                save(domainevent)
            |Error e -> Error e 
        |Error e -> Error e 
        
    let changeAmount(command:Command<BudgetDto>) : Result<unit,string> =
        match command.Data.Amount with
        |Some a -> 
            let amount = a |> Money.Money
            let events = command.Data.Id |> hydrate
            let budget = events |> DomainEvents.BudgetEvent.build
            let event = BudgetAggregate.changeAmount(budget,amount)
            let domainEvent = 
                {
                    DomainEvent.Data = event
                    TimeStamp = DateTime.Now
                    UserId = command.UserId
                    Version = events.Length + 1
                    ProcessId = command.Data.Id
                }
            save(domainEvent)
        |None _ -> Error "Amount cannot be empty"
