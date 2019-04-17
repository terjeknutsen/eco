 module internal BudgetWriteStore
    open Microsoft.WindowsAzure.Storage
    open Newtonsoft.Json
    open Eco.BudgetWorkFlow
    open Eco.Common

    let budgetContainer =  
        let connString = "-----------"
        let storageAccount = CloudStorageAccount.Parse(connString)
        let blobClient = storageAccount.CreateCloudBlobClient()
        let container = blobClient.GetContainerReference("Budget")
        container.CreateIfNotExistsAsync() |> Async.AwaitTask |> ignore
        container

    let saveBudgetEvent(budgetEvent: BudgetEvent) =
        match budgetEvent with
        |BudgetAdded event -> 
            let id = BudgetId.value event.BudgetId
            let blobId = sprintf "Budget%s" id
            let blob = budgetContainer.GetBlockBlobReference(blobId)
            let json = JsonConvert.SerializeObject budgetEvent
            blob.UploadTextAsync(json)
    

    ///--------------------------------
    /// Read about Azure blob storage
    /// Save events to own store and get list of events

    //let getEvents budgetId = 
    //    async{
    //    let id = BudgetId.value budgetId
    //    let blobId = sprintf "Budget%s" id
    //    let blob = budgetContainer.GetBlockBlobReference(blobId)
    //    let! events = blob.DownloadTextAsync() |> Async.AwaitTask
    //    let budgetDto = JsonConvert.DeserializeObject<BudgetDto> events
    //    return budgetDto
    //    }