namespace DomainModel

module rec Goal = 
    open CommonTypes

    type Goal =
        {
            GoalId : GoalId
            Name : string
            ProgressPerMinute : Progress
        }
    
    type GoalId = private GoalId of id:string
    let idFromString(id:string) : GoalId = 
        GoalId(id)

    type Progress = private Progress of Money.Money
module rec Expense = 
    open CommonTypes

    type Expense = 
        {
            ExpenseId : ExpenseId
            Name : string
            Amount : Money
        }
    type ExpenseId = private ExpenseId of id: string
    let idFromString id = ExpenseId(id)

open Expense


module rec Account =
    open CommonTypes

    type Account = 
        {
            AccountId: AccountId
            Name: string
            Total: Money option
            Available: Money option
            State: State
        }
  

    type AccountId = AccountId of id: string

module rec Budget = 
    open Account
    open CommonTypes
    open System
    
    type Budget = 
        {
            BudgetId: BudgetId
            AccountId: AccountId option
            Name: String50 option
            Period: Period option
            Remaining: Money
            InitialBalance: Money 
            State: State
            Expenses: ExpenseId list option
            TotalExpenses: Money option
        }

    type BudgetId = BudgetId of id: Guid
