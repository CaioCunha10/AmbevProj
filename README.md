 ---

# Projeto de Avaliação de Desenvolvedor

Este projeto consiste em uma API para gerenciamento de vendas, seguindo o padrão **DDD** (Domain-Driven Design). A aplicação está configurada para rodar com **Docker** e **Docker Compose**.

## Requisitos

- **Docker**: Para rodar os containers da aplicação e banco de dados.
- **Docker Compose**: Para orquestrar os containers.

## Como Executar
### 1. Verificar Containers Ativos

Antes de rodar a aplicação, é recomendado verificar se já existem containers ativos para evitar conflitos de porta ou duplicidade:

### 1.2 Subir os containers

Para rodar a aplicação e o banco de dados, utilize o seguinte comando:

```bash
docker-compose up --build
```

Este comando vai construir as imagens necessárias e iniciar os containers.

### 2. Rodar a API

Após subir os containers, a API estará disponível..

Você pode testar os endpoints da API utilizando ferramentas como **Postman** ou **cURL**.

### 2.1 instalar dependencias node
npm install


### 3. Rodar os Testes Unitários

Os testes unitários estão configurados para garantir o funcionamento correto da aplicação. Para rodá-los, execute:

```bash
docker-compose exec backend dotnet test
```

### 4. Endpoints

- **GET /api/vendas**: Lista todas as vendas.
- **POST /api/vendas**: Cria uma nova venda.
- **GET /api/vendas/{id}**: Recupera uma venda pelo ID.
- **PUT /api/vendas/{id}**: Atualiza os dados de uma venda.
- **DELETE /api/vendas/{id}**: Cancela uma venda.

## Estrutura do Projeto

```
/src
  /Api            # Controllers e lógica da API
  /Domain         # Modelos e regras de negócio
  /Application    # Serviços e lógica de aplicação
  /Infrastructure  # Banco de dados e integrações externas
  /Tests          # Testes unitários
```

## Solution
A solução foi desenvolvida dentro da solution Ambev.DeveloperEvaluation.sln. Você pode abrir a solução no Visual Studio ou utilizar o comando dotnet para rodar o projeto.

## Tecnologias Utilizadas

- **.NET 8.0**: Framework principal.
- **C#**: Linguagem utilizada no backend.
- **Docker**: Para execução do ambiente.
- **PostgreSQL**: Banco de dados.
- **xUnit**: Framework de testes unitários.
- **AutoMapper**: Para mapeamento de objetos.
- **AngularJs**: Frontend.
- **EntityFramework**: Object relational mapping.





---

 Projeto feito com dedicação, fiz com carinho pois, desejo muito fazer parte da NTT e evoluir em conjunto com a empresa.