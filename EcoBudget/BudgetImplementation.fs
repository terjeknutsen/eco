module internal BudgetImplementation
open Eco.Common
open Eco.InternalTypes
open Eco.BudgetWorkFlow


let toBudgetId budgetId = 
    budgetId
    |> BudgetId.create "BudgetId"
    |> Result.mapError ValidationError



let validateBudget(unvalidatedBudget: UnvalidatedBudget) = 
        result {
            
            let! budgetId = 
                unvalidatedBudget.Id
                |> toBudgetId
            
            let! name = 
                let validatedName = unvalidatedBudget.Name
                match validatedName with
                    |Some n -> n 
                                |> String50.create "BudgetName"
                                |> Result.mapError ValidationError
                    |None -> Error "BudgetName missing"
                                |> Result.mapError ValidationError
            
            let validatedBudget : ValidatedBudget = {
                Budgetid = budgetId
                Name = name
            }
        return validatedBudget
    }
let createBudgetAddedEvent (validatedBudget: ValidatedBudget) : BudgetEvent = 
    let budgetId = validatedBudget.Budgetid
    let name = validatedBudget.Name
    let budgetAdded = {BudgetAdded.BudgetId = budgetId; Name = name }
    BudgetEvent.BudgetAdded(budgetAdded) 
    

let createEvents : CreateEvents = 
    fun validatedBudget -> 
              validatedBudget
              |> createBudgetAddedEvent  
              
         


let createBudget : CreateBudget = 
    fun unvalidatedBudget -> 
        result {
         let! validatedBudget = 
            validateBudget unvalidatedBudget
            |> Result.mapError BudgetError.Create
            
         let events = createEvents validatedBudget
         return events
    }

let changeAmount : 