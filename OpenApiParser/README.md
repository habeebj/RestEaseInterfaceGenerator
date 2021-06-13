# OpenApiParser
Converts open api specification to code (using `CSharpCodeWriter`)

## Supports
- version 2.x.x
- version 3.x.x

## Usage
```bash
dotnet run -- -u https://petstore.swagger.io/v2/swagger.json -o Sample
```

```bash
dotnet OpenApiParser.dll -u https://petstore.swagger.io/v2/swagger.json -o Sample
```