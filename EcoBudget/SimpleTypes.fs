namespace Eco.Common

open System

type String50 = private String50 of string  

type GoalId = private GoalId of string

type BillId = private BillId of string

type BudgetId = private BudgetId of string

type AccountId = private AccountId of string

type State = 
    |Active
    |InActive
    |Removed
    |Deleted  

[<AutoOpen>]
module UUIDPattern = 

    let get = "[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}"

module ConstrainedType = 

    let createString fieldName ctor maxLen str = 
        if String.IsNullOrEmpty(str) then
            let msg = sprintf "%s must not be null or empty" fieldName
            Error msg
        elif str.Length > maxLen then
            let msg = sprintf "%s must not be more than %i chars" fieldName maxLen
            Error msg
        else 
            Ok (ctor str)


    let createStringOption fieldName ctor maxLen str = 
        if String.IsNullOrEmpty(str) then
            Ok None
        elif str.Length > maxLen then
            let msg = sprintf "%s must not be more than %i chars" fieldName maxLen
            Error msg
        else 
            Ok (ctor str |> Some)

    let createInt fieldName ctor minVal maxVal i = 
        if i < minVal then
            let msg = sprintf "%s: Must not be less than %i" fieldName minVal
            Error msg
        elif i > maxVal then
            let msg = sprintf "%s: Must not be greater than %i" fieldName maxVal
            Error msg
        else
            Ok (ctor i)

    let createDecimal fieldName ctor minVal maxVal i = 
        if i < minVal then
            let msg = sprintf "%s: Must not be less than %M" fieldName minVal
            Error msg
        elif i > maxVal then
            let msg = sprintf "%s: Must not be greater than %M" fieldName maxVal
            Error msg
        else
            Ok (ctor i)

    let createLike fieldName ctor pattern str = 
        if String.IsNullOrEmpty(str) then
            let msg = sprintf "%s: Must not be null or empty" fieldName
            Error msg
        elif System.Text.RegularExpressions.Regex.IsMatch(str,pattern) then
            Ok (ctor str)
        else
            let msg = sprintf "%s: '%s' must match the pattern '%s'" fieldName str pattern
            Error msg

module String50 = 
    let value (String50 str) = str
    
    let create fieldName str = 
        ConstrainedType.createString fieldName String50 50 str
        
    let createOption fieldName str = 
        ConstrainedType.createStringOption fieldName String50 50 str
    
module GoalId = 
    let value (GoalId str) = str

    let create fieldName str =
        ConstrainedType.createLike fieldName GoalId UUIDPattern.get str 

module BillId = 
    let value (BillId str) = str
    let create fieldName str = 
        ConstrainedType.createLike fieldName BillId UUIDPattern.get str

module BudgetId = 
    let value (BudgetId str) = str
    let create fieldName str = 
        ConstrainedType.createLike fieldName BudgetId UUIDPattern.get str

module AccountId = 
    let value (AccountId str) = str
    let create fieldName str =
        ConstrainedType.createLike fieldName AccountId UUIDPattern.get str

[<AutoOpen>]
module Money = 
    type Money = 
        |Money of decimal
        static member (+) (Money(m1), Money(m2)) = Money(m1 + m2)
        static member (-) (Money(m1), Money(m2)) = Money(m1 - m2)
    
    let value (Money m) = m
