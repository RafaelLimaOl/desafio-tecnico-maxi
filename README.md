# 📘 Projeto — Documentação Geral
Aplicação Full-stack de um sistema de gestão financeira, construída com React, totalmente integrada à API REST do projeto desenvolvida utilizando .NET 9

# 🎨 FronEnd
Interface Web construída com React usando o framework NEXT.JS + ShadcnUI, totalmente integrada à API REST do projeto.

# 🧩 Stack Front-End
| Tecnologia          | Uso                             |
| ------------------- | ------------------------------- |
| **React 18**        | Biblioteca de UI                |
| **Shadcn**          | Componentes reutilizáveis       |
| **TailwindCSS**     | Estilização                     |
| **React Query**     | Cache, sincronização e mutações |
| **Axios**           | Cliente HTTP                    |
| **React Hook Form** | Gerenciamento de formulários    |
| **Zod**             | Validação de schemas            |
| **JWT**             | Controle de sessão              |
| **NEXT**            | Framework                       |

---

## 🔌 Conexão com o backend

- Integração com API via React Query (cache, refetch automático e loading states).
- Redirecionamento automático para o Dashboard após login.

## 🔐 Autenticação (Login & Registro)

- Tela com um formulário para Login e Cadastro de usuários.
- Validação utilizando Zod + React Hook Form.
- Armazenamento JWT na aplicação.

--- 
## 📊 Dashboard
- Sobre contrução
- TODO: Visão geral da plataforma com informações referente a toda informação cadastrada

## 👥 Pessoas | 🏷️ Categorias | 💰 Transações
- CRUD completo:
- Listagem em tabela.
- Funcionalidades de Criar, Editar e Excluir

## ⚙️ Configurações
- Listagem dos valroes
- Edição dos dados: Email e nome
- Deletar conta
  
---

## 🧷 Estrutura do Projeto
```
src/
  app/
  components/
    [auth]/
      categoria/
      config/
      pessoas/
      transacoes/
  hooks/
  lib/
  schema/
  services/
  types/
```

## 🏁 Como Executar o Front-End
```sh
npm i
npm run dev
```
Aplicação ficará disponível na url: http://localhost:3000

# Backend 
REST API criada utilizando .NET 9

# 🧩 Stack Back-End
| Tecnologia                   | Uso                             |
| ---------------------------- | ----------------------------------- |
| **Fluent Validation**        | Validação de requests               |
| **Dapper**                   | micro ORM                           |
| **JWT**                      | Tokens de autenticação com JWT      |
| **Open API Scalar**          | Documentação da API                 |
| **Layered Architecture**     | Controller - Service - Repository   |         

---

## 📄 **Documentação com OpenAPI usando Scalar**
- Documentação interativa da API utilizando **Scalar**.
- Permite testar endpoints diretamente pela interface.
- Configuração de segurança para autenticação via JWT.
- Interface intuitiva e simples de usar


## 🔐 **Autenticação e Autorização com JWT**
- Implementação de autenticação baseada em **JSON Web Tokens (JWT)**.
- Suporte a controle de acesso via **Claims** e **Roles**.
- Função para adquirir **userId** automaticamente para cada Endpoint

### 🗄️ **Banco de Dados SQL Server**
- Conexão com **SQL Server**.
- Utilização do micro Mapeador Objeto-Relacional  **DAPPER**
- Implementação com **Entity Framework Core**.

---

### 🌐 **Política de CORS Customizada**
- Restrição de origens, métodos e cabeçalhos.
- Suporte para ambientes especificos.

### 🧩 **Exceções e Erros Customizados**
- Respostas padronizadas e rastreamento de exceções.

### ✔️ **Validação com FluentValidation**
- Validação de Requests utilizando **FluentValidation**.
- Regras centralizadas e respostas padronizadas.

---

### 📦 **Estruturas de Dados**
- **Entity**: Modelos de banco de dados.
- **Request**: DTOs para entrada de dados.
- **Response**: DTOs para saída de dados.

---

### 🧩 **Arquitetura: Controller, Service, Repository**
- Controllers.
- Regras de negócio isoladas em Services.
- Persistência desacoplada via Repository Pattern.

### 📬 **Resposta de API Customizada**
- Estrutura consistente de retorno:
  - Sucesso
  - Mensagens
  - Erros
  - Dados

## 🧷 Estrutura do Projeto (REST)
```
Back/
  Controller/
  Interface/
    IServices/
    IRespository/
  Services/
  Repository/
  Models/
    Request/
    Response/
    Validator/
    Entity

```

---

## 🏁 Como Executar o Projeto

Execute a migração através do console do Gerenciador de Pacotes
```sh
Add-Migration <nome-migracação>
Update-Database
```

Ou execute o script do banco de dados no arquivo anexado:
---

## Futuras funcionalidade:
- Criação da tela de Dashboard
- Paginação para as entidades restantes
- Adição do Aspire e telemetria
- Logs estruturados

---

## 📄 Licença
Uso FREE, projetos pessoais e uso comercial.

