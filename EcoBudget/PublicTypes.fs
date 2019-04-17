namespace Eco.BudgetWorkFlow

open Eco.Common
//=========================
// input to workflow
//=========================
type UnvalidatedBudget = {
    Id: string
    Name: string option
    }
type UnvalidatedBudgetAmount = {
    Id: string
    Amount: decimal option
    }

type UnvalidatedExpense = {
    Id: string
    BudgetId: string
    Amount: decimal
    Description: string
    }






//==================================
//Outputs from the workflow (success case)
//==================================

type BudgetAdded = {
    BudgetId: BudgetId
    Name: String50
    }

type ExenseAdded = {
    BudgetId: BudgetId
    Amount: Money
    Description: string
    }

type BudgetEvent = BudgetAdded of BudgetAdded




//======================================
//Error outputs
//======================================
type ValidationError = ValidationError of string

type ServiceInfo = {
    Name: string
    EndPoint: System.Uri
    }

type RemoteServiceError = {
    Service: ServiceInfo
    Exception: System.Exception
    }
type BudgetError = 
    |Create of ValidationError
    |RemoteService of RemoteServiceError



//========================================
//The workflows
//========================================

type CreateBudget = 
    UnvalidatedBudget -> Result<BudgetEvent, BudgetError>

type ChangeAmount = 
    UnvalidatedBudgetAmount -> Result<BudgetEvent,BudgetError>

type AddExpense = 
    UnvalidatedExpense -> Result<BudgetEvent,BudgetError>

