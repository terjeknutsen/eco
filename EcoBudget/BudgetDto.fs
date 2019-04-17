namespace Eco.BudgetWorkFlow

open Eco.Common
open Eco.InternalTypes
open System
open System.Collections.Generic

[<AutoOpen>]
module internal Utils = 
    let defaultIfNone defaultValue opt = 
        match opt with
        | Some v -> v
        | None -> defaultValue
[<CLIMutable>]
type BudgetDto = {
    BudgetId: string
    Name: string option
    Amount : decimal option
    }

module internal BudgetDto = 
    let toUnvalidatedBudget (dto: BudgetDto) : UnvalidatedBudget = 
        {
            Id = dto.BudgetId
            Name = dto.Name
        }
    let toUnvalidatedAmount (dto: BudgetDto) : UnvalidatedBudgetAmount = 
        {
            Id = dto.BudgetId
            Amount = dto.Amount
        }

type BudgetAddedDto = {
    BudgetId : string
    Name : string
    }
module internal BudgetAddedDto = 
    let fromDomain (domainObj: BudgetAdded) : BudgetAddedDto = 
        {
            BudgetId = domainObj.BudgetId |> BudgetId.value
            Name = domainObj.Name |> String50.value
        }

//================================================================
// DTO for AddBudgetEvent
//================================================================

type AddBudgetEventDto = string*obj

module internal AddBudgetEventDto = 
    let fromDomain (domainObj: BudgetEvent) : AddBudgetEventDto = 
        match domainObj with
        | BudgetAdded budgetAdded -> 
            let obj = budgetAdded |> BudgetAddedDto.fromDomain |> box
            let key = "BudgetAdded"
            (key,obj)

//=============================================================
// DTO for AddBudgetError
//=============================================================

type AddBudgetErrorDto = {
    Code: string
    Message: string
    }
module internal AddBudgetErrorDto = 
    let fromDomain (domainObj: BudgetError) : AddBudgetErrorDto = 
        match domainObj with
        | Create validationError -> 
            let (ValidationError msg) = validationError
            {
                Code = "ValidationError"
                Message = msg
            }
        | RemoteService remoteServiceError -> 
            let msg = sprintf "%s: %s" remoteServiceError.Service.Name remoteServiceError.Exception.Message
            {
                Code = "RemoteServiceError"
                Message = msg
            }