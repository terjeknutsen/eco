namespace CommonTypes
[<AutoOpen>]
module CommonTypes =

    type Period = Period of fromTime:System.DateTime * toTime:System.DateTime
    
    type State = 
        |Active
        |InActive
        |Removed
        |Deleted
    
    type Command<'T> = {
    Data : 'T
    TimeStamp: System.DateTime
    UserId: string}

    [<CLIMutable>]
    type DomainEvent<'T> = {
    Data: 'T
    TimeStamp: System.DateTime
    Version: int
    UserId: string
    ProcessId: string}

[<AutoOpen>]
module Money =
    type Money = 
        |Money of decimal
        static member (+) (Money(m1), Money(m2)) =  Money(m1 + m2)
        static member (-) (Money(m1), Money(m2)) = Money(m1 - m2)
    let value(Money(m1)) = m1

[<AutoOpen>]
module String50 = 
    type String50 = private String50 of string
    let value (String50(a)) : string = a 
    let create txt = 
        if isNull txt then
            Error "cannot be null"
        else if System.String.IsNullOrEmpty(txt) then
            Error "cannot be empty"
        else if System.String.IsNullOrWhiteSpace(txt) then
            Error "cannot be white space"
        else if txt.Length > 50 then
            Error "cannot be more than 50 chars"
        else
            Ok (String50 txt)