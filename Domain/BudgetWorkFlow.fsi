namespace WorkFlow
    module Money = 
        type Money = Money of decimal
    module Budget =

        type Command<'a> = {
         Data: 'a
         TimeStamp: System.DateTime}

         type BudgetId = BudgetId of string
         type Dto = {
            BudgetId: BudgetId
            Name: string
            Amount: Money.Money
            }
        
        type Create =  Command<Dto> -> Result<unit,string>
