# musicstoreapi
Este proyecto forma parte del curso de MitoCode .NET 6 FullStack y Angular 14.
El cual se encuentra en la [web oficial de MitoCode](https://mitocode.com/netfs.html)

## Referencias de comandos utiles para programar con .NET CLI (Command Line Interface) #


## Crear un proyecto de libreria de clases 
`dotnet new classlib -o MusicStore.Entities`

_La plantilla "Biblioteca de clases" se creo correctamente._

## Crear un proyecto de pruebas unitarias con xUnit
`dotnet new xunit -o MusicStore.UnitTest`

_La plantilla "xUnit Test Project" se creo correctamente._

## Listar los archivos de una carpeta

`ls`

## Agregar un proyecto a una solucion existente

`dotnet sln add .\MusicStore.Entities\`

_Se ha agregado el proyecto "MusicStore.Entities\MusicStore.Entities.csproj" a la solución._

`dotnet sln add .\MusicStore.DataAccess\`

_Se ha agregado el proyecto "MusicStore.DtaAccess\MusicStore.DataAccess.csproj" a la solución._

`dotnet sln add .\MusicStore.Dto\`

_Se ha agregado el proyecto "MusicStore.Dto\MusicStore.Dto.csproj" a la solución._

## Consultar que proyectos estan dentro de la solucion

`dotnet sln .\MusicStore.sln list`

## Agregar un paquete nuget a un proyecto existente

`dotnet add package Microsoft.EntityFrameworkCore --version 6.0.11`

_Tener en cuenta que de no saber el nombre del paquete y la version a instalar se puede consultar en nuget.org_

## Agregar una referencia de proyecto 
`dotnet add reference ..\MusicStore.Entities\`

_Se ha agregado la referencia "..\MusicStore.Entities\MusicStore.Entities.csproj" al proyecto._

## Comprobar si se tiene instalado Entity Framework Core Tools

`dotnet ef`

_Deberia mostrarse el resultado siguiente:_

                     _/\__
               ---==/    \\
         ___  ___   |.    \|\
        | __|| __|  |  )   \\\
        | _| | _|   \_/ |  //|\\
        |___||_|       /   \\\/\\

Entity Framework Core .NET Command-line Tools 6.0.11

## Instalar EF Core Tools de manera global en el equipo

`dotnet tool install dotnet-ef --global`

_La herramienta "dotnet-ef" ya está instalada._

## Actualizar EF Core Tools

`dotnet tool update dotnet-ef --global`

_La herramienta "dotnet-ef" se reinstaló con la versión estable más reciente (versión "6.0.11")._

## Crear una migracion con EF

`dotnet ef migrations add Initial-Migration --startup-project .\MusicStore\ --project .\MusicStore.DataAccess\`

_Donde el parametro **--startup-project** es el proyecto que contiene la cadena de conexion y el parametro **--project** es el que contiene la clase con el DbContext._

## Aplicar una migracion
`dotnet ef database update --startup-project .\MusicStore\ --project .\MusicStore.DataAccess\`

_Esto hará que se cree la base de datos en la cadena de conexión de no existir._

## Utilizar Database First con EF CLI
`dotnet ef dbcontext scaffold "Server=.;Database=MusicStore;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer --startup-project .\MusicStore\ --project .\MusicStore.DataAccess\`
_Donde el parametro **--startup-project** es el proyecto que contiene la cadena de conexion y el parametro **--project** es el que contiene la clase con el DbContext._

## Utilizar Database First con EF CLI importando solo algunas tablas y evitar colocar el noconfiguring
`dotnet ef dbcontext scaffold "Server=.;Database=MusicStore;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer --startup-project .\MusicStore\ --project .\MusicStore.DataAccess\ --table Sale --table Customer --table Genre --table Events --no-onconfiguring`

__Repetir el parametro --Table o -t por cada tabla que se desea importar_

## Crear un archivo gitignore para la solucion

`dotnet new gitignore`

_La plantilla "archivo gitignore de dotnet" se creó correctamente._

## Iniciar el repositorio de Git

`git init`

## Agregar todos los archivos del proyecto al repositorio
`git add --all`


## Crear el primer commit
`git commit -m "Primera Clase de FullStack NET 6"`

## Agregar el repositorio remoto al Git
`git remote add origin https://github.com/erickorlando/musicproject.git`

## Subir las fuentes al repositorio remoto usando la rama principal master

`git push origin master`

## Inicializar un proyecto con User-Secrets
`dotnet user-secrets init`

## Establecer/modificar un valor para User-Secrets
`dotnet user-secrets set "seccion:clave" "valor"`

## Quitar un valor para User-Secrets
`dotnet user-secrets remove "seccion:clave"`

## Listar los valores establecidos en User-Secrets
`dotnet user-secrets list`