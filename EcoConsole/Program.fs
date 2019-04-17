// Learn more about F# at http://fsharp.org

open System
open Domain.Workflow
open DomainEvents
open DomainModel
open CommonTypes.Money
open AggregateFlow
open Dtos
open CommonTypes.CommonTypes

[<EntryPoint>]
let main argv =
    printfn "Hello World from F#! %s" "test"
    let createEvent = AccountFlow.createAccount(Guid.NewGuid().ToString(), "Min konto")
    
    let myEvents = [|createEvent|]

    let account = DomainEvents.AccountEvent.build(myEvents) 

    let addFunds = AccountFlow.addFunds(account, Money(100m))
    

    let myEvents = [|createEvent;addFunds|]

    let account = DomainEvents.AccountEvent.build(myEvents)
    let withdraw = AccountFlow.withdrawFunds(account, Money(10m))
    let myEvents = [|createEvent;addFunds;withdraw|]
    let account = DomainEvents.AccountEvent.build(myEvents)
    let defaultAmount = Money.Money 0m
    let total = Option.defaultValue defaultAmount account.Total
    let available = Option.defaultValue defaultAmount account.Available

    printfn "Name: %s Total: %f Available: %f" account.Name (value total) (value available)
    let dto = {BudgetDto.Id = Guid.NewGuid().ToString(); Name = "Enda ett budsjett"; Amount=None}
    let command = {Command.Data = dto; TimeStamp = DateTime.Now; UserId = "terjeK"}
    let result = BudgetFlow.createBudget(command)

    let amountDto = {dto with Amount = Some 200m}
    let amountCommand = {Command.Data = amountDto; TimeStamp = DateTime.Now; UserId = "terjeK"}
    let changeAmount = BudgetFlow.changeAmount(amountCommand)
    
    match changeAmount with
    |Ok e -> Console.Write("Ok")
    |Error e -> Console.WriteLine(e)

    Console.ReadKey();

    0 // return an integer exit code
