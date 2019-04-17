module BudgetApi

open Newtonsoft.Json
open Eco.BudgetWorkFlow
open System.Net

module HttpStatus = 
    type HttpStatus = private HttpStatus of int
    let getValue(HttpStatus(s)) = s
    let error = HttpStatus(401)
    let ok = HttpStatus(200)
    let created = HttpStatus(201)
    let accepted = HttpStatus(202)
    let noContent = HttpStatus(204)
    let badRequest= HttpStatus(400)
    let notFound = HttpStatus(404)
    let methodNotAllowed = HttpStatus(405)
    let notAcceptable = HttpStatus(406)
    let isOk(HttpStatus(s)) = s < 400 

type JsonString = string


type HttpRequest = {
    Action: string
    Uri: string
    Body: BudgetDto
    }
type HttpResponse = {
    HttpStatusCode : HttpStatus.HttpStatus
    Body: JsonString
    }
type AddBudgetApi = HttpRequest -> HttpResponse
type ChangeAmountApi = HttpRequest -> HttpResponse

let undefinedResponse = {HttpStatusCode = HttpStatus.badRequest; Body = ""}

let workflowResultToHttpResponse result = 
    match result with
    | Ok events -> 
        let dtos = 
            events
            |>  AddBudgetEventDto.fromDomain
        let json = JsonConvert.SerializeObject(dtos)
        let response = 
            {
                HttpStatusCode = HttpStatus.created
                Body = json
            }
        response
    | Error err -> 
        let dto = err |> AddBudgetErrorDto.fromDomain
        let json = JsonConvert.SerializeObject(dto)
        let response = 
            {
                HttpStatusCode = HttpStatus.notAcceptable
                Body = json
            }
        response

let saveEvent(result: Result<BudgetEvent,BudgetError>) : Result<BudgetEvent,BudgetError> = 
    match result with
    |Ok events -> 
        printf "%s" "test"
        result
    |Error err -> 
        result

let addBudgetApi : AddBudgetApi = 
    fun request -> 
        let unvalidatedBudget = request.Body |> BudgetDto.toUnvalidatedBudget

        let workflow = 
            BudgetImplementation.createBudget 
        let result = workflow unvalidatedBudget
        result |> 
        saveEvent |> 
        workflowResultToHttpResponse

type undefined = exn

let changeAmountApi : ChangeAmountApi = 
    fun request -> 
        let unvalidatedBudgetAmount = request.Body |> BudgetDto.toUnvalidatedAmount
        undefinedResponse

        