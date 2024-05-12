# crud-quest-back

- Instalar o Sql Server e deixar o banco rodando (aberto)
- Abrir o projeto e apagar a pasta migrations (para que se possa rodar o comando novamente para criar as tabelas no seu banco de dados)

-ter o dotnet 6 instalado
-olhar em dependencies e instalar os pacotes usados (Entity Framework etc) se precisar.

- commando para rodar a Migration e criar as tabelas no banco:  dotnet-ef migrations add CriacaoTabelasQuest (*Para rodar esse comando tem que ter as dependências intaladas*)
- quando o commando executar a migration vai ser criada.
   Para, enfim, criar as tabelas no SqlServer rodar o seguinte comando : dotnet-ef database update 

- (Talvez precise de ajustar a Connection String, se caso der erro que fica no arquivo appsettings.Development.json)
- Está para ser autenticado altomaticamente com o inicio do windows se tiver essa opção na hora de instalar/configurar, favor concondar.

- Depois de instalar tudo (se precisar) e rodar as migrations, a aplicação abrirá a tela do Swagger para testar a API, depois de fazer os testes é só buscar desativar para não abrir automaticamente




