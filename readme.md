F# script for deployment to AWS S3
----------------------------------
Script arguments:
* -f = path to a file to upload
* -k = S3 object key (optional)
* -b = S3 bucket name
* -ak = AWS access key
* -sk = AWS secret access key
* -re = AWS region endpoint, "us-east-1" by default
* -p = if present, the uploaded file will be public

#### Usage in MSBuild
```xml
<Target Name="UploadToS3" AfterTargets="AfterBuild" Condition="'$(BuildingInsideVisualStudio)' != 'true'">
  <GetFrameworkSdkPath>
    <Output TaskParameter="Path" PropertyName="SdkPath" />
  </GetFrameworkSdkPath>
  <PropertyGroup>
    <BucketName>deployment</BucketName>
    <!-- C:\Program Files (x86)\Microsoft SDKs\F#\3.0\Framework\v4.0\Fsi.exe -->
    <FsiPath>$(SdkPath)..\..\F#\3.0\Framework\v4.0\Fsi.exe</FsiPath>
    <UploadToS3Command>"$(FsiPath)" AwsS3Upload.fsx -f "$(OutDir)product.exe" -b "$(BucketName)" -ak AMAZONKEY -sk AMAZONSECRET -p</UploadToS3Command>
  </PropertyGroup>
  <Exec Command="$(UploadToS3Command)" />
</Target>
```