// Arguments:
// -f = path to a file to upload
// -k = S3 object key (optional)
// -b = S3 bucket name
// -ak = AWS access key
// -sk = AWS secret access key
// -re = AWS region endpoint, "us-east-1" by default
// -p = if present, the uploaded file will be public

module AwsS3UploadOptions

type Options = {
    FilePath: string;
    Key: string;
    BucketName: string;
    AccessKey: string;
    SecretKey: string;
    RegionEndpoint: string;
    MakePublic: bool
}

let rec parseCmdArgs (args:string list) (options: Options) =
    match args with
    | "-f"::rest ->
        match rest with
        | file::otherArgs ->
            let newOptions = { options with FilePath = file }
            parseCmdArgs otherArgs newOptions
        | [] -> failwith "Incomplete file path (-f option)"
    | "-k"::rest ->
        match rest with
        | key::otherArgs ->
            let newOptions = { options with Key = key }
            parseCmdArgs otherArgs newOptions
        | [] -> failwith "Incomplete object key (-k option)"
    | "-b"::rest ->
        match rest with
        | bucket::otherArgs ->
            let newOptions = { options with BucketName = bucket }
            parseCmdArgs otherArgs newOptions
        | [] -> failwith "Incomplete bucket name (-b option)"
    | "-ak"::rest ->
        match rest with
        | accessKey::otherArgs ->
            let newOptions = { options with AccessKey = accessKey }
            parseCmdArgs otherArgs newOptions
        | [] -> failwith "Incomplete access key (-ak option)"
    | "-sk"::rest ->
        match rest with
        | secretKey::otherArgs ->
            let newOptions = { options with SecretKey = secretKey }
            parseCmdArgs otherArgs newOptions
        | [] -> failwith "Incomplete secret key (-sk option)"
    | "-re"::rest ->
        match rest with
        | region::otherArgs ->
            let newOptions = { options with RegionEndpoint = region }
            parseCmdArgs otherArgs newOptions
        | [] -> failwith "Incomplete region endpoint (-re option)"
    | "-p"::rest ->
        let newOptions = { options with MakePublic = true }
        parseCmdArgs rest newOptions
    | option::rest ->
        printfn "Unrecognized option %s" option
        parseCmdArgs rest options
    | [] -> options