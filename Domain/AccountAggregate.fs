namespace Domain.Workflow
module AccountFlow =
    open DomainModel.Account
    open DomainEvents.AccountEvent
    open CommonTypes

    let createAccount (id: string, name: string) : AccountEvent = 
        let accountId = AccountId(id)
        let accountCreatedEvent = {AccountCreated.AccountId = accountId; Name = name}
        AccountEvent.Created(accountCreatedEvent)
    
    let addFunds(account: Account, amount: Money) : AccountEvent =
        let defaultMoney = Money(0m)
        let currentTotal = Option.defaultValue defaultMoney account.Total
        let newTotal = amount + currentTotal
        let currentAvailable = Option.defaultValue defaultMoney account.Available
        let newAvailable = amount + currentAvailable
        let fundsAddedEvent = 
            {
                AccountFundsAdded.AccountId = account.AccountId; 
                Total = newTotal; 
                Available = newAvailable;
                AmountAdded = amount
            }
        AccountEvent.FundsAdded(fundsAddedEvent)
    
    let reName(account:Account, newName: string):AccountEvent =
        let renameEvent = {AccountNameChanged.AccountId = account.AccountId; Name=newName}
        AccountEvent.NameChanged(renameEvent)
    
    let withdrawFunds(account: Account, amount: Money) : AccountEvent =
        let defaultMoney = Money(0m)
        let currentTotal = Option.defaultValue defaultMoney account.Total
        let newTotal = currentTotal - amount
        let currentAvaliable = Option.defaultValue defaultMoney account.Available
        let newAvailable = currentAvaliable - amount
        let withdrawEvent = 
            {
                AccountFundsWithdrawn.AccountId = account.AccountId; 
                Total = newTotal;
                Available = newAvailable
                AmountWithdrawn = amount 
            }
        AccountEvent.Withdrawal(withdrawEvent)
        
        
    
