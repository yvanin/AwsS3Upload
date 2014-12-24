#load "AwsS3UploadOptions.fsx"
#r "AWSSDK.dll"

open System.Net
open AwsS3UploadOptions

[<Literal>]
let RESULT_SUCCESS = 0

[<Literal>]
let RESULT_ERROR = 1

let upload (options: Options) =
    let region =
        match Amazon.RegionEndpoint.GetBySystemName(options.RegionEndpoint) with
        | region when region.DisplayName <> "Unknown" -> region
        | _ ->
            printfn "Region %s not found. Using us-east-1." options.RegionEndpoint
            Amazon.RegionEndpoint.USEast1

    let s3client = new Amazon.S3.AmazonS3Client(options.AccessKey, options.SecretKey, region)

    let putObjectRequest = new Amazon.S3.Model.PutObjectRequest()
    putObjectRequest.FilePath <- options.FilePath
    if options.Key <> null then
        putObjectRequest.Key <- options.Key
    putObjectRequest.BucketName <- options.BucketName
    if options.MakePublic then
        putObjectRequest.CannedACL <- Amazon.S3.S3CannedACL.PublicRead

    let response = s3client.PutObject(putObjectRequest)
    match response.HttpStatusCode with
    | HttpStatusCode.OK -> RESULT_SUCCESS
    | statusCode ->
        printfn "Uploading to AWS S3 failed. Server response: %s" (response.ToString())
        RESULT_ERROR

let defaultOptions = {
    FilePath = null;
    Key = null;
    BucketName = null;
    AccessKey = null;
    SecretKey = null;
    RegionEndpoint = "us-east-1";
    MakePublic = false
}

let options = parseCmdArgs (fsi.CommandLineArgs |> List.ofSeq) defaultOptions

printfn "AWS S3 Upload options:\n%A\n" options
printfn "Uploading to S3..."

match upload options with
| RESULT_SUCCESS ->
    printfn "File uploaded."
    exit RESULT_SUCCESS
| result ->
    printfn "Upload failed."
    exit result