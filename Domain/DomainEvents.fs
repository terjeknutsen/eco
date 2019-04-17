namespace DomainEvents
module rec AccountEvent =
    open DomainModel.Account
    open CommonTypes

    type AccountEvent = 
    |Created of AccountCreated
    |NameChanged of AccountNameChanged
    |FundsAdded of AccountFundsAdded
    |Withdrawal of AccountFundsWithdrawn

    type AccountCreated ={
    AccountId : AccountId
    Name : string}

    type AccountNameChanged = {
    AccountId : AccountId
    Name : string}
    
    type AccountFundsAdded = {
    AccountId : AccountId
    Total : Money
    Available : Money
    AmountAdded: Money}

    type AccountFundsWithdrawn = {
    AccountId: AccountId
    Total : Money
    Available : Money
    AmountWithdrawn : Money}

     
    let applyEvent =
     fun e a -> 
        match e with
        |Created c -> {a with Account.AccountId = c.AccountId; Name = c.Name;}
        |NameChanged x -> {a with Name = x.Name}
        |FundsAdded x ->  
               let total = Some(x.Total)
               let available = Some(x.Available)
               {a with Total = total; Available = available}
        |Withdrawal x -> 
                let total = Some(x.Total)
                let available = Some(x.Available)
                {a with Total = total; Available = available}
    let emptyAccount = {AccountId = AccountId(".."); Name=".."; Total=None;Available=None; State=State.InActive}
    
    let build(accountEvents: AccountEvent array) = 
        Array.fold(fun acc elem -> applyEvent elem acc) emptyAccount accountEvents

module rec BudgetEvent =
    open DomainModel.Budget
    open DomainModel.Expense
    open DomainModel.Account
    open CommonTypes
    open System

    type BudgetEvent = 
        |Created of BudgetCreated
        |AccountConnected of AccountConnected
        |ExpenseAdded of ExpenseAdded
        |Activated of Activated
        |DeActivated of DeActivated
        |AmountChanged of AmountChanged
        |NameChanged of NameChanged
        |Removed of Removed
        |Deleted of Deleted
    
   
    [<CLIMutable>]
    type BudgetCreated = {
    BudgetId : string
    Name : string
    }

    type AccountConnected = {
    AccountId: AccountId}
    
    type ExpenseAdded = {
    BudgetId : BudgetId
    ExpenseId: ExpenseId
    Remaining: Money
    TotalExpenses: Money
    }
   
    type Activated = {
    BudgetId : BudgetId}
    
    type DeActivated = {
    BudgetId : BudgetId}
    
    type AmountChanged ={
        BudgetId:BudgetId
        Amount:Money
        Remaining: Money}
    
    type NameChanged={
    BudgetId: BudgetId
    Name : string}
    
    type Deleted = {
    BudgetId: BudgetId}
    
    type Removed = {
    BudgetId: BudgetId}
       

    let applyEvent = 
        fun e b -> 
        match e with
        |Created x ->
            let name = String50.create(x.Name)
            match name with 
            |Ok n -> {b with Budget.BudgetId = BudgetId(Guid.Parse(x.BudgetId)); Name = Some n}
            |Error _e -> raise (System.Exception(_e))
            
        |AccountConnected x -> {b with AccountId = Some x.AccountId}
        |ExpenseAdded x ->
            let exsistingExpenses = Option.defaultValue [] b.Expenses
            {b with Remaining = x.Remaining; TotalExpenses = Some x.TotalExpenses; Expenses = Some (x.ExpenseId :: exsistingExpenses) }
        |Activated x -> {b with State = State.Active}
        |DeActivated x -> {b with State = State.InActive}
        |AmountChanged x -> {b with InitialBalance=x.Amount; Remaining = x.Remaining }
        |NameChanged x -> 
            let name = String50.create(x.Name)
            match name with
            |Ok n -> {b with Name= Some(n)}
            |Error _e -> raise (System.Exception _e)
            
        |Removed x -> {b with State = State.Removed}
        |Deleted x -> {b with State = State.Deleted}

    let emptyBudget = {
        BudgetId=BudgetId(System.Guid.NewGuid()) 
        AccountId= None
        Name= None 
        Period = None
        Remaining = Money(0m)
        InitialBalance=Money(0m) 
        State = State.InActive
        Expenses = None
        TotalExpenses = None
        }

    let build(budgetEvents: BudgetEvent array) = 
        Array.fold(fun acc elem -> applyEvent elem acc) emptyBudget budgetEvents