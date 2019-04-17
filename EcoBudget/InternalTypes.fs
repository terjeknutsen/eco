module internal Eco.InternalTypes

open Eco.Common
open Eco.BudgetWorkFlow

//=========================================
//Define each step in the workflow using internal types
//=========================================

//-------------------------
//Validation step
//-------------------------

type ValidatedBudget = {
    Budgetid : BudgetId
    Name : String50
    }

type ValidateBudget = 
    UnvalidatedBudget -> Result<ValidatedBudget,ValidationError>


type CreateEvents = 
    ValidatedBudget -> BudgetEvent 