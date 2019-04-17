namespace Domain.Workflow
module BudgetAggregate = 
    open DomainModel.Expense
    open DomainModel.Budget
    open DomainEvents.BudgetEvent
    open DomainEvents
    open CommonTypes
    open DomainModel.Account

    let createBudget (BudgetId(id),name:String50):BudgetEvent = 
        
        let budgetCreatedEvent = {BudgetCreated.BudgetId = id.ToString(); Name = String50.value name}
        BudgetEvent.Created(budgetCreatedEvent)
    
    let connectToAccount (accountId: AccountId): BudgetEvent = 
        let accountConnectedEvent = {AccountConnected.AccountId = accountId}
        BudgetEvent.AccountConnected(accountConnectedEvent)
    
    let addExpense(budget: Budget, expense: Expense) : BudgetEvent =
        let defaultValue = Money(0m)
        let totE = Option.defaultValue defaultValue budget.TotalExpenses
        let remaining = budget.Remaining - expense.Amount
        let totalExpenses = totE + expense.Amount
        let expenseAddedEvent = {
            ExpenseAdded.BudgetId = budget.BudgetId; 
            ExpenseId = expense.ExpenseId;
            Remaining = remaining;
            TotalExpenses = totalExpenses
            }
        BudgetEvent.ExpenseAdded(expenseAddedEvent)
    
    let activate(budget:Budget) : BudgetEvent = 
        let activateEvent = {Activated.BudgetId = budget.BudgetId}
        BudgetEvent.Activated(activateEvent)
    
    let deActivate(budget:Budget) : BudgetEvent=
        let deActivateEvent = {DeActivated.BudgetId = budget.BudgetId}
        BudgetEvent.DeActivated(deActivateEvent)

    let changeAmount (budget: Budget, amount: Money) : BudgetEvent = 
        let delta = budget.InitialBalance - amount
        let remaining = budget.Remaining + delta
        let amountChangedEvent = {AmountChanged.BudgetId = budget.BudgetId; Amount = amount; Remaining= remaining}
        BudgetEvent.AmountChanged(amountChangedEvent)

    let changeName(budget:Budget, name: String50) : BudgetEvent = 
        let nameChangedEvent =  {NameChanged.BudgetId = budget.BudgetId; Name = String50.value name }
        BudgetEvent.NameChanged(nameChangedEvent)

    let remove(budget: Budget) : BudgetEvent = 
        let removeEvent = {Removed.BudgetId = budget.BudgetId}
        BudgetEvent.Removed(removeEvent)
    
    let delete(budget: Budget) : BudgetEvent = 
        let deleteEvent = {Deleted.BudgetId = budget.BudgetId}
        BudgetEvent.Deleted(deleteEvent)
    