# Academy .NET Training Backend

## ES: Este código ha sido migrado de un repositorio de BitBucket, donde tengo otra cuenta que figura como "tnapolitano".
## EN: This code has been migrated from BitBucket, where I have a different account showing here as "tnapolitano".

## Código
Incorporar el codigo en la carpeta `src/`para que el pipeline pueda buildear y desplegar.
## Build
```bash
$ dotnet restore
$ dotnet dotnet publish --no-restore --no-self-contained --configuration Release --output ../publish
```
## Deploys
Los commits a la rama **develop** se construyen y despliegan automáticamente al entorno **DEV**.
Para pasar al entorno **UAT/QAT** se debe realizar un **PullRequest** desde la rama **develop** hacia la rama **master**.
