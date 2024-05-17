# WakeApi

## Descrição

Este é um projeto ASP.NET Core Web API construído para realizar o CRUD de produtos com os seguintes requisitos funcionais:

- **Criar um produto**
  - O valor do produto não pode ser negativo
- Atualizar um produto
- Deletar um produto
- **Listar os produtos**
  - Visualizar um produto específico
  - Ordenar os produtos por diferentes campos
  - Buscar produto pelo nome

A solução utiliza o Entity Framework para interagir com o banco de dados e está configurada para funcionar com um banco de dados em memória para o desenvolvimento e os testes que buscam os dados já armazenados.

## Tecnologias Utilizadas

- **.NET 6 ou Superior**: O projeto utiliza o .NET 6 para garantir suporte a longo prazo e a utilização das últimas funcionalidades da plataforma.
- **Entity Framework Core**: Utilizado para a interação com o banco de dados. O projeto está configurado para utilizar o modo Code-First e com banco inMemory.
- **Code-First**: Utilizada abordagem Code-First do Entity Framework. Nesta abordagem, o modelo de dados é definido usando classes C# e o Entity Framework gera o esquema do banco de dados com base nesses modelos.

## Testes Unitários

- **xUnit**: Framework de testes para .NET.
- **Moq**: Biblioteca de mocking para .NET que permite criar objetos simulados (mocks) para testes unitários.

## GitHub Action

- **Validação Automática dos Testes**:
= Configurei o GitHub Actions para executar automaticamente os testes unitários do projeto sempre que um commit for enviado ao repositório. Sendo continuamente validado, ajudando a identificar problemas rapidamente e manter a qualidade do código.

- **Validação em Múltiplos Ambientes para Melhoria de Desempenho**:
Para melhorar o tempo de execução dos testes, utilizei paralelização de tarefas e caching das dependências. Essas técnicas permitem que os testes sejam executados mais rapidamente, evitando a restauração de dependências em cada execução e Os testes são validados tanto em ambientes Ubuntu quanto Windows, garantindo que o código seja compatível e funcione corretamente em diferentes sistemas operacionais.

## Camadas do Projeto

- **wakeApi.Api**: Camadas utilizada para Centralizar a lógica de apresentação e roteamento da aplicação, gerenciando as requisições HTTP e delegando as operações para as camadas de domínio e infraestrutura.
   - Configuração para Injeção de Depencência.
   - Global: BaseController para configuração da response do HTTP para manter padrão de retorno da API.
   - Controllers: requests do CRUD e listar produtos no banco.
     
- **wakeDomain.Domain**: Camadas utilizada para separar a lógica de negócio do resto da aplicação, facilitando a manutenção e evolução do sistema. Esta camada define as regras e conceitos fundamentais da aplicação.
  - Models: Entities de Product.
  - Interface para Service e Repository: separando a camada de banco de dados e de negócio.
  - Service: classe para regras de negócio e validação dos requisitos funcionais do CRUD. 

- **wakeInfra.Infra**: Camadas utilizada para abstrair a persistência de dados, permitindo que a camada de domínio permaneça agnóstica em relação aos detalhes de armazenamento. Facilita a troca de tecnologias de persistência se necessário.
  - Context: utilizada para configurar e gerenciar a conexão com o banco de dados e o mapeamento das entidades Product para tabelas no banco de dados.
  - Global: A finalidade dessa classe é garantir que o banco de dados seja populado com dados de exemplo sempre que a aplicação é iniciada, mas apenas se o banco de dados estiver vazio.
  - Repository: Encapsulamento de toda a lógica de acesso e manipulação dos dados da entidade Product utilizando Entity Framework Core.

- **wakeApi.Tests**: Camadas utilizada para garantir a qualidade e corretude do código através de testes automatizados. Os testes unitários permitem verificar individualmente a funcionalidade de cada componente da aplicação, facilitando a detecção de erros e regressões.
  - ControllerTest: Validam a lógica dos endpoints do controlador de produtos, garantindo que eles funcionem conforme esperado e retornem o http esperado.
  - RepositoriesTest: Validam as operações de acesso a dados realizadas pelos repositórios de produtos. Eles garantem que as operações CRUD (Create, Read, Update, Delete) funcionem corretamente com o banco de dados.
  - ServiceTest: Validam a lógica de negócios implementada nos serviços de produtos. Eles garantem que as operações realizadas pelo serviço estejam corretas e lidem adequadamente com as regras de negócio.

