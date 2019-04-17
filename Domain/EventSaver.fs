namespace Backend.Event
module EventSaver = 
    open JsonFlatFileDataStore
    open CommonTypes.CommonTypes
    open DomainEvents.BudgetEvent
    open System.Collections.Generic
    open System.Linq.Expressions
    open System.Linq


    let private addToStore b = 
        async{
          use db = new DataStore("data.json")
          let c = db.GetCollection<DomainEvent<BudgetEvent>>()
          try
            do!
              c.InsertOneAsync(b) |> Async.AwaitTask |> Async.Ignore
          with
          | e ->  
            printfn "%s" e.Message
        }
    
    let save(e:DomainEvent<BudgetEvent>) = 
        e |> addToStore |> Async.RunSynchronously
    
    let get(id: string) = 
            use db = new DataStore("data.json")
            let c = db.GetCollection<DomainEvent<BudgetEvent>>()
            let queryable = c.AsQueryable()
            queryable 
            |> Seq.toArray 
            |> Array.filter(fun a -> a.ProcessId = id) 
            |> Array.map(fun a -> a.Data)