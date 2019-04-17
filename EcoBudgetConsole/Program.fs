// Learn more about F# at http://fsharp.org

open System
open Newtonsoft.Json

type body = {
        BudgetId : string
        Name: string
        }
[<EntryPoint>]
let main argv =
    let budgetId = System.Guid.NewGuid().ToString()
    let name = "budget"
    
    let body = {BudgetId = budgetId; Name = name}
    let jsonBody = JsonConvert.SerializeObject(body)
    
    let httpRequest = {
        BudgetApi.HttpRequest.Action = "get"
        BudgetApi.HttpRequest.Uri = "budget"
        BudgetApi.HttpRequest.Body = jsonBody
        }
    let response = BudgetApi.addBudgetApi httpRequest
    let responseText = sprintf "%i - %s " response.HttpStatusCode response.Body
    Console.WriteLine(responseText)
    Console.ReadKey()
    0 // return an integer exit code
