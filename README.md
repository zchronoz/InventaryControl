# InventoryControl
Desafio Técnico - Engenheiro de Software

Projeto: Inventary Control (C# - .NET Framework 4.6)

Projeto destinado a criar um CRUD de controle de inventário, onde o mesmo
possui um API para realizar as transações e uma interface visual em ASP .NET MVC.

Para rodar o projeto, é necessário ter um banco postgres e configurar com as pastas necessárias (pode ser utilizado o Migrations).
Com ele irá o script de criação de tabela, caso seja necessário.

Para facilitar o teste da api, está indo também um arquivo de rotas do Postman.

O projeto envia um email para cada equipamento cadastrado com um qrcode contendo os dados do cadastro.
(Os dados de acesso a esse email constam no web.config)

O projeto utiliza o ORM Entity Framework, no modelo Code First. As migrations estão configuradas e operacionais.
Como DI foi utilizado o Ninject.

A solution está com o projeto MVC e API separadas.
Caso seja necessário a configuração de multi deploy : https://docs.microsoft.com/pt-br/visualstudio/debugger/debug-multiple-processes?view=vs-2019

Configurar o banco:
1- Iniciar o banco postgres, com a database "DbInventoryControl" criada.
2- Definir o projeto inicial como .API
3- Abrir o Console do NuGet e utilizar os comandos:
-> add-migration InitialCreate
-> update-database -force

Para inicializar, recomendo executar ambos os projetos (multi deploy) 
informando que os projetos IC.MVC e IC.API serão de inicialização.

Versão online:
http://icmvc.gear.host/
